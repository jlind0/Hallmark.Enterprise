using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using HallData.ApplicationViews;
using HallData.Exceptions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.Common;

namespace HallData.Repository
{
	public abstract class Repository : IRepository
	{
		protected Repository(Database db)
		{
			this.Database = db;
		}
		protected Database Database { get; private set; }

		protected virtual void PopulateUserIdParameter(DbCommand cmd, Guid? userId)
		{
			if(userId != null)
				cmd.AddParameter("__userguid", userId.Value);
		}

		protected virtual DbParameter PopulateCountParameter(DbCommand cmd)
		{
			return cmd.AddOutputParameter("count", DbType.Int64);
		}
		protected virtual DbParameter PopulateErrorCodeParameter(DbCommand cmd)
		{
			return cmd.AddOutputParameter("errorCode", DbType.String, 1000);
		}
		protected virtual DbParameter PopulateErrorTypeParameter(DbCommand cmd)
		{
			return cmd.AddOutputParameter("errorType", DbType.Int16);
		}
		protected virtual DbParameter PopulatedIsStatusChangedParameter(DbCommand cmd)
		{
			return cmd.AddOutputParameter("ischanged", DbType.Boolean);
		}
		protected virtual DbParameter PopulateWarningMessageParameter(DbCommand cmd)
		{
			return cmd.AddOutputParameter("warningmessage", DbType.String, 8000);
		}
		protected virtual void PopulateViewNameParameter(DbCommand cmd, string viewname)
		{
			if (!string.IsNullOrWhiteSpace(viewname))
				cmd.AddParameter("viewname", viewname);
		}
		protected virtual void PopulateStatusTypeNameParameter(DbCommand cmd, string statusTypeName)
		{
			cmd.AddParameter("statustypename", statusTypeName);
		}
		protected virtual void PopulateStatusChangeForceParameter(bool force, DbCommand cmd)
		{
			cmd.AddParameter("force", force);
		}
		protected virtual void PopulateCascadeParameter(DbCommand cmd, bool cascade)
		{
			cmd.AddParameter("cascade", cascade);
		}
		protected virtual void PopulateIsHardParameter(DbCommand cmd, bool isHard)
		{
			cmd.AddParameter("ishard", isHard);
		}
		protected virtual void ProcessErrorOutput(DbParameter errorCode, DbParameter errorType)
		{
			string code = errorCode.Value as string;
			if(code != null)
			{
				ErrorType type = (ErrorType)(short)errorType.Value;
				switch(type)
				{
					case ErrorType.Authentication: throw new GlobalizedAuthenticationException(code);
					case ErrorType.Authorization: throw new GlobalizedAuthorizationException(code);
					case ErrorType.Other: throw new GlobalizedException(code);
					case ErrorType.Validation: throw new GlobalizedValidationException(code);
				}
			}
		}
		protected  async Task<T> Execute<T>(DbCommand cmd, Func<Task<T>> action)
		{
			var errorCode = PopulateErrorCodeParameter(cmd);
			var errorType = PopulateErrorTypeParameter(cmd);
			var t = await action();
			ProcessErrorOutput(errorCode, errorType);
			return t;
		}
		protected async Task Execute(DbCommand cmd, Func<Task> action)
		{
			var errorCode = PopulateErrorCodeParameter(cmd);
			var errorType = PopulateErrorTypeParameter(cmd);
			await action();
			ProcessErrorOutput(errorCode, errorType);
		}
		protected T ExecuteSync<T>(DbCommand cmd, Func<T> action)
		{
			var errorCode = PopulateErrorCodeParameter(cmd);
			var errorType = PopulateErrorTypeParameter(cmd);
			var t = action();
			ProcessErrorOutput(errorCode, errorType);
			return t;
		}
		protected void ExecuteSync(DbCommand cmd, Action action)
		{
			var errorCode = PopulateErrorCodeParameter(cmd);
			var errorType = PopulateErrorTypeParameter(cmd);
			action();
			ProcessErrorOutput(errorCode, errorType);
		}
		protected struct ViewResult
		{
			public IEnumerable<string> AvailableProperties { get; set; }
			public IEnumerable<JObject> Results { get; set; }
			public ViewResult(IEnumerable<string> availableProperties, IEnumerable<JObject> results) : this()
			{
				this.AvailableProperties = availableProperties;
				this.Results = results;
			}
		}

		protected virtual Task<ViewResult> ExecuteViewResult(DbCommand cmd, CancellationToken token)
		{
			return Execute(cmd, () => ExecuteViewResultReader(cmd, token));
		}

		protected async Task<ViewResult> ExecuteViewResultReader(DbCommand cmd, CancellationToken token = default(CancellationToken))
		{
			ParentResultTokens parentTokens = null;
			var parentResults = new List<JObject>();
			var childResults = new Dictionary<string, List<KeyValuePair<JObject, JObject>>>();
			Dictionary<int, ChildResultTokens> childColumns = new Dictionary<int, ChildResultTokens>();

			await this.Database.ExecuteReader(cmd, (dr, properties) =>
			{
				if (parentTokens == null)
					parentTokens = new ParentResultTokens(properties);
				JObject key;
				var obj = PopulateViewItem(dr, parentTokens, out key);
				if (obj != null)
					parentResults.Add(obj);

			}, (dr, properties, level) =>
			{
				ChildResultTokens childTokens;
				if (!childColumns.TryGetValue(level, out childTokens))
				{
					childTokens = new ChildResultTokens(level, properties);
					childColumns[level] = childTokens;
				}
				foreach (var path in childTokens.GetPaths())
				{
					JObject key;
					var obj = PopulateViewItem(dr, childTokens, out key, path);
					if (obj != null)
					{
						List<KeyValuePair<JObject, JObject>> pathChildren;
						if (!childResults.TryGetValue(path, out pathChildren))
						{
							pathChildren = new List<KeyValuePair<JObject, JObject>>();
							childResults[path] = pathChildren;
						}
						pathChildren.Add(new KeyValuePair<JObject, JObject>(key, obj));
					}
				}
			}, token);

			foreach (var childToken in childColumns.OrderBy(c => c.Key).Select(c => c.Value))
			{
				foreach (var collectionToken in childToken.GetCollectionTokens())
				{
					List<KeyValuePair<JObject, JObject>> pathChildren;
					if (childResults.TryGetValue(collectionToken.ResultPath, out pathChildren))
						collectionToken.SetCollection(parentResults, pathChildren);
				}
			}
			foreach (var child in childResults.SelectMany(c => c.Value.Select(v => v.Value)).Union(parentResults))
			{
				Token.RemoveKey(child);
			}
			string[] availableProperties = new string[] { };
			if (parentTokens != null)
				availableProperties = parentTokens.GetColumnsForAllPaths().Union(childColumns.SelectMany(c => c.Value.GetColumnsForAllPaths())).Select(c => c.ColumnName).Distinct().ToArray();
			return new ViewResult(availableProperties, parentResults.ToArray());
		}

		private JObject PopulateViewItem(DbDataReader dr, ResultTokens tokens, out JObject key, string path = "")
		{
			key = PopulateKey(dr, tokens.GetKeys(path));
			if (key.HasValues)
			{
				var obj = PopulateViewItem(dr, tokens.GetColumns(path));
				if (obj != null)
				{
					Token.SetKey(obj, key);
					return obj;
				}
			}
			return null;
		}
		private JObject PopulateViewItem(DbDataReader dr, IEnumerable<ColumnToken> columns)
		{
			JObject obj = new JObject();
			bool hasValues = false;
			foreach (var column in columns.Where(c => !c.IsVirtual))
			{
				var value = dr[column.ColumnName];
				if (value != null && value != DBNull.Value)
				{
					hasValues = true;
					column.BuildToken.SetValue(obj, JToken.FromObject(value));
				}
			}
			if(hasValues)
				return obj;
			return null;
		}
		private JObject PopulateKey(DbDataReader dr, IEnumerable<ColumnToken> columns)
		{
			JObject obj = new JObject();
			foreach(var column in columns)
			{
				var value = dr[column.ColumnName];
				if (value != null && value != DBNull.Value)
					column.BuildToken.SetValue(obj, JToken.FromObject(value));
			}
			return obj;
		}

		protected async Task<QueryResults<T>> ReadQueryResults<T>(DbCommand cmd, string viewName = null, Guid? userID = null, FilterContext<T> filter = null, SortContext<T> sort = null, PageDescriptor page = null, DbParameter countParm = null, CancellationToken token = default(CancellationToken))
		{
			PopulateUserIdParameter(cmd, userID);
			PopulateViewNameParameter(cmd, viewName);
			QueryDescriptor<T> query = new QueryDescriptor<T>(filter, sort, page);
			query.AddInCommand(cmd);
			if (countParm == null)
				countParm = PopulateCountParameter(cmd);
			var views = await ExecuteViewResult(cmd, token);
			var values = views.Results.Select(v => v.ToObject<T>()).ToArray();
			long resultCount = countParm.Value as long? ?? 0;
			return new QueryResults<T>(resultCount, values, views.AvailableProperties);
		}

		protected async Task<QueryResult<T>> ReadQueryResult<T>(DbCommand cmd, Guid? userID = null, CancellationToken token = default(CancellationToken))
		{
			PopulateUserIdParameter(cmd, userID);
			var views = await ExecuteViewResultReader(cmd, token);
			var value = views.Results.Select(v => v.ToObject<T>()).FirstOrDefault();
			if (value == null)
				return null;
			return new QueryResult<T>(value, views.AvailableProperties);
		}

		protected virtual async Task<QueryResults<JObject>> ReadViews(DbCommand cmd, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			PopulateUserIdParameter(cmd, userId);
			PopulateViewNameParameter(cmd, viewName);
			QueryDescriptor queryDescriptor = new QueryDescriptor(filter, sort, page);
			queryDescriptor.AddInCommand(cmd);
			var countParm = PopulateCountParameter(cmd);
			var views = await ExecuteViewResult(cmd, token);
			return new QueryResults<JObject>((long)countParm.Value, views.Results, views.AvailableProperties);
		}

		protected virtual async Task<QueryResult<JObject>> ReadView(DbCommand cmd, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			PopulateUserIdParameter(cmd, userId);
			var views = await ExecuteViewResultReader(cmd, token);
			var view = views.Results.FirstOrDefault();
			if (view == null)
				return null;
			return new QueryResult<JObject>(view, views.AvailableProperties);

		}

		protected virtual async Task<IEnumerable<T>> ReadResults<T>(DbCommand cmd, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			PopulateUserIdParameter(cmd, userId);
			var views = await ExecuteViewResult(cmd, token);
			return views.Results.Select(v => v.ToObject<T>()).ToArray();
		}

		protected virtual async Task<T> ReadResult<T>(DbCommand cmd, Guid? userID = null, CancellationToken token = default(CancellationToken))
		{
			var results = await this.ReadResults<T>(cmd, userID, token);
			return results.FirstOrDefault();
		}

		protected virtual async Task<ChangeStatusResult> ExecuteChangeStatus(DbCommand cmd, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			PopulateUserIdParameter(cmd, userId);
			PopulateStatusTypeNameParameter(cmd, statusTypeName);
			PopulateStatusChangeForceParameter(force, cmd);
			var isChangedParm = PopulatedIsStatusChangedParameter(cmd);
			var warningMessageParm = PopulateWarningMessageParameter(cmd);
			await Execute(cmd, () => this.Database.ExecuteNonQueryAsync(cmd, token));
			return new ChangeStatusResult((bool)isChangedParm.Value, warningMessageParm.Value as string);
		}
	}

	public abstract class ReadOnlyRepository<TKey, TView> : Repository, IReadOnlyRepository<TKey, TView>
	{
		protected string GetAllStoredProcName { get; private set; }
		protected virtual void PopulateGetAllStoredProcedure(DbCommand cmd)
		{

		}

		protected ReadOnlyRepository(Database db, string getAllStoredProcName, string getStoredProcName)
			:base(db)
		{
			this.GetSqlQuery = getStoredProcName;
			this.GetAllStoredProcName = getAllStoredProcName;
		}
		
		protected string GetSqlQuery { get; private set; }

		public virtual Task<QueryResults<JObject>> GetView(string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			return ReadViews(cmd, viewName, userId, filter, sort, page, token);
		}

		public virtual Task<QueryResults<TView>> Get(string viewName = null, Guid? userId = null, FilterContext<TView> filter = null, SortContext<TView> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			Database db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			PopulateGetAllStoredProcedure(cmd);
			return ReadQueryResults<TView>(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public virtual Task<QueryResult<TView>> Get(TKey id, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			Database db = this.Database;
			var cmd = db.CreateSqlQueryCommand(this.GetSqlQuery);
			PopulateGetCommand(id, cmd);
			return ReadQueryResult<TView>(cmd, userId, token);
			
		}

		protected abstract void PopulateGetCommand(TKey id, DbCommand cmd);

		public virtual Task<QueryResult<JObject>> GetView(TKey id, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateSqlQueryCommand(this.GetSqlQuery);
			PopulateGetCommand(id, cmd);
			return ReadView(cmd, userId, token);
		}
	}

	public abstract class ReadOnlyRepository<TEnity> : ReadOnlyRepository<int, TEnity>, IReadOnlyRepository<TEnity>
	{
		protected ReadOnlyRepository(Database database, string getAllStoredProcName, string getStoredProcName)
			: base(database, getAllStoredProcName, getStoredProcName) { }
	}

	public abstract class Repository<TKey, TView, TViewForAdd, TViewForUpdate> : ReadOnlyRepository<TKey, TView>, IRepository<TKey, TView, TViewForAdd, TViewForUpdate>
		where TView : IHasKey<TKey>
		where TViewForAdd : IHasKey<TKey>
		where TViewForUpdate: IHasKey<TKey>
	{
		protected Repository(Database database, string getAllStoredProcName, string getStoredProcName, string addStoredProcedure, 
			string updateStoredProcedure, string changeStatusStoredProcedure)
			: base(database, getAllStoredProcName, getStoredProcName)
		{
			this.AddStoredProcedure = addStoredProcedure;
			this.UpdateStoredProcedure = updateStoredProcedure;
			this.ChangeStatusStoredProcedure = changeStatusStoredProcedure;
		}

		protected string AddStoredProcedure { get; private set; }
		protected string UpdateStoredProcedure { get; private set; }
		protected string ChangeStatusStoredProcedure { get; private set; }

		public virtual async Task Add(TViewForAdd view, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			Database db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.AddStoredProcedure); // can unit test
			PopulateUserIdParameter(cmd, userId); // tested by get
			PopulateAddCommand(view, cmd); // can unit test
			var obj = await Execute(cmd, () => db.ExecuteScalarAsync(cmd, token)); // tested by get
			view.Key = ReadKeyFromScalarReturnObject(obj, view); // cant test
		}

		protected virtual void PopulateAddCommand(TViewForAdd view, DbCommand cmd)
		{
			cmd.MapParameters(view, this.Database, ViewOperations.Add);
		}

		public virtual Task Update(TViewForUpdate view, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			Database db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.UpdateStoredProcedure); // can unit test
			PopulateUserIdParameter(cmd, userId); // tested by get
			PopulateUpdateCommand(view, cmd); // can unit test
			return Execute(cmd, () => db.ExecuteNonQueryAsync(cmd, token)); // tested by get
		}

		protected virtual void PopulateUpdateCommand(TViewForUpdate view, DbCommand cmd)
		{
			cmd.MapParameters(view, this.Database, ViewOperations.Update);
		}

		protected abstract TKey ReadKeyFromScalarReturnObject(object obj, TViewForAdd view);


		public virtual Task<ChangeStatusResult> ChangeStatus(TKey id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.ChangeStatusStoredProcedure);
			PopulateChangeStatusCommand(cmd, id);
			return ExecuteChangeStatus(cmd, statusTypeName, force, userId, token);
		}
		protected abstract void PopulateChangeStatusCommand(DbCommand cmd, TKey id);
		
	}

	public abstract class Repository<TKey, TView, TViewForAdd, TViewForUpdate, TViewForAddRelationship, TViewForUpdateRelationship> : Repository<TKey, TView, TViewForAdd, TViewForUpdate>, 
		IRepository<TKey, TView, TViewForAdd, TViewForUpdate, TViewForAddRelationship, TViewForUpdateRelationship>
		where TView : IHasKey<TKey>
		where TViewForAdd : IHasKey<TKey>
		where TViewForAddRelationship : IHasKey<TKey>
		where TViewForUpdate : IHasKey<TKey>
		where TViewForUpdateRelationship : IHasKey<TKey>
	{
		protected string AddRelationshipStoredProcedure { get; private set; }
		protected string UpdateRelationshipStoredProcedure { get; private set; }
		protected string ChangeStatusRelationshipStoredProcedure { get; private set; }
		protected Repository(Database database, string getAllStoredProcName, string getStoredProcName, string addStoredProcedure,
			string updateStoredProcedure, string changeStatusStoredProcedure, string addRelationshipStoredProcedure, string updateRelationshipStoredProcedure, string changeStatusRelationshipStoredProcedure)
			: base(database, getAllStoredProcName, getStoredProcName, addStoredProcedure, updateStoredProcedure, changeStatusStoredProcedure)
		{
			this.AddRelationshipStoredProcedure = addRelationshipStoredProcedure;
			this.UpdateRelationshipStoredProcedure = updateStoredProcedure;
			this.ChangeStatusRelationshipStoredProcedure = changeStatusRelationshipStoredProcedure;
		}

		public virtual Task AddRelationship(TViewForAddRelationship view, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.AddRelationshipStoredProcedure);
			PopulateAddRelationshipCommand(view, cmd);
			PopulateUserIdParameter(cmd, userId);
			return Execute(cmd, () => this.Database.ExecuteNonQueryAsync(cmd, token));
		}
		protected virtual void PopulateAddRelationshipCommand(TViewForAddRelationship view, DbCommand cmd)
		{
			cmd.MapParameters(view, this.Database, ViewOperations.Add);
		}
		public virtual Task UpdateRelationship(TViewForUpdateRelationship view, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.UpdateRelationshipStoredProcedure);
			PopulateUpdateRelationshipCommand(view, cmd);
			PopulateUserIdParameter(cmd, userId);
			return Execute(cmd, () => this.Database.ExecuteNonQueryAsync(cmd, token));
		}
		protected virtual void PopulateUpdateRelationshipCommand(TViewForUpdateRelationship view, DbCommand cmd)
		{
			cmd.MapParameters(view, this.Database, ViewOperations.Update);
		}
		public virtual Task<ChangeStatusResult> ChangeStatusRelationship(TKey id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand(this.ChangeStatusRelationshipStoredProcedure);
			PopulateChangeStatusRelationshipCommand(cmd, id);
			return ExecuteChangeStatus(cmd, statusTypeName, force, userId, token);
		}
		protected virtual void PopulateChangeStatusRelationshipCommand(DbCommand cmd, TKey key)
		{
			this.PopulateChangeStatusCommand(cmd, key);
		}
	}

	public abstract class Repository<TKey, TView, TViewBase> : Repository<TKey, TView, TViewBase, TViewBase>, IRepository<TKey, TView, TViewBase>
		where TView: IHasKey<TKey>
		where TViewBase: IHasKey<TKey>
	{
		protected Repository(Database database, string getAllStoredProcName, string getStoredProcName, string addStoredProcedure, string updateStoredProcedure,
			string changeStatusStoredProcedure)
			: base(database, getAllStoredProcName, getStoredProcName, addStoredProcedure, updateStoredProcedure, changeStatusStoredProcedure) { }
	}

	public abstract class Repository<TKey, TView> : Repository<TKey, TView, TView>, IRepository<TKey, TView>
		where TView: IHasKey<TKey>
	{
		protected Repository(Database database, string getAllStoredProcName, string getStoredProcName, string addStoredProcedure, string updateStoredProcedure, 
			string changeStatusStoredProcedure)
			: base(database, getAllStoredProcName, getStoredProcName, addStoredProcedure, updateStoredProcedure, changeStatusStoredProcedure) { }
	}

	public abstract class Repository<TView> : Repository<int, TView>, IRepository<TView>
		where TView : IHasKey
	{
		protected Repository(Database database, string getAllStoredProcName, string getStoredProcName, string addStoredProcedure, string updateStoredProcedure, 
			string changeStatusStoredProcedure)
			: base(database, getAllStoredProcName, getStoredProcName, addStoredProcedure, updateStoredProcedure, changeStatusStoredProcedure) { }
		protected override int ReadKeyFromScalarReturnObject(object obj, TView view)
		{
			return (obj as int?) ?? 0;
		}
	}

	public abstract class DeletableRepository<TKey, TView, TViewForAdd, TViewForUpdate> : Repository<TKey, TView, TViewForAdd, TViewForUpdate>, IDeletableRepository<TKey, TView, TViewForAdd, TViewForUpdate>
		where TView: IHasKey<TKey>
		where TViewForAdd : IHasKey<TKey>
		where TViewForUpdate: IHasKey<TKey>
	{
		protected DeletableRepository(Database database, string getAllStoredProcName, string getStoredProcName,
			string addStoredProcedure, string updateStoredProcedure, string deleteStoredProcedure, string changeStatusStoredProcedure)
			: base(database, getAllStoredProcName, getStoredProcName, addStoredProcedure, updateStoredProcedure, changeStatusStoredProcedure)
		{
			this.DeleteStoredProcedure = deleteStoredProcedure;
		}
		protected string DeleteStoredProcedure { get; private set; }
		
		public virtual Task Delete(TKey id, Guid? userId = null, bool cascade = false, bool isHard = false, CancellationToken token = default(CancellationToken))
		{
			Database db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.DeleteStoredProcedure);
			PopulateUserIdParameter(cmd, userId);
			PopulateDeleteCommand(id, cmd);
			PopulateCascadeParameter(cmd, cascade);
			PopulateIsHardParameter(cmd, isHard);
			return Execute(cmd, () => db.ExecuteNonQueryAsync(cmd, token));
		}
		protected abstract void PopulateDeleteCommand(TKey id, DbCommand cmd);
	}

	public abstract class DeletableRepository<TKey, TView, TViewForAdd, TViewForUpdate, TViewForAddRelationship, TViewForUpdateRelationship> : 
		Repository<TKey, TView, TViewForAdd, TViewForUpdate, TViewForAddRelationship, TViewForUpdateRelationship>, 
		IDeletableRepository<TKey, TView, TViewForAdd, TViewForUpdate, TViewForAddRelationship, TViewForUpdateRelationship>
		where TView: IHasKey<TKey>
		where TViewForAdd: IHasKey<TKey>
		where TViewForAddRelationship: IHasKey<TKey>
		where TViewForUpdate: IHasKey<TKey>
		where TViewForUpdateRelationship: IHasKey<TKey>
	{
		protected string DeleteStoredProcedure { get; private set; }
		protected string DeleteReleationshipStoredProcedure { get; private set; }
		protected DeletableRepository(Database database, string getAllStoredProcName, string getStoredProcName, string addStoredProcedure,
			string updateStoredProcedure, string changeStatusStoredProcedure, string addRelationshipStoredProcedure, string updateRelationshipStoredProcedure, 
			string changeStatusRelationshipStoredProcedure, string deleteStoredProcedure, string deleteRelationshipStoredProcedure)
			: base(database, getAllStoredProcName, getStoredProcName, addStoredProcedure, updateStoredProcedure, changeStatusStoredProcedure, addRelationshipStoredProcedure, updateRelationshipStoredProcedure, changeStatusRelationshipStoredProcedure)
		{
		}

		public virtual Task DeleteRelationship(TKey id, Guid? userId = null, bool cascade = false, bool isHard = false, CancellationToken token = default(CancellationToken))
		{
			Database db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.DeleteReleationshipStoredProcedure);
			PopulateUserIdParameter(cmd, userId);
			PopulateDeleteCommand(id, cmd);
			PopulateCascadeParameter(cmd, cascade);
			PopulateIsHardParameter(cmd, isHard);
			return Execute(cmd, () => db.ExecuteNonQueryAsync(cmd, token));
		}

		public virtual Task Delete(TKey id, Guid? userId = null, bool cascade = false, bool isHard = false, CancellationToken token = default(CancellationToken))
		{
			Database db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.DeleteStoredProcedure);
			PopulateUserIdParameter(cmd, userId);
			PopulateDeleteCommand(id, cmd);
			PopulateCascadeParameter(cmd, cascade);
			PopulateIsHardParameter(cmd, isHard);
			return Execute(cmd, () => db.ExecuteNonQueryAsync(cmd, token));
		}
		protected abstract void PopulateDeleteCommand(TKey id, DbCommand cmd);
		protected virtual void PopulateDeleteRelationshipCommand(TKey id, DbCommand cmd)
		{
			this.PopulateDeleteCommand(id, cmd);
		}
	}

	public abstract class DeletableRepository<TKey, TView, TViewBase> : DeletableRepository<TKey, TView, TViewBase, TViewBase>, IDeletableRepository<TKey, TView, TViewBase>
		where TView: IHasKey<TKey>
		where TViewBase: IHasKey<TKey>
	{
		protected DeletableRepository(Database database, string getAllStoredProcName, string getStoredProcName,
			string addStoredProcedure, string updateStoredProcedure, string deleteStoredProcedure, string changeStatusStoredProcedure)
			: base(database, getAllStoredProcName, getStoredProcName, addStoredProcedure, updateStoredProcedure, deleteStoredProcedure, changeStatusStoredProcedure) { }
	}

	public abstract class DeletableRepository<TKey, TView> : DeletableRepository<TKey, TView, TView>, IDeletableRepository<TKey, TView>
		where TView : IHasKey<TKey>
	{
		protected DeletableRepository(Database database, string getAllStoredProcName, string getStoredProcName,
			string addStoredProcedure, string updateStoredProcedure, string deleteStoredProcedure, string changeStatusStoredProcedure)
			: base(database, getAllStoredProcName, getStoredProcName, addStoredProcedure, updateStoredProcedure, deleteStoredProcedure, changeStatusStoredProcedure) { }
	}

	public abstract class DeletableRepository<TView> : DeletableRepository<int, TView>, IDeletableRepository<TView>
		where TView: IHasKey
	{
		protected DeletableRepository(Database database, string getAllStoredProcName, string getStoredProcName,
			string addStoredProcedure, string updateStoredProcedure, string deleteStoredProcedure, string changeStatusStoredProcedure)
			: base(database, getAllStoredProcName, getStoredProcName, addStoredProcedure, updateStoredProcedure, deleteStoredProcedure, changeStatusStoredProcedure) { }
		protected override int ReadKeyFromScalarReturnObject(object obj, TView view)
		{
			return (obj as int?) ?? 0;
		}
	}
}

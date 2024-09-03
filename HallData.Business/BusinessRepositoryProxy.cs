using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;
using System.Threading;
using HallData.Security;
using HallData.ApplicationViews;
using System.Reflection;
using HallData.Models;
using HallData.Validation;
using Newtonsoft.Json.Linq;

namespace HallData.Business
{
	public class BusinessProxy : IBusinessImplementation
	{
		protected ISecurityImplementation Security { get; private set; }
		public BusinessProxy(ISecurityImplementation security)
		{
			this.Security = security;
		}
		protected virtual async Task<Guid?> ActivateAndGetSignedInUserGuid(CancellationToken token)
		{
			await this.Security.MarkSessionActivity(token);
			var user = await this.Security.GetSignedInUser(token);
			return user != null ? user.UserGuid : null as Guid?;
		}
		protected virtual Task<IEnumerable<string>> GetModelAvailableProperties(IEnumerable<string> properties, CancellationToken token, Type modelType)
		{
			return Task.FromResult(GetModelAvailableProperties(properties, modelType));
		}
		private static IEnumerable<string> GetModelAvailableProperties(IEnumerable<string> propertes, Type type, string modelPath = "", string viewPath = "")
		{
			List<string> modelProperties = new List<string>();
			foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				string propertyPath = modelPath + property.Name;

				var mapAttr = property.GetCustomAttribute<MapToViewPropertyAttribute>(true);
				if (mapAttr != null && propertes.Any(p => p.ToLower() == (viewPath + mapAttr.PropertyPath).ToLower()))
					modelProperties.Add(propertyPath);
				else if (mapAttr is MapChildToModelPropertyAttribute)
					modelProperties.AddRange(GetModelAvailableProperties(propertes, property.PropertyType, string.Format("{0}{1}.", modelPath, property.Name),
						string.Format("{0}{1}.", viewPath, mapAttr.PropertyPath)));
				else if (mapAttr == null && property.GetCustomAttribute<ResultIgnoreAttribute>(true) == null)
					modelProperties.Add(propertyPath);
			}
			return modelProperties;
		}
		
		protected virtual Task<ChangeStatusQueryResult<T>> ChangeStatus<T>(Func<Guid?, Task<ChangeStatusResult>> changeStatusDelegate,
			Func<Task<QueryResult<T>>> getDelegate, CancellationToken token = default(CancellationToken))
		{
			return this.ExecuteQuery(async (userGuid) =>
			{
				var status = await changeStatusDelegate(userGuid);
				var result = await getDelegate();
				if (result != null)
					return new ChangeStatusQueryResult<T>(result.Result, result.AvailableProperties, status);
				return new ChangeStatusQueryResult<T>(status);
			}, token);
		}
		protected virtual async Task<T> ExecuteQuery<T>(Func<Guid?, Task<T>> query, CancellationToken token = default(CancellationToken))
		{
			var userGuid = await ActivateAndGetSignedInUserGuid(token);
			return await query(userGuid);
		}
		protected virtual Task<TResult> ExecuteAction<TInput, TResult>(TInput data, Func<TInput, Guid?, Task> action, Func<TInput, Guid?, Task<TResult>> result, Func<TInput, Guid?, Task<TInput>> transform = null, CancellationToken token = default(CancellationToken))
		{
			data.Validate();
			return ExecuteQuery(async userId => 
			{
				if (transform != null)
				{
					data = await transform(data, userId);
					data.Validate();
				}
				await action(data, userId);
				return await result(data, userId);
			}, token);
		}
		protected virtual Task<TResult> ExecuteAction<TResult>(Func<Guid?, Task> action, Func<Guid?, Task<TResult>> result, CancellationToken token = default(CancellationToken))
		{
			return ExecuteQuery(async userId =>
			{
				await action(userId);
				return await result(userId);
			}, token);
		}
		protected virtual async Task ExecuteAction(Func<Guid?, Task> action, CancellationToken token = default(CancellationToken))
		{
			var userId = await ActivateAndGetSignedInUserGuid(token);
			await action(userId);
		}
		public Guid? CurrentSessionId
		{
			get
			{
				return this.Security.CurrentSessionId;
			}
			set
			{
				this.Security.CurrentSessionId = value;
			}
		}

		public Task<bool> IsCurrentSessionActive(CancellationToken token = default(CancellationToken))
		{
			return this.Security.IsCurrentSessionActive(token);
		}

		public bool IsCurrentSessionActiveSync()
		{
			return this.Security.IsCurrentSessionActiveSync();
		}
	}
	public class BusinessRepositoryProxy<TRepository> : BusinessProxy
		where TRepository : IRepository
	{
		protected TRepository Repository { get; private set; }
		public BusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(security) { this.Repository = repository; }
	}


	public abstract class BusinessRepositoryProxy<TRepository, TKey, TView, TViewForAdd, TViewForUpdate, TModel, TModelForAdd, TModelForUpdate> : ReadOnlyBusinessRepositoryProxy<TRepository, TKey, TView, TModel>, IBusinessImplementation<TKey, TView, TModel, TModelForAdd, TModelForUpdate>
		where TRepository : IRepository<TKey, TView, TViewForAdd, TViewForUpdate>
		where TView : IHasKey<TKey>
		where TViewForAdd : IHasKey<TKey>
		where TViewForUpdate: IHasKey<TKey>
		where TModel : IHasKey<TKey>
		where TModelForAdd: IHasKey<TKey>
		where TModelForUpdate: IHasKey<TKey>
	{
		public BusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }


		public virtual Task<QueryResult<TModel>> Add(TModelForAdd view, CancellationToken token = default(CancellationToken))
		{
			if(!AreModelViewTypesEqual)
				view.Validate();
			return this.ExecuteAction(CreateViewForAdd(view), (data, userId) => this.Add(data, userId, token), 
				(data, userId) => this.Get(data.Key, token), token: token);
		}

		protected virtual Task Add(TViewForAdd view, Guid? userId, CancellationToken token)
		{
			return this.Repository.Add(view, userId, token);
		}

		public virtual Task<QueryResult<TModel>> Update(TModelForUpdate view, CancellationToken token = default(CancellationToken))
		{
			if(!AreModelViewTypesEqual)
				view.Validate();
			return this.ExecuteAction(CreateViewForUpdate(view), (data, userId) => this.Update(data, userId, token), 
				(data, userId) => this.Get(data.Key, token), token: token);
		}

		protected virtual Task Update(TViewForUpdate view, Guid? userId, CancellationToken token)
		{
			return this.Repository.Update(view, userId, token);
		}

		protected abstract TViewForAdd CreateViewForAdd(TModelForAdd model);

		protected abstract TViewForUpdate CreateViewForUpdate(TModelForUpdate model);

		protected virtual Task<ChangeStatusQueryResult<TModel>> ChangeStatus(TKey id, string statusTypeName, bool force, CancellationToken token)
		{
			return ChangeStatus(u => this.ChangeStatus(id, statusTypeName, force, u, token), () => this.Get(id, token), token);
		}

		protected virtual Task<ChangeStatusQueryResult<JObject>> ChangeStatusView(TKey id, string statusTypeName, bool force, CancellationToken token)
		{
			return ChangeStatus(u => this.ChangeStatus(id, statusTypeName, force, u, token), () => this.GetView(id, token), token);
		}

		public virtual Task<ChangeStatusQueryResult<TModel>> ChangeStatus(TKey id, string statusTypeName, CancellationToken token = default(CancellationToken))
		{
			return ChangeStatus(id, statusTypeName, false, token);
		}

		protected virtual Task<ChangeStatusResult> ChangeStatus(TKey id, string statusTypeName, bool force, Guid? userId, CancellationToken token)
		{
			return this.Repository.ChangeStatus(id, statusTypeName, force, userId, token);
		}
		
		public virtual Task<ChangeStatusQueryResult<TModel>> ChangeStatusForce(TKey id, string statusTypeName, CancellationToken token = default(CancellationToken))
		{
			return ChangeStatus(id, statusTypeName, true, token);
		}
		public virtual Task<QueryResult<JObject>> UpdateView(TModelForUpdate view, CancellationToken token = default(CancellationToken))
		{
			if (!AreModelViewTypesEqual)
				view.Validate();
			return this.ExecuteAction(CreateViewForUpdate(view), (data, userId) => this.Update(data, userId, token), 
				(data, userId) => this.GetView(data.Key, token), token: token);
		}

		public virtual Task<ChangeStatusQueryResult<JObject>> ChangeStatusView(TKey id, string statusTypeName, CancellationToken token = default(CancellationToken))
		{
			return ChangeStatusView(id, statusTypeName, false, token);
		}

		public virtual Task<ChangeStatusQueryResult<JObject>> ChangeStatusForceView(TKey id, string statusTypeName, CancellationToken token = default(CancellationToken))
		{
			return ChangeStatusView(id, statusTypeName, true, token);
		}

		public virtual Task<QueryResult<JObject>> AddView(TModelForAdd view, CancellationToken token = default(CancellationToken))
		{
			if (!AreModelViewTypesEqual)
				view.Validate();
			return this.ExecuteAction(CreateViewForAdd(view), (data, userId) => this.Add(data, userId, token), 
				(data, userId) => this.GetView(data.Key, token), token: token);
		}
	}
	public abstract class BusinessRepositoryProxy<TRepository, TKey, TView, TViewBase, TModel, TModelBase> : BusinessRepositoryProxy<TRepository, TKey, TView, TViewBase, TViewBase, TModel, TModelBase, TModelBase>,
		IBusinessImplementation<TKey, TView, TModel, TModelBase>
		where TView: IHasKey<TKey>
		where TViewBase: IHasKey<TKey>
		where TModel: IHasKey<TKey>
		where TModelBase: IHasKey<TKey>
		where TRepository: IRepository<TKey, TView, TViewBase>
	{
		public BusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
	}
	public abstract class BusinessRepositoryProxy<TRepository, TKey, TView, TModel> : BusinessRepositoryProxy<TRepository, TKey, TView, TView, TModel, TModel>, 
		IBusinessImplementation<TKey, TView, TModel>
		where TRepository : IRepository<TKey, TView>
		where TView: IHasKey<TKey>
		where TModel: IHasKey<TKey>
	{
		public BusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
	}
	public class BusinessRepositoryProxy<TRepository, TKey, TView> : BusinessRepositoryProxyWithBase<TRepository, TKey, TView, TView>
		where TRepository : IRepository<TKey, TView>
		where TView: IHasKey<TKey>
	{
		public BusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }

	}
	public class BusinessRepositoryProxy<TRepository, TView> : BusinessRepositoryProxy<TRepository, int, TView>, IBusinessImplementation<TView>
		where TRepository: IRepository<TView>
		where TView: IHasKey
	{
		public BusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
	}
}

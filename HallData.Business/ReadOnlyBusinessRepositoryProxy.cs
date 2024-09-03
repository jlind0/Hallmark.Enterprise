using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HallData.Repository;
using HallData.ApplicationViews;
using HallData.Security;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Reflection;

namespace HallData.Business
{
	public class ReadOnlyBusinessRepositoryProxy<TRepository, TView> : ReadOnlyBusinessRepositoryProxy<TRepository, int, TView>, IReadOnlyBusinessImplementation<TView>
		where TRepository : IReadOnlyRepository<TView>
	{
		public ReadOnlyBusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
	}

	public class ReadOnlyBusinessRepositoryProxy<TRepository, TKey, TView> : ReadOnlyBusinessRepositoryProxy<TRepository, TKey, TView, TView>, IReadOnlyBusinessImplementation<TKey, TView>
		where TRepository : IReadOnlyRepository<TKey, TView>
	{
		public ReadOnlyBusinessRepositoryProxy(TRepository repository, ISecurityImplementation security) : base(repository, security) { }
		protected override TView CreateModel(TView view)
		{
			return view;
		}
		protected override Task<IEnumerable<string>> GetModelAvailableProperties(IEnumerable<string> entityAvailableProperties, CancellationToken token)
		{
			return Task.FromResult(entityAvailableProperties);
		}
	}

	public abstract class ReadOnlyBusinessRepositoryProxy<TRepository, TKey, TView, TModel> : BusinessRepositoryProxy<TRepository>, IReadOnlyBusinessImplementation<TKey, TView, TModel>
		where TRepository: IReadOnlyRepository<TKey, TView>
	{
		private static readonly Lazy<bool> AreModelViewTypesEqualLazy = new Lazy<bool>(() => typeof(TModel) == typeof(TView), true);
		private static readonly Lazy<DefaultViewAttribute> DefaultViewLazy = new Lazy<DefaultViewAttribute>(() => typeof(TModel).GetCustomAttribute<DefaultViewAttribute>(true), true);
		protected static DefaultViewAttribute DefaultView
		{
			get { return DefaultViewLazy.Value; }
		}
		protected static bool AreModelViewTypesEqual
		{
			get { return AreModelViewTypesEqualLazy.Value; }
		}
		public ReadOnlyBusinessRepositoryProxy(TRepository repository, ISecurityImplementation security)
			: base(repository, security) { }
		
		protected virtual string GetDefaultViewName()
		{
			return DefaultView != null ? DefaultView.DefaultView : null;
		}
		
		protected virtual string GetDefaultViewNameForMany()
		{
			if (DefaultView == null)
				return GetDefaultViewName();
			return DefaultView.DefaultViewMany;
		}
		protected virtual Task<IEnumerable<string>> GetModelAvailableProperties(IEnumerable<string> properties, CancellationToken token)
		{
			if (AreModelViewTypesEqual)
				return Task.FromResult(properties);
			return GetModelAvailableProperties(properties, token, typeof(TModel));
		}
		public virtual Task<QueryResults<TModel>> GetMany(string viewName = null, FilterContext<TView> filter = null, SortContext<TView> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return ExecuteQuery(async (userid) =>
			{
				viewName = viewName ?? GetDefaultViewNameForMany();
				var results = await this.Repository.Get(viewName, userid, filter, sort, page, token);
				IEnumerable<TModel> models = null;
				if (!AreModelViewTypesEqual)
					models = results.Results.Select(r => CreateModel(r));
				else
					models = results.Results.Cast<TModel>();
				return new QueryResults<TModel>(results.TotalResultsCount, models, await GetModelAvailableProperties(results.AvailableProperties, token));
			}, token);
		   
		}

		public virtual Task<QueryResult<TModel>> Get(TKey id, CancellationToken token = default(CancellationToken))
		{
			return ExecuteQuery(async userGuid =>{
				var view = await this.Repository.Get(id, userGuid, token);
				if (view == null || view.Result == null)
					return null;
				return new QueryResult<TModel>(CreateModel(view.Result), await GetModelAvailableProperties(view.AvailableProperties, token));
			}, token);
		}

		protected abstract TModel CreateModel(TView view);

		public virtual Task<QueryResult<JObject>> GetView(TKey id, CancellationToken token = default(CancellationToken))
		{
			return ExecuteQuery(userGuid => this.Repository.GetView(id, userGuid, token), token);
		}

		public virtual Task<QueryResults<JObject>> GetManyView(string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			viewName = viewName ?? GetDefaultViewNameForMany();
			return ExecuteQuery(userGuid => this.Repository.GetView(viewName, userGuid, filter, sort, page, token));
		}
	}
}
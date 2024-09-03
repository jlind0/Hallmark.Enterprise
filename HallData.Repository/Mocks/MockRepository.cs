using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using System.Threading;
using HallData.Utilities;

namespace HallData.Repository.Mocks
{
	public abstract class ReadOnlyMockRepository<TKey, TView> : IReadOnlyRepository<TKey, TView>
		where TView : IHasKey<TKey>
	{
		public ReadOnlyMockRepository(IEnumerable<TView> views)
		{
			this.Views = views.ToDictionary(v => v.Key);
		}

		protected Dictionary<TKey, TView> Views { get; private set; }
		public virtual Task<QueryResults<TView>> Get(string viewName = null, Guid? userId = null, FilterContext<TView> filter = null, SortContext<TView> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			IEnumerable<TView> views = null;
			if (page != null)
				views = this.Views.Values.Skip(page.PageSize * (page.CurrentPage - 1)).Take(page.PageSize);
			else
				views = this.Views.Values;
			return Task.FromResult(new QueryResults<TView>(this.Views.LongCount(), views, null));
		}

		public virtual Task<QueryResult<TView>> Get(TKey id, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			TView view;
			if(this.Views.TryGetValue(id, out view))
				return Task.FromResult(new QueryResult<TView>(view, null));
			return Task.FromResult<QueryResult<TView>>(null);
		}

		public virtual Task<QueryResult<Newtonsoft.Json.Linq.JObject>> GetView(TKey id, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			if (!this.Views.ContainsKey(id))
				return Task.FromResult<QueryResult<Newtonsoft.Json.Linq.JObject>>(null);
			return Task.FromResult(new QueryResult<Newtonsoft.Json.Linq.JObject>(Newtonsoft.Json.Linq.JObject.FromObject(this.Views[id]), null));
		}

		public virtual Task<QueryResults<Newtonsoft.Json.Linq.JObject>> GetView(string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			IEnumerable<TView> views = null;
			if (page != null)
				views = this.Views.Values.Skip(page.PageSize * (page.CurrentPage - 1)).Take(page.PageSize);
			else
				views = this.Views.Values;
			return Task.FromResult(new QueryResults<Newtonsoft.Json.Linq.JObject>(this.Views.LongCount(), views.Select(v => Newtonsoft.Json.Linq.JObject.FromObject(v)).ToArray(), null));
		}
	}

	public abstract class MockRepository<TKey, TView, TViewForAdd, TViewForUpdate> : ReadOnlyMockRepository<TKey, TView>, IRepository<TKey, TView, TViewForAdd, TViewForUpdate>
		where TView : IHasKey<TKey>
		where TViewForAdd: IHasKey<TKey>
		where TViewForUpdate: IHasKey<TKey>
	{
		public MockRepository(IEnumerable<TView> views) : base(views) { }

		protected abstract TKey GetNextKey(TKey currentKey);

		public virtual Task Add(TViewForAdd view, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			view.Key = GetNextKey(view.Key);
			this.Views.Add(view.Key, view.CreateRelatedInstance<TView>());
			return Task.FromResult(false);
		}

		public virtual Task Update(TViewForUpdate view, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			this.Views[view.Key] = view.CreateRelatedInstance<TView>();
			return Task.FromResult(false);
		}

		public virtual Task<ChangeStatusResult> ChangeStatus(TKey id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(new ChangeStatusResult() { StatusChanged = false, WarningMessage = "Status change not implemented in base mock" });
		}
	}

	public abstract class MockRepositoryGuid<TView, TViewForAdd, TViewForUpdate> : MockRepository<Guid, TView, TViewForAdd, TViewForUpdate>
		where TView : IHasKey<Guid>
		where TViewForAdd : IHasKey<Guid>
		where TViewForUpdate : IHasKey<Guid>
	{
		public MockRepositoryGuid(IEnumerable<TView> views) : base(views) { }
		protected override Guid GetNextKey(Guid currentKey)
		{
			return Guid.NewGuid();
		}
	}

	public abstract class MockRepositoryInt<TView, TViewForAdd, TViewForUpdate> : MockRepository<int, TView, TViewForAdd, TViewForUpdate>
		where TView : IHasKey<int>
		where TViewForAdd : IHasKey<int>
		where TViewForUpdate : IHasKey<int>
	{
		public MockRepositoryInt(IEnumerable<TView> views) : base(views) { }
		protected override int GetNextKey(int currentKey)
		{
			if (this.Views.Keys.Count == 0)
				return 1;
			return this.Views.Keys.Max() + 1;
		}
	}

	public abstract class DeleteableMockRepository<TKey, TView, TViewForAdd, TViewForUpdate> : MockRepository<TKey, TView, TViewForAdd, TViewForUpdate>, IDeletableRepository<TKey, TView, TViewForAdd, TViewForUpdate>
		where TView : IHasKey<TKey>
		where TViewForAdd : IHasKey<TKey>
		where TViewForUpdate : IHasKey<TKey>
	{
		public DeleteableMockRepository(IEnumerable<TView> views) : base(views) { }

		public virtual Task Delete(TKey id, Guid? userId = null, bool cascade = false, bool isHard = false, CancellationToken token = default(CancellationToken))
		{
			this.Views.Remove(id);
			return Task.FromResult(false);
		}
	}

	public abstract class DeleteableMockRepositoryGuid<TView, TViewForAdd, TViewForUpdate> : DeleteableMockRepository<Guid, TView, TViewForAdd, TViewForUpdate>
		where TView : IHasKey<Guid>
		where TViewForAdd : IHasKey<Guid>
		where TViewForUpdate : IHasKey<Guid>
	{
		public DeleteableMockRepositoryGuid(IEnumerable<TView> views) : base(views) { }
		protected override Guid GetNextKey(Guid currentKey)
		{
			return Guid.NewGuid();
		}
	}

	public abstract class DeleteableMockRepositoryInt<TView, TViewForAdd, TViewForUpdate> : DeleteableMockRepository<int, TView, TViewForAdd, TViewForUpdate>
		where TView : IHasKey<int>
		where TViewForAdd : IHasKey<int>
		where TViewForUpdate : IHasKey<int>
	{
		public DeleteableMockRepositoryInt(IEnumerable<TView> views) : base(views) { }
		protected override int GetNextKey(int currentKey)
		{
			if (this.Views.Keys.Count == 0)
				return 1;
			return this.Views.Keys.Max() + 1;
		}
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace HallData.Tests.HallData.Web.Mocks
{
	class MockBusinessImplementation : IMockBusinessImplementation
	{
		public Task DeleteSoft(int id, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task DeleteHard(int id, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.QueryResult<MockApplicationView>> Add(MockApplicationView view, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.QueryResult<Newtonsoft.Json.Linq.JObject>> AddView(MockApplicationView view, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.QueryResult<MockApplicationView>> Update(MockApplicationView view, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.QueryResult<Newtonsoft.Json.Linq.JObject>> UpdateView(MockApplicationView view, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.ChangeStatusQueryResult<MockApplicationView>> ChangeStatus(int id, string statusTypeName, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.ChangeStatusQueryResult<Newtonsoft.Json.Linq.JObject>> ChangeStatusView(int id, string statusTypeName, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.ChangeStatusQueryResult<MockApplicationView>> ChangeStatusForce(int id, string statusTypeName, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.ChangeStatusQueryResult<Newtonsoft.Json.Linq.JObject>> ChangeStatusForceView(int id, string statusTypeName, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.QueryResults<MockApplicationView>> GetMany(string viewName = null, ApplicationViews.FilterContext<MockApplicationView> filter = null, ApplicationViews.SortContext<MockApplicationView> sort = null, ApplicationViews.PageDescriptor page = null, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.QueryResults<Newtonsoft.Json.Linq.JObject>> GetManyView(string viewName = null, ApplicationViews.FilterContext filter = null, ApplicationViews.SortContext sort = null, ApplicationViews.PageDescriptor page = null, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.QueryResult<MockApplicationView>> Get(int id, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task<ApplicationViews.QueryResult<Newtonsoft.Json.Linq.JObject>> GetView(int id, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Guid? CurrentSessionId
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public Task<bool> IsCurrentSessionActive(System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public bool IsCurrentSessionActiveSync()
		{
			throw new NotImplementedException();
		}
	}
}

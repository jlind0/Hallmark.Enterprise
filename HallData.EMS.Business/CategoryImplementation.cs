using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Business;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Data;
using HallData.Security;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{

	public class ReadOnlyCategoryImplementation :
		ReadOnlyBusinessRepositoryProxy<IReadOnlyCategoryRepository, CategoryResult>, IReadOnlyCategoryImplementation
	{
		public ReadOnlyCategoryImplementation(IReadOnlyCategoryRepository repository, ISecurityImplementation security) :
			base(repository, security)
		{

		}
	}

	public class CategoryImplementation :
		DeletableBusinessRepositoryProxyWithBase<ICategoryRepository, int, CategoryResult, CategoryForAdd, CategoryForUpdate>,
		ICategoryImplementation
	{
		protected IReadOnlyCategoryImplementation ReadOnly { get; private set; }

		public CategoryImplementation(
			ICategoryRepository repository, 
			ISecurityImplementation security, 
			IReadOnlyCategoryImplementation readOnly) :
			base(repository, security)
		{
			this.ReadOnly = readOnly;
		}

		public override Task<QueryResult<CategoryResult>> Get(int id, CancellationToken token = default(CancellationToken))
		{
			return ReadOnly.Get(id, token);
		}

		public override Task<QueryResults<CategoryResult>> GetMany(string viewName = null, FilterContext<CategoryResult> filter = null, SortContext<CategoryResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return ReadOnly.GetMany(viewName, filter, sort, page, token);
		}

		public override Task<QueryResults<JObject>> GetManyView(string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			return ReadOnly.GetManyView(viewName, filter, sort, page, token);
		}

		public override Task<QueryResult<JObject>> GetView(int id, CancellationToken token = default(CancellationToken))
		{
			return ReadOnly.GetView(id, token);
		}
	}
}

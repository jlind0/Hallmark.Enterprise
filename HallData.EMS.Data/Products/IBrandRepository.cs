using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.ApplicationViews.Enums;
using HallData.Repository;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
    public interface IReadOnlyBrandRepository : IReadOnlyProductBaseRepository<BrandResult>
    {
		Task<QueryResults<BrandResult>> GetBrands(CustomerId customerId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext<BrandResult> filter = null, SortContext<BrandResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		Task<QueryResults<JObject>> GetBrandsView(CustomerId customerId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		Task<QueryResult<BrandResult>> GetBrand(CustomerId customerId, Guid brandId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
		Task<QueryResult<JObject>> GetBrandView(CustomerId customerId, Guid brandId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }

    public interface IBrandRepository : IProductBaseRepository<BrandResult, BrandForAddBase, BrandForUpdateBase>, IReadOnlyBrandRepository { }
}

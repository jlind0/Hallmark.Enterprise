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
    public interface IReadOnlyBrandedProductRepository<TBrandedProductResult> : IReadOnlyProductRepository<TBrandedProductResult>
        where TBrandedProductResult : IBrandedProductResultBase
    {
	    Task<QueryResult<TBrandedProductResult>> Get(CustomerId customerId, Guid brandId, Guid productId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
		Task<QueryResult<JObject>> GetView(CustomerId customerId, Guid brandId, Guid productId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
		Task<QueryResults<SupplierResult>> GetSuppliers(CustomerId customerId, Guid brandId, Guid productId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
		Task<QueryResults<JObject>> GetSuppliersView(CustomerId customerId, Guid brandId, Guid productId, Guid? businessUnitId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }

    public interface IBrandedProductRepository<TBrandedProductResult, in TBrandedProductForAdd, in TBrandedProductForUpdate> : IReadOnlyBrandedProductRepository<TBrandedProductResult>,
        IProductRepository<TBrandedProductResult, TBrandedProductForAdd, TBrandedProductForUpdate>
        where TBrandedProductForAdd: IBrandedProductForAddBase
        where TBrandedProductForUpdate: IBrandedProductForUpdateBase
        where TBrandedProductResult: IBrandedProductResultBase
    {
		Task AddSupplier(CustomerId customerId, Guid brandId, Guid productId, SupplierId supplierId, Guid? businessUnitId = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
		Task RemoveSupplier(CustomerId customerId, Guid brandId, Guid productId, SupplierId supplierId, Guid? businessUnitId = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
}

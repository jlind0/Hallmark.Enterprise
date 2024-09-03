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
    public interface IReadOnlyProductRepository : IReadOnlyBrandedProductRepository<ProductResult>
    {
        Task<QueryResults<TProductResult>> GetProducts<TProductResult>(Guid productId, ProductTypes? productType = null, string viewName = null, Guid? userId = null, FilterContext<TProductResult> filter = null, SortContext<TProductResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
            where TProductResult: IProductResultBase;
        Task<QueryResults<JObject>> GetProductsView(Guid productId, ProductTypes? productType = null, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        Task<QueryResult<TProductResult>> GetParent<TProductResult>(Guid productId, ProductTypes parentProductType, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
            where TProductResult: IProductResultBase;
        Task<QueryResult<JObject>> GetParentView(Guid productId, ProductTypes parentProductType, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken));
    }
    public interface IProductRepository : IReadOnlyProductRepository, IBrandedProductRepository<ProductResult, ProductForAddBase, ProductForUpdateBase> { }
}

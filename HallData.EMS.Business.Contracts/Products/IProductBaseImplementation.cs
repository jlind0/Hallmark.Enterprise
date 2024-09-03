using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews;
using HallData.EMS.Models;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
	public interface IReadOnlyProductBaseImplementation<TProductResult> : IReadOnlyBusinessImplementation<Guid, TProductResult>
		where TProductResult: IProductBaseKey
	{
		// products

		[Description("Get products by customer")]
		Task<QueryResults<TProductResult>> GetByCustomer([Description("Target party id")]Guid partyID, Guid? customerOfId = null, string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext<TProductResult> filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext<TProductResult> sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		[Description("Get untyped products by customer")]
		Task<QueryResults<JObject>> GetByCustomerView([Description("Target party id")]Guid partyID, Guid? customerOfId = null, string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		[Description("Get all products by customer")]
		Task<QueryResults<TProductResult>> GetAllByCustomer([Description("Target party id")]Guid partyID, Guid? customerOfId = null, string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext<TProductResult> filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext<TProductResult> sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		[Description("Get all untyped products by customer")]
		Task<QueryResults<JObject>> GetAllByCustomerView([Description("Target party id")] Guid partyID, Guid? customerOfId = null, string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		[Description("Get products by brand by business unit")]
		Task<QueryResults<TProductResult>> GetByBusinessUnit(Guid businessUnitId, string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext<TProductResult> filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext<TProductResult> sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		[Description("Get untyped products by business unit")]
		Task<QueryResults<JObject>> GetByBusinessUnitView(Guid businessUnitId, string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
	}

	public interface IProductBaseImplementation<TProductResult, in TProductForAdd, in TProductForUpdate> :
		IDeletableBusinessImplementationWithBase<Guid, TProductResult, TProductForAdd, TProductForUpdate>, 
		IReadOnlyProductBaseImplementation<TProductResult>
		where TProductResult : IProductBaseKey
		where TProductForAdd: IProductBaseForAddBase
		where TProductForUpdate: IProductBaseForUpdateBase
	{
		
	}
}
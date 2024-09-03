using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using System.Threading;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.ApplicationViews;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Business
{
	public interface ICustomerImplementationBase<TCustomerImplementationResult> : IBusinessImplementation
		where TCustomerImplementationResult: ICustomerResult
	{
		[UpdateMethod]
		[ServiceRoute("ChangeStatusTypeCustomerRelationship", "{partyId}/CustomerRelationship/Status/{statusTypeName}/")]
		[ServiceRoute("ChangeStatusTypeCustomerRelationshipWithCustomerOf", "{partyId}/{customerOfId}/CustomerRelationship/Status/{statusTypeName}/")]
		[ServiceRoute("ChangeStatusTypeCustomerRelationshipTypedDefault", "{partyId}/CustomerRelationship/Status/{statusTypeName}/TypedView/")]
		[ServiceRoute("ChangeStatusTypeCustomerRelationshipWithCustomerOfTypedDefault", "{partyId}/{customerOfId}/CustomerRelationship/Status/{statusTypeName}/TypedView/")]
		[Description("Changes the status, if no warnings, of the customer relationship")]
		Task<ChangeStatusQueryResult<TCustomerImplementationResult>> ChangeStatusTypeCustomerRelationship([Description("Target party id")]Guid partyId, string statusTypeName,
			[Description("Target customer of id, if not provided will assume top level association to signed in user")]Guid? customerOfId = null, CancellationToken token = default(CancellationToken));
		
		[UpdateMethod]
		[ServiceRoute("ChangeStatusTypeCustomerRelationshipForce", "{partyId}/CustomerRelationship/Status/{statusTypeName}/Force/")]
		[ServiceRoute("ChangeStatusTypeCustomerRelationshipForceWithCustomerOf", "{partyId}/{customerOfId}/CustomerRelationship/Status/{statusTypeName}/Force/")]
		[ServiceRoute("ChangeStatusTypeCustomerRelationshipForceTypedDefault", "{partyId}/CustomerRelationship/Status/{statusTypeName}/Force/TypedView/")]
		[ServiceRoute("ChangeStatusTypeCustomerRelationshipForceWithCustomerOfTypedDefault", "{partyId}/{customerOfId}/CustomerRelationship/Status/{statusTypeName}/Force/TypedView/")]
		[Description("Changes the status, ignoring warnings, of the customer relationship")]
		Task<ChangeStatusQueryResult<TCustomerImplementationResult>> ChangeStatusTypeCustomerRelationshipForce([Description("Target party id")]Guid partyId, string statusTypeName,
			Guid? customerOfId = null, CancellationToken token = default(CancellationToken));
		
		[GetMethod]
		[ServiceRoute("GetCustomersOfCustomer", "{partyId}/Customers/")]
		[ServiceRoute("GetCustomersOfCustomerTyped", "{partyId}/Customers/TypedView/{viewName}/")]
		[ServiceRoute("GetCustomersOfCustomerTypedDefault", "{partyId}/Customers/TypedView/")]
		Task<QueryResults<CustomerResult>> GetCustomersOfCustomer(Guid partyId, string viewName = null, FilterContext<CustomerResult> filter = null, SortContext<CustomerResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		
		[GetMethod]
		[ServiceRoute("GetCustomersOfCustomerView", "{partyId}/Customers/View/{viewName}")]
		[ServiceRoute("GetCustomersOfCustomerViewDefault", "{partyId}/Customers/View/")]
		Task<QueryResults<JObject>> GetCustomersOfCustomerView(Guid partyId, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		
		// brands

		[GetMethod]
		[ServiceRoute("GetBrands", "{partyID}/brands")]
		[ServiceRoute("GetBrandsWithCustomerOf", "{partyID}/{customerOfId}/brands")]
		[ServiceRoute("GetBrandsTypedView", "{partyID}/brands/TypedView/{viewName}")]
		[ServiceRoute("GetBrandsWithCustomerOfTypedView", "{partyID}/{customerOfId}/brands/TypedView/{viewName}")]
		[ServiceRoute("GetBrandsTypedViewDefault", "{partyID}/brands/TypedView")]
		[ServiceRoute("GetBrandsWithCustomerOfTypedViewDefault", "{partyID}/{customerOfId}/brands/TypedView")]
		[Description("Gets brands by customer")]
		Task<QueryResults<BrandResult>> GetBrands([Description("Target party id")]Guid partyID, Guid? customerOfId = null,
			string viewName = null,
			[Description("Filter for brands, must be url encoded JSON")][JsonEncode]FilterContext<BrandResult> filter = null,
			[Description("Sort for brands, must be url encoded JSON")][JsonEncode]SortContext<BrandResult> sort = null,
			[Description("Page for brands, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		// brands view

		[GetMethod]
		[ServiceRoute("GetBrandsView", "{partyID}/brands/View/{viewName}")]
		[ServiceRoute("GetBrandsWithCustomerOfView", "{partyID}/{customerOfId}/brands/View/{viewName}")]
		[ServiceRoute("GetBrandsViewDefault", "{partyID}/brands/View")]
		[ServiceRoute("GetBrandsWithCustomerOfViewDefault", "{partyID}/{customerOfId}/brands/View")]
		[Description("Gets untyped brands by customer")]
		Task<QueryResults<JObject>> GetBrandsView([Description("Target party id")]Guid partyID, Guid? customerOfId = null,
			string viewName = null,
			[Description("Filter for brands, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
			[Description("Sort for brands, must be url encoded JSON")][JsonEncode]SortContext sort = null,
			[Description("Page for brands, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));

		// products

		[GetMethod]
		[ServiceRoute("GetProducts", "{partyID}/products")]
		[ServiceRoute("GetProductsWithCustomerOf", "{partyID}/{customerOfId}/products")]
		[ServiceRoute("GetProductsTypedView", "{partyID}/products/TypedView/{viewName}/")]
		[ServiceRoute("GetProductsWithCustomerOfTypedView", "{partyID}/{customerOfId}/products/TypedView/{viewName}/")]
		[ServiceRoute("GetProductsTypedViewDefault", "{partyID}/products/TypedView")]
		[ServiceRoute("GetProductsWithCustomerOfTypedViewDefault", "{partyID}/{customerOfId}/products/TypedView")]
		[Description("Gets Products by customer")]
		Task<QueryResults<ProductResult>> GetProducts([Description("Target party id")]Guid partyID, 
			Guid? customerOfId = null,
			string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext<ProductResult> filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext<ProductResult> sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, 
			CancellationToken token = default(CancellationToken));

		// products view

		[GetMethod]
		[ServiceRoute("GetProductsView", "{partyID}/products/View/{viewName}/")]
		[ServiceRoute("GetProductsViewDefault", "{partyID}/products/View")]
		[ServiceRoute("GetProductsWithCustomerOfView", "{partyID}/{customerOfId}/products/View/{viewName}/")]
		[ServiceRoute("GetProductsWithCustomerOfViewDefault", "{partyID}/{customerOfId}/products/View")]
		[Description("Gets products by customer")]
		Task<QueryResults<JObject>> GetProductsView([Description("Target party id")]Guid partyID, 
			Guid? customerOfId = null,
			string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, 
			CancellationToken token = default(CancellationToken));

		// all products

		[GetMethod]
		[ServiceRoute("GetAllProducts", "{partyID}/products")]
		[ServiceRoute("GetAllProductsWithCustomerOf", "{partyID}/{customerOfId}/products")]
		[ServiceRoute("GetAllProductsTypedView", "{partyID}/products/TypedView/{viewName}")]
		[ServiceRoute("GetAllProductsWithCustomerOfTypedView", "{partyID}/{customerOfId}/products/TypedView/{viewName}")]
		[ServiceRoute("GetAllProductsTypedViewDefault", "{partyID}/products/TypedView")]
		[ServiceRoute("GetAllProductsWithCustomerOfTypedViewDefault", "{partyID}/{customerOfId}/products/TypedView")]
		[Description("Gets All Products by customer")]
		Task<QueryResults<ProductResult>> GetAllProducts([Description("Target party id")]Guid partyID, Guid? customerOfId = null,
			string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext<ProductResult> filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext<ProductResult> sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, 
			CancellationToken token = default(CancellationToken));

		// all products view

		[GetMethod]
		[ServiceRoute("GetAllProductsView", "{partyID}/products/View/{viewName}")]
		[ServiceRoute("GetAllProductsWithCustomerOfView", "{partyID}/{customerOfId}/products/View/{viewName}")]
		[ServiceRoute("GetAllProductsViewDefault", "{partyID}/products/View")]
		[ServiceRoute("GetAllProductsWithCustomerOfViewDefault", "{partyID}/{customerOfId}/products/View")]
		[Description("Gets All products by customer")]
		Task<QueryResults<JObject>> GetAllProductsView([Description("Target party id")]Guid partyID,
			Guid? customerOfId = null, 
			string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, 
			CancellationToken token = default(CancellationToken));


		// business units

		[GetMethod]
		[ServiceRoute("GetProducts", "{partyID}/products")]
		[ServiceRoute("GetProductsWithCustomerOf", "{partyID}/{customerOfId}/products")]
		[ServiceRoute("GetProductsTypedView", "{partyID}/products/TypedView/{viewName}")]
		[ServiceRoute("GetProductsWithCustomerOfTypedView", "{partyID}/{customerOfId}/products/TypedView/{viewName}")]
		[ServiceRoute("GetProductsTypedViewDefault", "{partyID}/products/TypedView")]
		[ServiceRoute("GetProductsWithCustomerOfTypedViewDefault", "{partyID}/{customerOfId}/products/TypedView")]
		[Description("Gets business units by customer")]
		Task<QueryResults<ProductResult>> GetBusinessUnits([Description("Target party id")]Guid partyID,
			Guid? customerOfId = null,
			string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext<ProductResult> filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext<ProductResult> sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null,
			CancellationToken token = default(CancellationToken));

		// business units view

		[GetMethod]
		[ServiceRoute("GetProductsView", "{partyID}/products/View/{viewName}")]
		[ServiceRoute("GetProductsWithCustomerOfView", "{partyID}/{customerOfId}/products/View/{viewName}")]
		[ServiceRoute("GetProductsViewDefault", "{partyID}/products/View")]
		[ServiceRoute("GetProductsWithCustomerOfViewDefault", "{partyID}/{customerOfId}/products/View")]
		[Description("Gets business units by customer")]
		Task<QueryResults<JObject>> GetBusinessUnitsView([Description("Target party id")]Guid partyID,
			Guid? customerOfId = null,
			string viewName = null,
			[Description("Filter for products, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
			[Description("Sort for products, must be url encoded JSON")][JsonEncode]SortContext sort = null,
			[Description("Page for products, must be url encoded JSON")][JsonEncode]PageDescriptor page = null,
			CancellationToken token = default(CancellationToken));
	}

	public interface IReadOnlyCustomerImplementation<TCustomerResult> : IReadOnlyPartyImplementation<CustomerId, TCustomerResult>
		where TCustomerResult: ICustomerResult
	{
		[GetMethod]
		[ServiceRoute("GetCustomer", "{partyId}/")]
		[ServiceRoute("GetCustomerWithCustomerOf", "{partyId}/{customerOfId}/")]
		[ServiceRoute("GetCustomerTypedDefault", "{partyId}/TypedView/")]
		[ServiceRoute("GetCustomerWithCustomerOfTypedDefault", "{partyId}/{customerOfId}/TypedView/")]
		[Description("Gets a strongly customer by party id and customer of id")]
		Task<QueryResult<TCustomerResult>> GetCustomer([Description("Target party id")]Guid partyId, 
			Guid? customerOfId = null, 
			CancellationToken token = default(CancellationToken));
		
		[GetMethod]
		[ServiceRoute("GetCustomerView", "{partyId}/View/{viewName}/")]
		[ServiceRoute("GetCustomerViewDefault", "{partyId}/View/")]
		[ServiceRoute("GetCustomerViewDefaultWithCustomerOf", "{partyId}/{customerOfId}/View/")]
		[Description("Gets a dynamically typed customer view by party id and customer of id")]
		Task<QueryResult<JObject>> GetCustomerView([Description("Target party id")]Guid partyId, 
			Guid? customerOfId = null, 
			CancellationToken token = default(CancellationToken));
	}

	public interface IReadOnlyCustomerImplementation : IReadOnlyCustomerImplementation<CustomerResult> { }

	public interface ICustomerImplementation<TCustomerResult, TCustomerForAdd, TCustomerForUpdate, TCustomerImplementationResult> :
		IPartyImplementation<CustomerId, TCustomerResult, TCustomerForAdd, TCustomerForUpdate>, 
		ICustomerImplementationBase<TCustomerImplementationResult>, 
		IReadOnlyCustomerImplementation<TCustomerResult>
		where TCustomerResult: ICustomerResult
		where TCustomerForAdd : ICustomerForAdd
		where TCustomerForUpdate: ICustomerForUpdate
		where TCustomerImplementationResult: ICustomerResult
	{
		[UpdateMethod]
		[ServiceRoute("ChangeStatusCustomer", "{partyId}/Status/{statusTypeName}/")]
		[ServiceRoute("ChangeStatusCustomerWithCustomerOf", "{partyId}/{customerOfId}/Status/{statusTypeName}/")]
		[ServiceRoute("ChangeStatusCustomerTyped", "{partyId}/Status/{statusTypeName}/TypedView/{viewName}/")]
		[ServiceRoute("ChangeStatusCustomerWithCustomerOfTyped", "{partyId}/{customerOfId}/Status/{statusTypeName}/TypedView/{viewName}/")]
		[ServiceRoute("ChangeStatusCustomerTypedDefault", "{partyId}/Status/{statusTypeName}/TypedView/")]
		[ServiceRoute("ChangeStatusCustomerWithCustomerOfTypedDefault", "{partyId}/{customerOfId}/Status/{statusTypeName}/TypedView/")]
		[Description("Changes the status, if no warnings, of a customer")]
		Task<ChangeStatusQueryResult<TCustomerResult>> ChangeStatusCustomer([Description("Target party id")]Guid partyId,
			string statusTypeName, 
			Guid? customerOfId = null, 
			string viewName = null, 
			CancellationToken token = default(CancellationToken));
		
		[UpdateMethod]
		[ServiceRoute("ChangeStatusCustomerForce", "{partyId}/Status/{statusTypeName}/Force/")]
		[ServiceRoute("ChangeStatusCustomerForceWithCustomerOf", "{partyId}/{customerOfId}/Status/{statusTypeName}/Force/")]
		[ServiceRoute("ChangeStatusCustomerForceTyped", "{partyId}/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
		[ServiceRoute("ChangeStatusCustomerForceWithCustomerOfTyped", "{partyId}/{customerOfId}/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
		[ServiceRoute("ChangeStatusCustomerForceTypedDefault", "{partyId}/Status/{statusTypeName}/Force/TypedView/")]
		[ServiceRoute("ChangeStatusCustomerForceWithCustomerOfTypedDefault", "{partyId}/{customerOfId}/Status/{statusTypeName}/Force/TypedView/")]
		[Description("Changes the status, ignoring warnings, of a customer")]
		Task<ChangeStatusQueryResult<TCustomerResult>> ChangeStatusCustomerForce([Description("Target party id")]Guid partyId,
			string statusTypeName, 
			Guid? customerOfId = null, 
			string viewName = null, 
			CancellationToken token = default(CancellationToken));
		
		[DeleteMethod]
		[ServiceRoute("DeleteCustomer", "{partyId}/Soft/")]
		[ServiceRoute("DeleteCustomerDefault", "{partyId}/")]
		[ServiceRoute("DeleteCustomerWithCustomerOf", "{partyId}/{customerOfId}/Soft/")]
		[ServiceRoute("DeleteCustomerDefaultWithCustomerOf", "{partyId}/{customerOfId}/")]
		[Description("Soft deletes a customer")]
		Task DeleteCustomerSoft([Description("Target party id")]Guid partyId, 
			Guid? customerOfId = null, 
			CancellationToken token = default(CancellationToken));
		
		[DeleteMethod]
		[ServiceRoute("DeleteHardCustomer", "{partyId}/Hard/")]
		[ServiceRoute("DeleteHardCustomerWithCustomerOf", "{partyId}/{customerOfId}/Hard/")]
		[Description("Hard deletes a customer")]
		Task DeleteCustomerHard([Description("Target party id")]Guid partyId, 
			Guid? customerOfId = null, 
			CancellationToken token = default(CancellationToken));
	}

	public interface ICustomerImplementation<TCustomerResult, TCustomerForAdd, TCustomerForUpdate> : ICustomerImplementation<TCustomerResult, TCustomerForAdd, TCustomerForUpdate, TCustomerResult>
		where TCustomerResult: ICustomerResult
		where TCustomerForAdd : ICustomerForAdd
		where TCustomerForUpdate: ICustomerForUpdate
	{ }

	[Service("customers")]
	public interface ICustomerImplementation : ICustomerImplementation<CustomerResult, CustomerForAdd, CustomerForUpdate>, IReadOnlyCustomerImplementation { }
}

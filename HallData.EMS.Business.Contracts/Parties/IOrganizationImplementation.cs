using HallData.EMS.ApplicationViews.Results;
using System;
using HallData.Business;
using HallData.ApplicationViews;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;
namespace HallData.EMS.Business
{
	[Service("organizations")]
	public interface IOrganizationImplementation : IOrganizationImplementation<Guid, OrganizationResult, OrganizationForAdd, OrganizationForUpdate>, IReadOnlyOrganizationImplementation
	{
		[GetMethod]
		[ServiceRoute("GetCustomers", "{organizationId}/Customers/")]
		[ServiceRoute("GetCustomersTyped", "{organizationId}/Customers/TypedView/{viewName}/")]
		[ServiceRoute("GetCustomersTypedDefault", "{organizationId}/Customers/TypedView/")]
		Task<QueryResults<CustomerResult>> GetCustomers(Guid organizationId, string viewName = null, FilterContext<CustomerResult> filter = null, SortContext<CustomerResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		[GetMethod]
		[ServiceRoute("GetCustomersView", "{organizationId}/Customers/View/{viewName}")]
		[ServiceRoute("GetCustomersViewDefault", "{organizationId}/Customers/View/")]
		Task<QueryResults<JObject>> GetCustomersView(Guid organizationId, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
	}

	public interface IOrganizationImplementation<TKey, TOrganizationResult, in TOrganizationForAdd, in TOrganizationForUpdate> :
		IReadOnlyOrganizationImplementation<TKey, TOrganizationResult>, IPartyImplementation<TKey, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>
		where TOrganizationResult: IOrganizationResult<TKey>
		where TOrganizationForAdd : IOrganizationForAdd<TKey>
		where TOrganizationForUpdate : IOrganizationForUpdate<TKey>
	{
		[GetMethod]
		[ServiceRoute("GetBusinessUnits", "{organizationID}/BusinessUnits/")]
		[ServiceRoute("GetBusinessUnitsTyped", "{organizationID}/BusinessUnits/TypedView/{viewName}/")]
		[ServiceRoute("GetBusinessUnitsTypedDefault", "{organizationID}/BusinessUnits/TypedView/")]
		[Description("Get business units for an organization")]
		Task<QueryResults<BusinessUnitResult>> GetBusinessUnits([Description("Target organization id")]Guid organizationID, string viewName = null,
			[Description("Filter for business units, must be url encoded JSON")][JsonEncode]FilterContext<BusinessUnitResult> filter = null,
			[Description("Sort for business units, must be url encoded JSON")][JsonEncode]SortContext<BusinessUnitResult> sort = null,
			[Description("Page for business units, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		
		[GetMethod]
		[ServiceRoute("GetBusinessUnitsView", "{organizationID}/BusinessUnits/View/{viewName}/")]
		[ServiceRoute("GetBusinessUnitsDefault", "{organizationID}/BusinessUnits/View/")]
		[Description("Get untyped business units for an organization")]
		Task<QueryResults<JObject>> GetBusinessUnitsView([Description("Target organization id")]Guid organizationID, string viewName = null,
			[Description("Filter for business units, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
			[Description("Sort for business units, must be url encoded JSON")][JsonEncode]SortContext sort = null,
			[Description("Page for business units, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		
		[GetMethod]
		[ServiceRoute("GetEmployees", "{organizationID}/Employees/")]
		[ServiceRoute("GetEmployeesTyped", "{organizationID}/Employees/TypedView/{viewName}/")]
		[ServiceRoute("GetEmployeesTypedDefault", "{organizationID}/Employees/TypedView/")]
		[Description("Get employees for an organization")]
		Task<QueryResults<EmployeeResult>> GetEmployees([Description("Target organization id")]Guid organizationID, string viewName = null,
			[Description("Filter for employees, must be url encoded JSON")][JsonEncode]FilterContext<EmployeeResult> filter = null,
			[Description("Sort for employees, must be url encoded JSON")][JsonEncode]SortContext<EmployeeResult> sort = null,
			[Description("Page for employees, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		
		[GetMethod]
		[ServiceRoute("GetEmployeesView", "{organizationID}/Employees/View/{viewName}/")]
		[ServiceRoute("GetEmployeesViewDefault", "{organizationID}/Employees/View/")]
		[Description("Get untyped employees for an organization")]
		Task<QueryResults<JObject>> GetEmployeesView([Description("Target organization id")]Guid organizationID, string viewName = null,
			[Description("Filter for employees, must be url encoded JSON")][JsonEncode]FilterContext filter = null,
			[Description("Sort for employees, must be url encoded JSON")][JsonEncode]SortContext sort = null,
			[Description("Page for employees, must be url encoded JSON")][JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
		
		[GetMethod]
		[ServiceRoute("GetEmployee", "{organizationID}/Employees/{employeeID}/")]
		[ServiceRoute("GetEmployeeTyped", "{organizationID}/Employees/{employeeID}/TypedView/{viewName}/")]
		[ServiceRoute("GetEmployeeTypedDefault", "{organizationID}/Employees/{employeeID}/TypedView/")]
		[Description("Get employee for an organization")]
		Task<QueryResult<EmployeeResult>> GetEmployee([Description("Target organization id")]Guid organizationID, [Description("Target employee id")] Guid employeeID, string viewName = null, CancellationToken token = default(CancellationToken));
		
		[GetMethod]
		[ServiceRoute("GetEmployeeView", "{organizationID}/Employees/{employeeID}/View/{viewName}/")]
		[ServiceRoute("GetEmployeeViewDefault", "{organizationID}/Employees/{employeeID}/View/")]
		[Description("Get untyped employee for an organization")]
		Task<QueryResult<JObject>> GetEmployeeView([Description("Target organization id")]Guid organizationID, [Description("Target employee id")] Guid employeeID, string viewName = null, CancellationToken token = default(CancellationToken));
	}
	public interface IReadOnlyOrganizationImplementation<TKey, TOrganizationResult> : IReadOnlyPartyImplementation<TKey, TOrganizationResult>
		where TOrganizationResult : IOrganizationResult<TKey> { }
	public interface IReadOnlyOrganizationImplementation : IReadOnlyOrganizationImplementation<Guid, OrganizationResult> { }
}
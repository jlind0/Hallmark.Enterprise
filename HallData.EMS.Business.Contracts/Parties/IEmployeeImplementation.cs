using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
    public interface IReadOnlyEmployeeImplementation : IReadOnlyPersonImplementation<EmployeeId, EmployeeResult>
    {
        [GetMethod]
        [ServiceRoute("GetEmployee", "{personID}/")]
        [ServiceRoute("GetEmployeeWithEmployer", "{personID}/{employerID}/")]
        [ServiceRoute("GetEmployeeTyped", "{personID}/TypedView/{viewName}/")]
        [ServiceRoute("GetEmployeeWithEmployerTyped", "{personID}/{employerID}/TypedView/{viewName}/")]
        [ServiceRoute("GetEmployeeTypedDefault", "{personID}/TypedView/")]
        [ServiceRoute("GetEmployeeWithEmployerTypedDefault", "{personID}/{employerID}/TypedView/")]
        Task<QueryResult<EmployeeResult>> GetEmployee(Guid personID, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetEmployeeView", "{personID}/View/{viewName}/")]
        [ServiceRoute("GetEmployeeViewWithEmployer", "{personID}/{employerID}/View/{viewName}")]
        [ServiceRoute("GetEmployeeViewDefault", "{personID}/View/")]
        [ServiceRoute("GetEmployeeViewDefaultWithEmployer", "{personID}/{employerID}/View/")]
        Task<QueryResult<JObject>> GetEmployeeView(Guid personID, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetByEmployer", "Employer/{employerID}/")]
        [ServiceRoute("GetByEmployerTyped", "Employer/{employerID}/TypedView/{viewName}/")]
        [ServiceRoute("GetByEmployerTypedDefault", "Employer/{employerID}/TypedView/")]
        Task<QueryResults<EmployeeResult>> GetByEmployer(Guid employerID, string viewName = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetByEmployerView", "Employer/{employerID}/View/{viewName}/")]
        [ServiceRoute("GetByEmployerViewDefault", "Employer/{employerID}/View/")]
        Task<QueryResults<JObject>> GetByEmployerView(Guid employerID, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    [Service("employees")]
    public interface IEmployeeImplementation : IPersonImplementation<EmployeeId, EmployeeResult, EmployeeForAdd, EmployeeForUpdate>, IReadOnlyEmployeeImplementation
    {
        [DeleteMethod]
        [ServiceRoute("DeleteEmployeeSoft", "{personID}/Soft/")]
        [ServiceRoute("DeleteEmployeeSoftWithEmployer", "{personID}/{employerID}/Soft/")]
        [ServiceRoute("DeleteEmployeeDefault", "{personID}/")]
        [ServiceRoute("DeleteEmployeeDefaultWithEmployer", "{personID}/{employerID}/")]
        Task DeleteEmployeeSoft(Guid personID, Guid? employerID = null, CancellationToken token = default(CancellationToken));
        [DeleteMethod]
        [ServiceRoute("DeleteEmployeeHard", "{personID}/Hard")]
        [ServiceRoute("DeleteEmployeeHardWithEmployer", "{personID}/{employerID}/Hard/")]
        Task DeleteEmployeeHard(Guid personID, Guid? employerID = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmployee", "{personID}/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusEmployeeWithEmployer", "{personID}/{employerID}/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusEmployeeTyped", "{personID}/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmployeeWithEmployerTyped", "{personID}/{employerID}/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmployeeTypedDefault", "{personID}/Status/{statusTypeName}/TypedView/")]
        [ServiceRoute("ChangeStatusEmployeeWithEmployerTypedDefault", "{personID}/{employerID}/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<EmployeeResult>> ChangeStatusEmployee(Guid personID, string statusTypeName, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmployeeForce", "{personID}/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusEmployeeForceWithEmployer", "{personID}/{employerID}/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusEmployeeForceTyped", "{personID}/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmployeeForceWithEmployerTyped", "{personID}/{employerID}/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmployeeForceTypedDefault", "{personID}/Status/{statusTypeName}/Force/TypedView/")]
        [ServiceRoute("ChangeStatusEmployeeForceWithEmployerTypedDefault", "{personID}/{employerID}/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<EmployeeResult>> ChangeStatusEmployeeForce(Guid personID, string statusTypeName, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmployeeRelationship", "{personID}/EmployeeRelationship/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusEmployeeRelationshipWithEmployer", "{personID}/{employerID}/EmployeeRelationship/Status/{statusTypeName}/")]
        [ServiceRoute("ChangeStatusEmployeeRelationshipTyped", "{personID}/EmployeeRelationship/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmployeeRelationshipWithEmployerTyped", "{personID}/{employerID}/EmployeeRelationship/Status/{statusTypeName}/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmployeeRelationshipTypedDefault", "{personID}/EmployeeRelationship/Status/{statusTypeName}/TypedView/")]
        [ServiceRoute("ChangeStatusEmployeeRelationshipWithEmployerTypedDefault", "{personID}/{employerID}/EmployeeRelationship/Status/{statusTypeName}/TypedView/")]
        Task<ChangeStatusQueryResult<EmployeeResult>> ChangeStatusEmployeeRelationship(Guid personID, string statusTypeName, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("ChangeStatusEmployeeRelationshipForce", "{personID}/EmployeeRelationship/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusEmployeeRelationshipForceWithEmployer", "{personID}/{employerID}/EmployeeRelationship/Status/{statusTypeName}/Force/")]
        [ServiceRoute("ChangeStatusEmployeeRelationshipForceTyped", "{personID}/EmployeeRelationship/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmployeeRelationshipForceWithEmployerTyped", "{personID}/{employerID}/EmployeeRelationship/Status/{statusTypeName}/Force/TypedView/{viewName}/")]
        [ServiceRoute("ChangeStatusEmployeeRelationshipForceTypedDefault", "{personID}/EmployeeRelationship/Status/{statusTypeName}/Force/TypedView/")]
        [ServiceRoute("ChangeStatusEmployeeRelationshipForceWithEmployerTypedDefault", "{personID}/{employerID}/EmployeeRelationship/Status/{statusTypeName}/Force/TypedView/")]
        Task<ChangeStatusQueryResult<EmployeeResult>> ChangeStatusEmployeeRelationshipForce(Guid personID, string statusTypeName, Guid? employerID = null, string viewName = null, CancellationToken token = default(CancellationToken));
    }
}
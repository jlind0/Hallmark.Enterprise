using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HallData.EMS.ApplicationViews.Results;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;
using HallData.Business;

namespace HallData.EMS.Business
{
    public interface IReadOnlyPersonImplementation<TKey, TPersonResult> : IReadOnlyPartyImplementation<TKey, TPersonResult>
        where TPersonResult : IPersonResult<TKey>
    {
        [GetMethod]
        [ServiceRoute("GetEmployers", "{personID}/Employers/")]
        [ServiceRoute("GetEmployersTyped", "{personID}/Employers/TypedView/{viewName}/")]
        [ServiceRoute("GetEmployersTypedDefault", "{personID}/Employers/TypedView/")]
        Task<QueryResults<EmployeeResult>> GetEmployers(Guid personID, string viewName = null, FilterContext<EmployeeResult> filter = null, SortContext<EmployeeResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
        [GetMethod]
        [ServiceRoute("GetEmployersView", "{personID}/Employers/View/{viewName}/")]
        [ServiceRoute("GetEmployersViewDefault", "{personID}/Employers/View/")]
        Task<QueryResults<JObject>> GetEmployersView(Guid personID, string viewName = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface IReadOnlyPersonImplementation : IReadOnlyPersonImplementation<Guid, PersonResult> { }
    public interface IPersonImplementation<TKey, TPersonResult, in TPersonForAdd, in TPersonForUpdate> : IPartyImplementation<TKey, TPersonResult, TPersonForAdd, TPersonForUpdate>, 
        IReadOnlyPersonImplementation<TKey, TPersonResult>
        where TPersonResult: IPersonResult<TKey>
        where TPersonForAdd: IPersonForAdd<TKey>
        where TPersonForUpdate: IPersonForUpdate<TKey>
    {
        
    }
    [Service("people")]
    public interface IPersonImplementation : IPartyImplementation<Guid, PersonResult, PersonForAdd, PersonForUpdate>, IReadOnlyPersonImplementation { }
}

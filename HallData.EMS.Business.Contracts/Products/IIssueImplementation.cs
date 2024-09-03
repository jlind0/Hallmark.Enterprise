using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
    public interface IReadOnlyIssueImplementation : IReadOnlyProductImplementation<IssueResult>
    {
    }
    public interface IIssueImplementation : IReadOnlyIssueImplementation, IProductImplementation<IssueResult, IssueForAdd, IssueForUpdate> { }
}

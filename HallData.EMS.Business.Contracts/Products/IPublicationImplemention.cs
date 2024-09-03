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
    public interface IReadOnlyPublicationImplementation : IReadOnlyBrandedProductImplementation<PublicationResult>
    {
    }
    public interface IPublicationImplementation : IReadOnlyPublicationImplementation, IBrandedProductImplementation<PublicationResult, PublicationForAdd, PublicationForUpdate> { }
}

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
    public interface IReadOnlyTrackImplementation : IReadOnlyProductImplementation<TrackResult>
    {
    }
    public interface ITrackImplementation : IReadOnlyTrackImplementation, IProductImplementation<TrackResult, TrackForAdd, TrackForUpdate> { }
}

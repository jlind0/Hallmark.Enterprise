using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews;
using System.Threading;

namespace HallData.EMS.Business
{
    [Service("enums")]
    public interface IEnumerationsImplemention : IBusinessImplementation
    {
        [GetMethod(requireSessionHeader: false)]
        [ServiceRoute("GetStatusTypes", "StatusTypes/")]
        [Description("Gets status types")]
        Task<IEnumerable<StatusTypeResult>> GetStatusTypes(CancellationToken token = default(CancellationToken));
        [GetMethod(requireSessionHeader: false)]
        [ServiceRoute("GetDeliveryMethodTypes", "DeliveryMethodTypes/")]
        [Description("Gets delivery method types")]
        Task<IEnumerable<DeliveryMethodType>> GetDeliveryMethodTypes(CancellationToken token = default(CancellationToken));
        [GetMethod(requireSessionHeader: false)]
        [ServiceRoute("GetTierTypes", "TierTypes/")]
        [Description("Gets tier types")]
        Task<IEnumerable<TierType>> GetTierTypes(CancellationToken token = default(CancellationToken));
        [GetMethod(requireSessionHeader: false)]
        [ServiceRoute("GetFrequencies", "Frequencies/")]
        [Description("Gets frequencies")]
        Task<IEnumerable<Frequency>> GetFrequencies(CancellationToken token = default(CancellationToken));
        [GetMethod(requireSessionHeader: false)]
        [ServiceRoute("GetProductTypes", "ProductTypes/")]
        [Description("Gets product types")]
        Task<IEnumerable<ProductType>> GetProductTypes(CancellationToken token = default(CancellationToken));
    }
}

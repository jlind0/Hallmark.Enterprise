using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;

namespace HallData.EMS.Data
{
    public interface IEnumerationsRepository : IRepository
    {
        Task<IEnumerable<StatusTypeResult>> GetStatusTypes(CancellationToken token = default(CancellationToken));
        Task<IEnumerable<DeliveryMethodType>> GetDeliveryMethodTypes(CancellationToken token = default(CancellationToken));
        Task<IEnumerable<TierType>> GetTierTypes(CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Frequency>> GetFrequencies(CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ProductType>> GetProductTypes(CancellationToken token = default(CancellationToken));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.Data;
using HallData.EMS.ApplicationViews;
using HallData.Security;
using System.Threading;

namespace HallData.EMS.Business
{
    public class EnumerationsImplemention : BusinessRepositoryProxy<IEnumerationsRepository>, IEnumerationsImplemention
    {
        public EnumerationsImplemention(IEnumerationsRepository repository, ISecurityImplementation security) :base(repository, security)
        { }


        public Task<IEnumerable<StatusTypeResult>> GetStatusTypes(CancellationToken token = default(CancellationToken))
        {
            return this.Repository.GetStatusTypes(token);
        }

        public Task<IEnumerable<DeliveryMethodType>> GetDeliveryMethodTypes(CancellationToken token = default(CancellationToken))
        {
            return this.Repository.GetDeliveryMethodTypes(token);
        }

        public Task<IEnumerable<TierType>> GetTierTypes(CancellationToken token = default(CancellationToken))
        {
            return this.Repository.GetTierTypes(token);
        }

        public Task<IEnumerable<Frequency>> GetFrequencies(CancellationToken token = default(CancellationToken))
        {
            return this.Repository.GetFrequencies(token);
        }

        public Task<IEnumerable<ProductType>> GetProductTypes(CancellationToken token = default(CancellationToken))
        {
            return this.Repository.GetProductTypes(token);
        }
    }
}

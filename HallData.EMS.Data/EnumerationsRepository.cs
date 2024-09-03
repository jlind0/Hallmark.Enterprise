using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Data;
using HallData.Repository;
using System.Threading;
using HallData.EMS.ApplicationViews;

namespace HallData.EMS.Data
{
    public class EnumerationsRepository : HallData.Repository.Repository, IEnumerationsRepository 
    {
        public EnumerationsRepository(Database db) : base(db) { }


        public Task<IEnumerable<StatusTypeResult>> GetStatusTypes(CancellationToken token = default(CancellationToken))
        {
            var db = this.Database;
            var cmd = this.Database.CreateStoredProcCommand("usp_select_statustypes");
            return this.ReadResults<StatusTypeResult>(cmd, token: token);
        }

        public Task<IEnumerable<DeliveryMethodType>> GetDeliveryMethodTypes(CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_select_deliverymethodtypes");
            return this.ReadResults<DeliveryMethodType>(cmd, token: token);
        }

        public Task<IEnumerable<TierType>> GetTierTypes(CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_select_tiertypes");
            return this.ReadResults<TierType>(cmd, token: token);
        }

        public Task<IEnumerable<Frequency>> GetFrequencies(CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_select_frequencies");
            return this.ReadResults<Frequency>(cmd, token: token);
        }

        public Task<IEnumerable<ProductType>> GetProductTypes(CancellationToken token = default(CancellationToken))
        {
            var cmd = this.Database.CreateStoredProcCommand("usp_select_producttypes");
            return this.ReadResults<ProductType>(cmd, token: token);
        }
    }
}

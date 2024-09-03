using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews;
using HallData.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace HallData.EMS.Data
{
    public class PartyContactMechanismRepository : ContactMechanismHolderRepository<PartyContactMechanismId, Guid, PartyContactMechanismKey>, IPartyContactMechanismRepository
    {
        public PartyContactMechanismRepository(Database db) : base(db, "usp_select_partycontactmechanisms", "usp_insert_partycontactmechanisms", 
            "usp_update_partycontactmechanisms", "usp_delete_partycontactmechanisms", "usp_changestatus_partycontactmechanisms") { }

        protected override void PopulateKeyParameter(DbCommand cmd, PartyContactMechanismId key)
        {
            cmd.AddParameter("partyguid", key.PartyGuid);
            cmd.AddParameter("contactmechanismguid", key.ContactMechanismGuid);
        }

        protected override void PopulateIdParameter(DbCommand cmd, Guid id)
        {
            cmd.AddParameter("partyguid", id);
        }
    }
}

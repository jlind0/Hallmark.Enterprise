using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using System.Data.SqlClient;
using HallData.Data;
using System.Data.Common;

namespace HallData.EMS.Data
{
    public class ProductContactMechanismRepository : ContactMechanismHolderRepository<ProductContactMechanismId, Guid, ProductContactMechanismKey>, IProductContactMechanismRepository
    {
        public ProductContactMechanismRepository(Database db) : base(db, "usp_select_productcontactmechanisms", "usp_insert_productcontactmechanisms", 
            "usp_update_productcontactmechanisms", "usp_delete_productcontactmechanisms", "usp_changestatus_productcontactmechanisms") { }

        protected override void PopulateKeyParameter(DbCommand cmd, ProductContactMechanismId key)
        {
            cmd.AddParameter("productguid", key.ProductGuid);
            cmd.AddParameter("contactmechanismguid", key.ContactMechanismGuid);
        }

        protected override void PopulateIdParameter(DbCommand cmd, Guid id)
        {
            cmd.AddParameter("productguid", id);
        }
    }
}

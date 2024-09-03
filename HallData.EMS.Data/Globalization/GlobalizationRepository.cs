using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;
using HallData.Data;
using HallData.Globalization;

namespace HallData.EMS.Data.Globalization
{
    public class GlobalizationRepository : HallData.Repository.Repository, IGlobalizationRepository
    {
        public GlobalizationRepository(Database db) : base(db) { }
        public string GetErrorMessage(string errorCode, Cultures culture)
        {
            Database db = this.Database;
            var cmd = db.CreateStoredProcCommand("usp_get_errormessage");
            db.AddParameter(cmd, "errorcode", errorCode);
            db.AddParameter(cmd, "cultureid", (int)culture);
            return db.ExecuteScalar(cmd) as string;
        }
    }
}

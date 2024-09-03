using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.EMS.ApplicationViews.Enums
{
    public enum StatusType
    {
        Enabled = 1,
        Disabled = 2,
        Created = 3,
        Processing = 4,
        Completed = 5,
        Deleted = 6,
        QA = 7,
        Outputting = 8,
        Compressing = 9,
        Copying = 10,
        Cancelled = 11
        //Deleted = 12
    }
}

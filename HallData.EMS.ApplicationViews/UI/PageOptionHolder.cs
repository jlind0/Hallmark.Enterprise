using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;

namespace HallData.EMS.ApplicationViews.UI
{
    public class PageOptionHolder
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public int PageOption { get; set; }
    }
}

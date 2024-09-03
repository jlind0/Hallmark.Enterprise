using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Business
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class Authorize : Attribute
    {
        public string Roles { get; set; }
        public Authorize(string roles = null)
        {
            this.Roles = roles;
        }
    }
}

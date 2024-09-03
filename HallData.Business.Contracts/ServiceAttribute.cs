using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Business
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple=false)]
    public class ServiceAttribute : Attribute
    {
        public string Name { get; set; }
        public ServiceAttribute(string name)
        {
            this.Name = name;
        }
    }
}

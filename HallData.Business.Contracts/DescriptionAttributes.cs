using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Business
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; private set; }
        public DescriptionAttribute(string description)
        {
            this.Description = description;
        }
    }
}

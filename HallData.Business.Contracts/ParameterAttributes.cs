using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Business
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class Content : Attribute { }
    [AttributeUsage(AttributeTargets.Parameter)]
    public class JsonEncode : Attribute { }
}

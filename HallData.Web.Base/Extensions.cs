using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using System.Net.Http;

namespace HallData.Web
{
    public static class Extensions
    {
        public static HttpMethod ToHttpMethod(this ServiceMethod method)
        {
            switch (method.MethodType)
            {
                case ServiceMethodTypes.Add: return HttpMethod.Post;
                case ServiceMethodTypes.Delete: return HttpMethod.Delete;
                case ServiceMethodTypes.Get: return HttpMethod.Get;
                case ServiceMethodTypes.Update: return HttpMethod.Put;
            }
            throw new NotImplementedException();
        }
    }
}

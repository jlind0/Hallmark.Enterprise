using System.Web;
using System.Web.Mvc;

namespace Halldata.EMS.CustomAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

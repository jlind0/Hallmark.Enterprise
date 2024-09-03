using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews.UI;
using HallData.Security;
using System.Threading;

namespace HallData.EMS.Business.UI
{
    [Service("ui/personalization")]
    public interface IPersonalizationImplementation : IBusinessImplementation
    {
        [GetMethod(requireSessionHeader: false)]
        [ServiceRoute("Get", "Get/{viewName}/")]
        [Description("Gets a view defination")]
        Task<ApplicationViewResult> Get([Description("The target view name")]string viewName, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("Personalize", "Personalize/")]
        [Description("Personalizes view for signed user")]
        Task<ApplicationViewResult> Personalize([Description("The new view settings")][Content]ApplicationViewForParty view, CancellationToken token = default(CancellationToken));
        [UpdateMethod]
        [ServiceRoute("Restore", "Restore/{viewName}")]
        [Description("Restores a view to default settings for signed in user")]
        Task<ApplicationViewResult> RestoreDefaultSettings([Description("The target view name")]string viewName, CancellationToken token = default(CancellationToken));
    }
}

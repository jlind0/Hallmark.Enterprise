using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.ApplicationViews;
using HallData.Business;
using System.Threading;

namespace HallData.Admin.Business
{
    
    public interface IReadOnlyTemplateTypeImplementation : IReadOnlyBusinessImplementation<TemplateTypeResult>
    {
    }
    [Service("templatetypes")]
    public interface ITemplateTypeImplementation : IReadOnlyTemplateTypeImplementation
    {
        [GetMethod]
        [ServiceRoute("GetTemplates", "{templateTypeId}/templates/")]
        [ServiceRoute("GetTemplatesTypedView", "{templateTypeId}/templates/TypedView/{viewName}")]
        [ServiceRoute("GetTemplatesTypedViewDefault", "{templateTypeId}/templates/TypedView/")]
        Task<QueryResults<TemplateResult>> GetTemplates(int templateTypeId, string viewName = null, [JsonEncode]FilterContext<TemplateResult> filter = null, [JsonEncode]SortContext<TemplateResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
}

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

    public interface IReadOnlyTemplateImplementation : IReadOnlyBusinessImplementation<TemplateResult>
    {
        Task<QueryResults<TemplateResult>> GetByTemplateType(int templateTypeId, string viewName = null, [JsonEncode]FilterContext<TemplateResult> filter = null, [JsonEncode]SortContext<TemplateResult> sort = null, [JsonEncode]PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    [Service("templates")]
    public interface ITemplateImplementation : IDeletableBusinessImplementationWithBase<int, TemplateResult, TemplateForAdd, TemplateForUpdate>, IReadOnlyTemplateImplementation
    {
        
    }
}

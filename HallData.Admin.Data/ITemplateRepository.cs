using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.Repository;
using System.Threading;
using HallData.ApplicationViews;

namespace HallData.Admin.Data
{
    public interface IReadOnlyTemplateRepository : IReadOnlyRepository<TemplateResult>
    {
        Task<QueryResults<TemplateResult>> GetByTemplateType(int templateTypeId, string viewName = null, Guid? userId = null, FilterContext<TemplateResult> filter = null, SortContext<TemplateResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken));
    }
    public interface ITemplateRepository : IDeletableRepository<int, TemplateResult, TemplateForAdd, TemplateForUpdate>, IReadOnlyTemplateRepository
    {

    }
}

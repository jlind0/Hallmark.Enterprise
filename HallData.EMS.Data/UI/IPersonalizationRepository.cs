using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.UI;
using HallData.Repository;

namespace HallData.EMS.Data.UI
{
    public interface IPersonalizationRepository : IRepository
    {
        Task<ApplicationViewResult> Get(string viewName, Guid? userID = null, CancellationToken token = default(CancellationToken));
        Task Personalize(ApplicationViewForParty view, Guid userID, CancellationToken token = default(CancellationToken));
        Task RestoreDefaultSettings(string viewName, Guid userID, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Template>> GetTemplates(int templateTypeId, int? parentTemplateId = null, int? dataViewColumnId = null, int? filterTypeId = null, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<FilterTypeWithColumns>> GetFilterTypes(int? dataViewColumnId = null, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ApplicationViewColumnWithData>> GetColumns(int applicationViewId, int? filterTypeId = null, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<ApplicationViewName>> GetApplicationViews(string dataViewName, Guid? userID = null, CancellationToken token = default(CancellationToken));
        Task<bool> DoesApplicationViewNameExist(string viewName, Guid? userID = null, CancellationToken token = default(CancellationToken));
    }
}
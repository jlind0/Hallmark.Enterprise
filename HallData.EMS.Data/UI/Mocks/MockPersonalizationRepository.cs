using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository.Mocks;
using HallData.Repository;
using System.Threading;
using HallData.EMS.ApplicationViews.UI;
using HallData.EMS.ApplicationViews;

namespace HallData.EMS.Data.UI.Mocks
{
    public class MockPersonalizationRepository : IPersonalizationRepository
    {
        protected Dictionary<string, ApplicationViewResult> Data { get; private set; }
        public MockPersonalizationRepository(IEnumerable<ApplicationViewResult> views)
        {
            this.Data = views.ToDictionary(v => v.Name);
        }
        public Task<ApplicationViewResult> Get(string viewName, Guid? userID = null, CancellationToken token = default(CancellationToken))
        {
            ApplicationViewResult result;
            if (Data.TryGetValue(viewName, out result))
                return Task.FromResult(result);
            return Task.FromResult(null as ApplicationViewResult);
        }

        public Task Personalize(ApplicationViewForParty view, Guid userID, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task RestoreDefaultSettings(string viewName, Guid userID, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Template>> GetTemplates(int templateTypeId, int? parentTemplateId = null, int? dataViewColumnId = null, int? filterTypeId = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FilterTypeWithColumns>> GetFilterTypes(int? dataViewColumnId = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationViewColumnWithData>> GetColumns(int applicationViewId, int? filterTypeId = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationViewName>> GetApplicationViews(string dataViewName, Guid? userID = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesApplicationViewNameExist(string viewName, Guid? userID = null, CancellationToken token = default(CancellationToken))
        {
            return Task.FromResult(this.Data.ContainsKey(viewName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.UI;
using HallData.Data;
using System.Threading;
using HallData.ApplicationViews;

namespace HallData.EMS.Data.UI
{
    public class PersonalizationRepository : Repository.Repository, IPersonalizationRepository
    {
        public PersonalizationRepository(Database db) : base(db) { }
        public Task<ApplicationViewResult> Get(string viewName, Guid? userID = null, CancellationToken token = default(CancellationToken))
        {
            var db = this.Database;
            var cmd = db.CreateStoredProcCommand("ui.usp_select_view_definition");
            cmd.AddParameter("viewname", viewName);
            return ReadResult<ApplicationViewResult>(cmd, userID, token);
        }

        public async Task Personalize(ApplicationViewForParty view, Guid userID, CancellationToken token = default(CancellationToken))
        {
            var db = this.Database;
            var cmd = db.CreateStoredProcCommand("ui.usp_personalizeview");
            PopulateUserIdParameter(cmd, userID);
            cmd.MapParameters(view, this.Database, ViewOperations.Update);
            await Execute(cmd, () => db.ExecuteNonQueryAsync(cmd, token));
            foreach(var column in view.Columns)
            {
                var c = db.CreateStoredProcCommand("ui.usp_personalizeview_column");
                PopulateUserIdParameter(c, userID);
                c.MapParameters(column, this.Database, ViewOperations.Update);
                await Execute(c, () => db.ExecuteNonQueryAsync(c, token));
            }
        }

        public Task RestoreDefaultSettings(string viewName, Guid userID, CancellationToken token = default(CancellationToken))
        {
            var db = this.Database;
            var cmd = db.CreateStoredProcCommand("ui.usp_applicationview_restorepersonalizationsettings");
            cmd.AddParameter("viewname", viewName);
            PopulateUserIdParameter(cmd, userID);
            return Execute(cmd, () => db.ExecuteNonQueryAsync(cmd, token));
        }


        public Task<IEnumerable<Template>> GetTemplates(int templateTypeId, int? parentTemplateId = null, int? dataViewColumnId = null, int? filterTypeId = null, CancellationToken token = default(CancellationToken))
        {
            var db = this.Database;
            var cmd = db.CreateStoredProcCommand("ui.usp_select_templates_bytemplatetype");
            cmd.AddParameter("templatetypeid", templateTypeId);
            if (parentTemplateId != null)
                cmd.AddParameter("parenttemplateid", parentTemplateId);
            if (dataViewColumnId != null)
                cmd.AddParameter("dataviewcolumnid", dataViewColumnId);
            if (filterTypeId != null)
                cmd.AddParameter("filtertypeid", filterTypeId);
            return this.ReadResults<Template>(cmd, token: token);
        }

        public Task<IEnumerable<FilterTypeWithColumns>> GetFilterTypes(int? dataViewColumnId = null, CancellationToken token = default(CancellationToken))
        {
            var db = this.Database;
            var cmd = db.CreateStoredProcCommand("ui.usp_select_filtertypes");
            if (dataViewColumnId != null)
                cmd.AddParameter("dataviewcolumnid", dataViewColumnId);
            return this.ReadResults<FilterTypeWithColumns>(cmd, token: token);
        }

        public Task<IEnumerable<ApplicationViewColumnWithData>> GetColumns(int applicationViewId, int? filterTypeId = null, CancellationToken token = default(CancellationToken))
        {
            var db = this.Database;
            var cmd = db.CreateStoredProcCommand("ui.usp_select_applicationviewcolumns");
            cmd.AddParameter("applicationviewid", applicationViewId);
            if (filterTypeId != null)
                cmd.AddParameter("filtertypeid", filterTypeId);
            return this.ReadResults<ApplicationViewColumnWithData>(cmd, token: token);
        }


        public Task<IEnumerable<ApplicationViewName>> GetApplicationViews(string dataViewName, Guid? userID = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesApplicationViewNameExist(string viewName, Guid? userID = null, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}

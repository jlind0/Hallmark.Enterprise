using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using HallData.Admin.ApplicationViews;
using HallData.ApplicationViews;
using HallData.Data;
using HallData.Repository;
using Newtonsoft.Json.Linq;

namespace HallData.Admin.Data
{
	public class TemplateRepository : DeletableRepository<int, TemplateResult, TemplateForAdd, TemplateForUpdate>,
		ITemplateRepository
	{
		public const string SelectAllTemplateProcedure = "ui.usp_select_templates";
		public const string SelectTemplateQuery = "select * from ui.v_templates where [templateid#] = @templateid and [__userguid?] = @__userguid";
        public const string InsertTemplateProcedure = "ui.usp_insert_templates";
        public const string UpdateTemplateProcedure = "ui.usp_update_templates";
        public const string DeleteTemplateProcedure = "ui.usp_delete_templates";

		public TemplateRepository(Database db)
            : base(db, SelectAllTemplateProcedure, SelectTemplateQuery, InsertTemplateProcedure, UpdateTemplateProcedure, DeleteTemplateProcedure,null)
		{

		}

		protected override void PopulateGetCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("templateid", id);
		}

		protected override int ReadKeyFromScalarReturnObject(object obj, TemplateForAdd view)
		{
			return (int)obj;
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, int id)
		{
			throw new NotImplementedException();
		}

		protected override void PopulateDeleteCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("templateid", id);
		}

		public Task<QueryResults<TemplateResult>> GetByTemplateType(int templateTypeId, string viewName = null, Guid? userId = null, FilterContext<TemplateResult> filter = null,
			SortContext<TemplateResult> sort = null, PageDescriptor page = null, CancellationToken token = new CancellationToken())
		{
			var cmd = this.Database.CreateStoredProcCommand(this.GetAllStoredProcName);
			cmd.AddParameter("templateid", templateTypeId);
			return this.ReadQueryResults<TemplateResult>(cmd, viewName, userId, filter, sort, page, token: token);
		}
	}
}

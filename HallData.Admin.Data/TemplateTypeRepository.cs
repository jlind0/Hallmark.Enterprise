using System;
using System.ComponentModel.Design;
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
	public class TemplateTypeRepository : ReadOnlyRepository<TemplateTypeResult>,IReadOnlyTemplateTypeRepository
	{

		public const string SelectAllTemplateTypeProcedure = "ui.usp_select_templatetypes";
		public const string SelectTemplateTypeQuery = "select * from ui.v_templatetypes where [templatetypeid#] = @templatetypeid and [__userguid?] = @__userguid";

		public TemplateTypeRepository(Database db) : base(db, SelectAllTemplateTypeProcedure, SelectTemplateTypeQuery) { }

		protected override void PopulateGetCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("templatetypeid", id);
		}

	}
}

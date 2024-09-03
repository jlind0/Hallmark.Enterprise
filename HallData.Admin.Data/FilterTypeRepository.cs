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
	public class FilterTypeRepository : DeletableRepository<int, FilterTypeResult, FilterTypeForAdd, FilterTypeForUpdate>,
		IFilterTypeRepository
	{
		public const string SelectAllFilterTypeProcedure = "ui.usp_select_filtertypes";
		public const string SelectFilterTypeQuery = "select * from ui.v_filtertypes where [filtertypeid#] = @filtertypeid and [__userguid?] = @__userguid";
        public const string InsertFilterTypeProcedure = "ui.usp_insert_filtertypes";
        public const string UpdateFilterTypeProcedure = "ui.usp_update_filtertypes";
        public const string DeleteFilterTypeProcedure = "ui.usp_delete_filtertypes";

		public FilterTypeRepository(Database db)
            : base(db, SelectAllFilterTypeProcedure, SelectFilterTypeQuery, InsertFilterTypeProcedure, UpdateFilterTypeProcedure, DeleteFilterTypeProcedure,null)
		{

		}

		protected override void PopulateGetCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("filtertypeid", id);
		}

		protected override int ReadKeyFromScalarReturnObject(object obj, FilterTypeForAdd view)
		{
			return (int)obj;
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, int id)
		{
			throw new NotImplementedException();
		}

		protected override void PopulateDeleteCommand(int id, DbCommand cmd)
		{
			cmd.AddParameter("filtertypeid", id);
		}
	}
}

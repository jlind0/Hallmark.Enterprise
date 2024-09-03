using System.Data.SqlClient;
using HallData.Data;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;
using System.Data.Common;

namespace HallData.EMS.Data
{
	public class CategoryRepository : DeletableRepository<int, CategoryResult, CategoryForAdd, CategoryForUpdate>, ICategoryRepository
	{
		protected const string SelectAllCommand = "usp_select_categories";
        protected const string SelectCommand = "select * from v_categories where [categoryid#] = @id and [__userguid?] = @__userguid";
		protected const string InsertCommand = "usp_insert_categories";
		protected const string UpdateCommand = "usp_update_categories";
		protected const string DeleteCommand = "usp_delete_categories";
		protected const string ChangeStatusCommand = "usp_changestatus_categories";

		public CategoryRepository(Database db)
			: base(db, SelectAllCommand, SelectCommand, InsertCommand, UpdateCommand, DeleteCommand, ChangeStatusCommand) { }

		public static void PopulateCategoryIdParameter(int id, DbCommand cmd)
		{
			cmd.AddParameter("categoryid", id);
		}

		protected override void PopulateDeleteCommand(int id, DbCommand cmd)
		{
			CategoryRepository.PopulateCategoryIdParameter(id, cmd);
		}

		protected override int ReadKeyFromScalarReturnObject(object obj, CategoryForAdd view)
		{
			return (int)obj;
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, int id)
		{
			CategoryRepository.PopulateCategoryIdParameter(id, cmd);
		}

		protected override void PopulateGetCommand(int id, DbCommand cmd)
		{
			CategoryRepository.PopulateCategoryIdParameter(id, cmd);
		}
	}
}

using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Data;
using HallData.EMS.ApplicationViews.Enums;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;
using Newtonsoft.Json.Linq;
using System.Data.Common;

namespace HallData.EMS.Data
{
	public class PartyCategoryRepository : DeletableRepository<PartyCategoryId, PartyCategoryResult, PartyCategoryForAdd, PartyCategoryForUpdate>, IPartyCategoryRepository
	{
		protected const string SelectAllCommand = "usp_select_categorypartyroles";
		protected const string SelectCommand = "select * from v_categorypartyroles where [id#] = @id and [partyguid#] = @partyguid and [roleid#] = @roleid and [__userguid?] = @__userguid";
		protected const string InsertCommand = "usp_insert_categorypartyroles";
		protected const string UpdateCommand = "usp_update_categorypartyroles";
		protected const string DeleteCommand = "usp_delete_categorypartyroles";
		protected const string ChangeStatusCommand = "usp_changestatus_categorypartyroles";

		public PartyCategoryRepository(Database db)
			: base(db, SelectAllCommand, SelectCommand, InsertCommand, UpdateCommand, DeleteCommand, ChangeStatusCommand) { }

		public static void PopulatePartyCategoryIdParameter(PartyCategoryId id, DbCommand cmd)
		{
			cmd.AddParameter("id", id.Id);
			cmd.AddParameter("partyguid", id.PartyGuid);
			cmd.AddParameter("roleid", id.RoleId);
		}

		protected override void PopulateDeleteCommand(PartyCategoryId id, DbCommand cmd)
		{
			PopulatePartyCategoryIdParameter(id, cmd);
		}

		protected override PartyCategoryId ReadKeyFromScalarReturnObject(object obj, PartyCategoryForAdd view)
		{
			return view.Key;
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, PartyCategoryId id)
		{
			PopulatePartyCategoryIdParameter(id, cmd);
		}

		protected override void PopulateGetCommand(PartyCategoryId id, DbCommand cmd)
		{
			PopulatePartyCategoryIdParameter(id, cmd);
		}

		public Task<QueryResults<PartyCategoryResult>> GetByParty(Guid partyId, int? categoryTypeId = null, int? roleId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			cmd.AddParameter("partyguid", partyId);

			if (categoryTypeId != null)
			{
				cmd.AddParameter("categoryTypeId", categoryTypeId);
			}

			if (roleId != null)
			{
				cmd.AddParameter("roleId", roleId);
			}

			return ReadQueryResults<PartyCategoryResult>(cmd, viewName, userId, token: token);
		}

		public Task<QueryResults<JObject>> GetByPartyView(Guid partyId, int? categoryTypeId = null, int? roleId = null, string viewName = null, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			cmd.AddParameter("partyguid", partyId);

			if (categoryTypeId != null)
			{
				cmd.AddParameter("categoryTypeId", categoryTypeId);
			}

			if (roleId != null)
			{
				cmd.AddParameter("roleId", roleId);
			}

			return ReadViews(cmd, viewName, userId, token: token);
		}
	}
}

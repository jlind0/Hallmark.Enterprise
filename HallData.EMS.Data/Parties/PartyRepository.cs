using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;
using HallData.EMS.ApplicationViews;
using System.Threading;
using HallData.Data;
using System.Data.SqlClient;
using HallData.EMS.ApplicationViews.Enums;
using System.Data;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;
using System.Data.Common;

namespace HallData.EMS.Data
{
	public abstract class PartyRepository<TKey, TPartyResult, TPartyForAdd, TPartyForUpdate> : DeletableRepository<TKey, TPartyResult, TPartyForAdd, TPartyForUpdate>,
		IPartyRepository<TKey, TPartyResult, TPartyForAdd, TPartyForUpdate>
		where TPartyResult: IPartyResult<TKey>
		where TPartyForAdd : IPartyForAdd<TKey>
		where TPartyForUpdate: IPartyForUpdate<TKey>
	{
		protected const string SelectAllPartiesProcedure = "usp_select_parties";
		protected const string SelectPartyQuery = "select * from v_parties where partyguid = @partyguid and [__userguid?] = @__userguid";
		protected const string InsertPartyProcedure = "usp_insert_parties";
		protected const string UpdatePartyProcedure = "usp_update_parties";
		protected const string DeletePartyProcedure = "usp_delete_parties";
		protected const string ChangeStatusPartyProcedure = "usp_changestatus_parties";

		public PartyRepository(Database db)
			: base(db, SelectAllPartiesProcedure, SelectPartyQuery, InsertPartyProcedure, UpdatePartyProcedure, DeletePartyProcedure, ChangeStatusPartyProcedure) { }

		protected PartyRepository(Database db, string selectAllProcedure = SelectAllPartiesProcedure, string selectProcedure = SelectPartyQuery, string insertProcedure = InsertPartyProcedure,
			string updateProcedure = UpdatePartyProcedure, string deleteProcedure = DeletePartyProcedure, string changeStatusProcedure = ChangeStatusPartyProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		public virtual Task AddCategory(Guid partyID, int categoryID, Guid userID, CancellationToken token = default(CancellationToken))
		{
			Database db = this.Database;
			var cmd = db.CreateStoredProcCommand("usp_insert_parties_categories");
			PopulateUserIdParameter(cmd, userID);
			cmd.AddParameter("partyguid", partyID);
			cmd.AddParameter("categoryid", categoryID);
			return Execute(cmd, () => db.ExecuteNonQueryAsync(cmd, token));
		}

		public virtual Task RemoveCategory(Guid partyID, int categoryID, Guid userID, CancellationToken token = default(CancellationToken))
		{
			Database db = this.Database;
			var cmd = db.CreateStoredProcCommand("usp_delete_parties_categories");
			PopulateUserIdParameter(cmd, userID);
			cmd.AddParameter("partyguid", partyID);
			cmd.AddParameter("categoryid", categoryID);
			return Execute(cmd, () => db.ExecuteNonQueryAsync(cmd, token));
		}

	}

	public class PartyRepository<TPartyResult, TPartyForAdd, TPartyForUpdate> : PartyRepository<Guid, TPartyResult, TPartyForAdd, TPartyForUpdate>, IPartyRepository<TPartyResult, TPartyForAdd, TPartyForUpdate>
		where TPartyResult: IPartyResult
		where TPartyForAdd: IPartyForAdd
		where TPartyForUpdate: IPartyForUpdate
	{
		public PartyRepository(Database db) : base(db, SelectAllPartiesProcedure, SelectPartyQuery, InsertPartyProcedure, UpdatePartyProcedure, DeletePartyProcedure, ChangeStatusPartyProcedure) { }

		protected PartyRepository(Database db, string selectAllProcedure = SelectAllPartiesProcedure, string selectProcedure = SelectPartyQuery, string insertProcedure = InsertPartyProcedure,
			string updateProcedure = UpdatePartyProcedure, string deleteProcedure = DeletePartyProcedure, string changeStatusProcedure = ChangeStatusPartyProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		protected override void PopulateDeleteCommand(Guid id, DbCommand cmd)
		{
			PartyRepository.PopulatePartyGuidParameter(id, cmd);
		}

		protected override Guid ReadKeyFromScalarReturnObject(object obj, TPartyForAdd view)
		{
			return (Guid)obj;
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, Guid id)
		{
			PartyRepository.PopulatePartyGuidParameter(id, cmd);
		}

		protected override void PopulateGetCommand(Guid id, DbCommand cmd)
		{
			PartyRepository.PopulatePartyGuidParameter(id, cmd);
		}
	}

	public class PartyRepository : PartyRepository<PartyResult, PartyForAdd, PartyForUpdate>, IPartyRepository
	{
		public PartyRepository(Database db) : base(db) { }
		public static void PopulatePartyTypeParameter(PartyType type, DbCommand cmd)
		{
			cmd.AddParameter("partytypeid", (int)type);
		}
		public static void PopulatePartyGuidParameter(Guid id, DbCommand cmd)
		{
			cmd.AddParameter("partyguid", id);
		}
	}
}

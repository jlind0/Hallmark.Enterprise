using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.ApplicationViews.Enums;
using HallData.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace HallData.EMS.Data
{
	public abstract class OrganizationRepository<TKey, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate> :
		PartyRepository<TKey, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>, IOrganizationRepository<TKey, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>
		where TOrganizationResult : IOrganizationResult<TKey>
		where TOrganizationForAdd : IOrganizationForAdd<TKey>
		where TOrganizationForUpdate: IOrganizationForUpdate<TKey>
	{
		public const string SelectOrganizationQuery = "select * from v_parties_organizations where partyguid = @partyguid and [__userguid?] = @__userguid";

		public OrganizationRepository(Database db) : base(db) { }

		protected OrganizationRepository(Database db, string selectAllProcedure = SelectAllPartiesProcedure, string selectProcedure = SelectOrganizationQuery, string insertProcedure = InsertPartyProcedure,
			string updateProcedure = UpdatePartyProcedure, string deleteProcedure = DeletePartyProcedure, string changeStatusProcedure = ChangeStatusPartyProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		protected override void PopulateGetAllStoredProcedure(DbCommand cmd)
		{
			base.PopulateGetAllStoredProcedure(cmd);
			PartyRepository.PopulatePartyTypeParameter(PartyType.Organization, cmd);
		}

		protected override void PopulateGetCommand(TKey id, DbCommand cmd)
		{
			PartyRepository.PopulatePartyTypeParameter(PartyType.Organization, cmd);
		}
	}

	public class OrganizationRepository<TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate> :
		OrganizationRepository<Guid, TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>, IOrganizationRepository<TOrganizationResult, TOrganizationForAdd, TOrganizationForUpdate>
		where TOrganizationResult: IOrganizationResult
		where TOrganizationForAdd : IOrganizationForAdd
		where TOrganizationForUpdate: IOrganizationForUpdate
	{
		public OrganizationRepository(Database db) : base(db) { }
		protected OrganizationRepository(Database db, string selectAllProcedure = SelectAllPartiesProcedure, string selectProcedure = SelectPartyQuery, string insertProcedure = InsertPartyProcedure,
			string updateProcedure = UpdatePartyProcedure, string deleteProcedure = DeletePartyProcedure, string changeStatusProcedure = ChangeStatusPartyProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }
		protected override void PopulateDeleteCommand(Guid id, DbCommand cmd)
		{
			PartyRepository.PopulatePartyGuidParameter(id, cmd);
		}

		protected override Guid ReadKeyFromScalarReturnObject(object obj, TOrganizationForAdd view)
		{
			return (Guid)obj;
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, Guid id)
		{
			PartyRepository.PopulatePartyGuidParameter(id, cmd);
		}

		protected override void PopulateGetCommand(Guid id, DbCommand cmd)
		{
			base.PopulateGetCommand(id, cmd);
			PartyRepository.PopulatePartyGuidParameter(id, cmd);
		}
	}
	public class OrganizationRepository : OrganizationRepository<OrganizationResult, OrganizationForAdd, OrganizationForUpdate>, IOrganizationRepository
	{
		public OrganizationRepository(Database db) : base(db) { }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Data;
using HallData.EMS.ApplicationViews.Results;
using System.Data.SqlClient;
using HallData.EMS.ApplicationViews.Enums;
using System.Data.Common;

namespace HallData.EMS.Data
{
	public abstract class PersonRepository<TKey, TPersonResult, TPersonForAdd, TPersonForUpdate> : PartyRepository<TKey, TPersonResult, TPersonForAdd, TPersonForUpdate>,
		IPersonRepository<TKey, TPersonResult, TPersonForAdd, TPersonForUpdate>
		where TPersonResult: IPersonResult<TKey>
		where TPersonForAdd: IPersonForAdd<TKey>
		where TPersonForUpdate: IPersonForUpdate<TKey>
	{
		public const string SelectPersonQuery = "select * from v_parties_persons where partyguid = @partyguid and [__userguid?] = @__userguid";

		public PersonRepository(Database db) : base(db) { }

		protected PersonRepository(Database db, string selectAllProcedure = SelectAllPartiesProcedure, string selectProcedure = SelectPersonQuery, string insertProcedure = InsertPartyProcedure,
			string updateProcedure = UpdatePartyProcedure, string deleteProcedure = DeletePartyProcedure, string changeStatusProcedure = ChangeStatusPartyProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		protected override void PopulateGetAllStoredProcedure(DbCommand cmd)
		{
			base.PopulateGetAllStoredProcedure(cmd);
			PartyRepository.PopulatePartyTypeParameter(PartyType.Person, cmd);
		}
		protected override void PopulateGetCommand(TKey id, DbCommand cmd)
		{
			PartyRepository.PopulatePartyTypeParameter(PartyType.Person, cmd);
		}
	}

	public class PersonRepository<TPersonResult, TPersonForAdd, TPersonForUpdate> : PersonRepository<Guid, TPersonResult, TPersonForAdd, TPersonForUpdate>, IPersonRepository<TPersonResult, TPersonForAdd, TPersonForUpdate>
		where TPersonResult : IPersonResult
		where TPersonForAdd : IPersonForAdd
		where TPersonForUpdate: IPersonForUpdate
	{
		public PersonRepository(Database db) : base(db) { }
		protected PersonRepository(Database db, string selectAllProcedure = SelectAllPartiesProcedure, string selectProcedure = SelectPartyQuery, string insertProcedure = InsertPartyProcedure,
			string updateProcedure = UpdatePartyProcedure, string deleteProcedure = DeletePartyProcedure, string changeStatusProcedure = ChangeStatusPartyProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }
		protected override void PopulateDeleteCommand(Guid id, DbCommand cmd)
		{
			PartyRepository.PopulatePartyGuidParameter(id, cmd);
		}

		protected override Guid ReadKeyFromScalarReturnObject(object obj, TPersonForAdd view)
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
	public class PersonRepository : PersonRepository<PersonResult, PersonForAdd, PersonForUpdate>, IPersonRepository
	{
		public PersonRepository(Database db) : base(db) { }
	}
}

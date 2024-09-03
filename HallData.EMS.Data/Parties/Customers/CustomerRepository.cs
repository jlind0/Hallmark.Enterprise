using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using System.Data.SqlClient;
using HallData.Data;
using System.Threading;
using HallData.Repository;
using HallData.ApplicationViews;
using System.Data.Common;

namespace HallData.EMS.Data
{
	public class CustomerRepository : PartyRepository<CustomerId, CustomerResult, CustomerForAdd, CustomerForUpdate>, ICustomerRepository
	{
		public const string SelectAllCustomersProcedure = "usp_select_customers";
        public const string SelectCustomerQuery = "select * from v_customers where [partyguid#] = @partyguid and ((@customerofpartyguid is null and [customerofpartyguid#] is null) or [customerofpartyguid#] = @customerofpartyguid) and [__userguid?] = @__userguid";
		public const string InsertCustomerProcedure = "usp_insert_customers";
		public const string UpdateCustomerProcedure = "usp_update_customers";
		public const string DeleteCustomerProcedure = "usp_delete_customers";

		public CustomerRepository(Database db) : base(db, SelectAllCustomersProcedure, SelectCustomerQuery, InsertCustomerProcedure, UpdateCustomerProcedure, DeleteCustomerProcedure) { }

		protected CustomerRepository(Database db, string selectAllProcedure = SelectAllCustomersProcedure, string selectProcedure = SelectCustomerQuery, string insertProcedure = InsertCustomerProcedure,
			string updateProcedure = UpdateCustomerProcedure, string deleteProcedure = DeleteCustomerProcedure, string changeStatusProcedure = ChangeStatusPartyProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

		public Task<ChangeStatusResult> ChangeStatusCustomerOfRelationship(CustomerId id, string statusTypeName, bool force = false, Guid? userId = null, CancellationToken token = default(CancellationToken))
		{
			var cmd = this.Database.CreateStoredProcCommand("usp_changestatus_customers_relationship");
			PopulateChangeStatusCommand(cmd, id);
			return ExecuteChangeStatus(cmd, statusTypeName, force, userId, token);
		}

		public static void PopulateCustomerKeyParameters(CustomerId id, DbCommand cmd)
		{
			cmd.AddParameter("partyguid", id.PartyGuid);
			cmd.AddParameter("customerofpartyguid", id.CustomerOfPartyGuid);
		}

		public static CustomerId ReadCustomerKeyFromScalarObject<TCustomerBase>(object obj, TCustomerBase view)
			where TCustomerBase: ICustomerForAdd
		{
			return new CustomerId((Guid)obj, view.CustomerOfPartyGuid);
		}

		protected override void PopulateDeleteCommand(CustomerId id, DbCommand cmd)
		{
			PopulateCustomerKeyParameters(id, cmd);
		}

		protected override CustomerId ReadKeyFromScalarReturnObject(object obj, CustomerForAdd view)
		{
			return ReadCustomerKeyFromScalarObject(obj, view);
		}

		protected override void PopulateChangeStatusCommand(DbCommand cmd, CustomerId id)
		{
			PopulateCustomerKeyParameters(id, cmd);
		}

		protected override void PopulateGetCommand(CustomerId id, DbCommand cmd)
		{
			PopulateCustomerKeyParameters(id, cmd);
		}

		public Task<QueryResults<CustomerResult>> GetCustomersOfCustomer(Guid partyId, string viewName = null, Guid? userId = null, FilterContext<CustomerResult> filter = null, SortContext<CustomerResult> sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			CustomerId custmerId = new CustomerId(partyId, null);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerKeyParameters(custmerId, cmd);
			return ReadQueryResults(cmd, viewName, userId, filter, sort, page, token: token);
		}

		public Task<QueryResults<Newtonsoft.Json.Linq.JObject>> GetCustomersOfCustomerView(Guid partyId, string viewName = null, Guid? userId = null, FilterContext filter = null, SortContext sort = null, PageDescriptor page = null, CancellationToken token = default(CancellationToken))
		{
			var db = this.Database;
			var cmd = db.CreateStoredProcCommand(this.GetAllStoredProcName);
			CustomerId custmerId = new CustomerId(partyId, null);
			PopulateGetAllStoredProcedure(cmd);
			PopulateCustomerKeyParameters(custmerId, cmd);
			return ReadViews(cmd, viewName, userId, filter, sort, page, token: token);
		}
	}
}

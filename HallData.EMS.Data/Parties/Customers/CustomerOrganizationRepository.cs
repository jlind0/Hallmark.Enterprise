using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace HallData.EMS.Data
{
    public class CustomerOrganizationRepository : OrganizationRepository<CustomerId, CustomerOrganizationResult, CustomerOrganizationForAdd, CustomerOrganizationForUpdate>, ICustomerOrganizationRepository
    {
        public const string SelectCustomerOrganizationQuery = "select * from v_customers_organizations where [partyguid#] = @partyguid and ((@customerofpartyguid is null and [customerofpartyguid#] is null) or [customerofpartyguid#] = @customerofpartyguid) and [__userguid?] = @__userguid";

        public CustomerOrganizationRepository(Database db)
			: base(db, CustomerRepository.SelectAllCustomersProcedure, SelectCustomerOrganizationQuery, CustomerRepository.InsertCustomerProcedure,
            CustomerRepository.UpdateCustomerProcedure, CustomerRepository.DeleteCustomerProcedure) { }

		protected CustomerOrganizationRepository(Database db, string selectAllProcedure = CustomerRepository.SelectAllCustomersProcedure, string selectProcedure = SelectCustomerOrganizationQuery,
			string insertProcedure = CustomerRepository.InsertCustomerProcedure,
			string updateProcedure = CustomerRepository.UpdateCustomerProcedure,
			string deleteProcedure = CustomerRepository.DeleteCustomerProcedure, 
			string changeStatusProcedure = ChangeStatusPartyProcedure)
			: base(db, selectAllProcedure, selectProcedure, insertProcedure, updateProcedure, deleteProcedure, changeStatusProcedure) { }

        protected override void PopulateDeleteCommand(CustomerId id, DbCommand cmd)
        {
            CustomerRepository.PopulateCustomerKeyParameters(id, cmd);
        }

        protected override CustomerId ReadKeyFromScalarReturnObject(object obj, CustomerOrganizationForAdd view)
        {
            return CustomerRepository.ReadCustomerKeyFromScalarObject(obj, view);
        }

        protected override void PopulateChangeStatusCommand(DbCommand cmd, CustomerId id)
        {
            CustomerRepository.PopulateCustomerKeyParameters(id, cmd);
        }

        protected override void PopulateGetCommand(CustomerId id, DbCommand cmd)
        {
            base.PopulateGetCommand(id, cmd);
            CustomerRepository.PopulateCustomerKeyParameters(id, cmd);
        }
    }
}

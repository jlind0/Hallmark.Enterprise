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
    public class CustomerPersonRepository : PersonRepository<CustomerId, CustomerPersonResult, CustomerPersonForAdd, CustomerPersonForUpdate>, ICustomerPersonRepository
    {
        public const string SelectCustomerPeopleQuery = "select * from v_customers_people where [partyguid#] = @partyguid and ((@customerofpartyguid is null and [customerofpartyguid#] is null) or [customerofpartyguid#] = @customerofpartyguid) and [__userguid?] = @__userguid";

        public CustomerPersonRepository(Database db)
			: base(db, CustomerRepository.SelectAllCustomersProcedure, SelectCustomerPeopleQuery, CustomerRepository.InsertCustomerProcedure, 
            CustomerRepository.UpdateCustomerProcedure, CustomerRepository.DeleteCustomerProcedure) { }
        protected override void PopulateDeleteCommand(CustomerId id, DbCommand cmd)
        {
            CustomerRepository.PopulateCustomerKeyParameters(id, cmd);
        }

        protected override CustomerId ReadKeyFromScalarReturnObject(object obj, CustomerPersonForAdd view)
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

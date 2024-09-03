using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Data;

namespace HallData.EMS.Data.Tests
{
	public class CustomerOrganizationMockRepositorySuccess : CustomerOrganizationRepository
	{
		private const string selectCustomerOrganizationQuery = @"select * from tst.v_customers_organizations
																 where [partyguid#] = @partyguid and ((@customerofpartyguid 
																 is null and [customerofpartyguid#] is null) 
																 or [customerofpartyguid#] = @customerofpartyguid) 
																 and [__userguid?] = @__userguid";

		public CustomerOrganizationMockRepositorySuccess(Database db)
			: base(db, "tst.usp_select_customers", selectCustomerOrganizationQuery, "tst.usp_insert_customers",
			"tst.usp_update_customers", "tst.usp_delete_customers", "tst.usp_changestatus_customers")
		{

		}
	}

	public class CustomerOrganizationMockRepositoryFail : CustomerOrganizationRepository
	{
		private const string selectCustomerOrganizationQuery = @"select * from tst.v_customers_organizations
																 where [partyguid#] = @partyguid and ((@customerofpartyguid 
																 is null and [customerofpartyguid#] is null) 
																 or [customerofpartyguid#] = @customerofpartyguid) 
																 and [__userguid?] = @__userguid and 1 = 0";

		public CustomerOrganizationMockRepositoryFail(Database db)
			: base(db, "tst.usp_select_customers_fail", selectCustomerOrganizationQuery, "tst.usp_insert_customers_fail",
			"tst.usp_update_customers_fail", "tst.usp_delete_customers_fail", "tst.usp_changestatus_customers_fail")
		{

		}
	}

	// used only for testing status
	public class CustomerOrganizationMockRepositoryWarning : CustomerOrganizationRepository
	{
		private const string selectCustomerOrganizationQuery = @"select * from tst.v_customers_organizations
																 where [partyguid#] = @partyguid and ((@customerofpartyguid 
																 is null and [customerofpartyguid#] is null) 
																 or [customerofpartyguid#] = @customerofpartyguid) 
																 and [__userguid?] = @__userguid and 1 = 0";

		public CustomerOrganizationMockRepositoryWarning(Database db)
			: base(db, "tst.usp_select_customers_fail", selectCustomerOrganizationQuery, "tst.usp_insert_customers_fail",
			"tst.usp_update_customers_fail", "tst.usp_delete_customers_fail", "tst.usp_changestatus_customers_warning")
		{

		}
	}
}

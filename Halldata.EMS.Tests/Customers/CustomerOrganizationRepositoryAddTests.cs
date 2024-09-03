using System;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HallData.Data.Mocks;
using System.Data.SqlClient;
using HallData.Exceptions;

namespace HallData.EMS.Tests.Customers
{
	[TestClass]
	public class CustomerOrganizationRepositoryAddTests
	{
		[TestMethod]
		public async Task CustomerOrganization_Add_Success()
		{
            bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			Guid addedGuid = Guid.NewGuid();
			string cust_name = "Name";
			string cust_code = "Code";
			int cust_partyid = 2;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), scalarFactory: c =>
            {
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var name = c.Parameters["Name"];
				Assert.IsNotNull(name);
				Assert.AreEqual(cust_name, name.Value);

				var code = c.Parameters["Code"];
				Assert.IsNotNull(code);
				Assert.AreEqual(cust_code, code.Value);

				var partyTypeId = c.Parameters["PartyType_PartyTypeId"];
				Assert.IsNotNull(partyTypeId);
				Assert.AreEqual(cust_partyid, partyTypeId.Value);

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				Assert.IsNull(errorCode.Value);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				Assert.IsNull(errorType.Value);
				
	            executed = true;
	            return addedGuid;
            });

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);
			CustomerOrganizationForAdd customerAdd = new CustomerOrganizationForAdd
			{
				Name = cust_name,
				PartyType = new PartyTypeKey { PartyTypeId = cust_partyid },
				Code = cust_code
			};

			await repo.Add(customerAdd, userGuid);
            Assert.IsTrue(executed);
			Assert.AreEqual(addedGuid, customerAdd.PartyGuid);
		}

        [TestMethod]
        public async Task CustomerOrganization_Add_AuthorizationFailure()
        {
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			Guid addedGuid = Guid.NewGuid();
			string cust_name = "Name";
			string cust_code = "Code";
			int cust_partyid = 2;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), scalarFactory: c =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var name = c.Parameters["Name"];
				Assert.IsNotNull(name);
				Assert.AreEqual(cust_name, name.Value);

				var code = c.Parameters["Code"];
				Assert.IsNotNull(code);
				Assert.AreEqual(cust_code, code.Value);

				var partyTypeId = c.Parameters["PartyType_PartyTypeId"];
				Assert.IsNotNull(partyTypeId);
				Assert.AreEqual(cust_partyid, partyTypeId.Value);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Authorization;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				executed = true;
				return addedGuid;
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);
			CustomerOrganizationForAdd customerAdd = new CustomerOrganizationForAdd
			{
				Name = cust_name,
				PartyType = new PartyTypeKey { PartyTypeId = cust_partyid },
				Code = cust_code
			};
			
			try
            {
				await repo.Add(customerAdd, userGuid);
				Assert.Fail("Authorization exception not thrown");
            }
			catch (GlobalizedAuthorizationException ex)
            {
                Assert.AreEqual("INVALID", ex.ErrorCode);
            }
        }

		[TestMethod]
		public async Task CustomerOrganization_Add_AuthenticationFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			Guid addedGuid = Guid.NewGuid();
			string cust_name = "Name";
			string cust_code = "Code";
			int cust_partyid = 2;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), scalarFactory: c =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var name = c.Parameters["Name"];
				Assert.IsNotNull(name);
				Assert.AreEqual(cust_name, name.Value);

				var code = c.Parameters["Code"];
				Assert.IsNotNull(code);
				Assert.AreEqual(cust_code, code.Value);

				var partyTypeId = c.Parameters["PartyType_PartyTypeId"];
				Assert.IsNotNull(partyTypeId);
				Assert.AreEqual(cust_partyid, partyTypeId.Value);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Authentication;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				executed = true;
				return addedGuid;
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);
			CustomerOrganizationForAdd customerAdd = new CustomerOrganizationForAdd
			{
				Name = cust_name,
				PartyType = new PartyTypeKey { PartyTypeId = cust_partyid },
				Code = cust_code
			};

			try
			{
				await repo.Add(customerAdd, userGuid);
				Assert.Fail("Authentication exception not thrown");
			}
			catch (GlobalizedAuthenticationException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}

		[TestMethod]
		public async Task CustomerOrganization_Add_ValidationFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			Guid addedGuid = Guid.NewGuid();
			string cust_name = "Name";
			string cust_code = "Code";
			int cust_partyid = 2;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), scalarFactory: c =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var name = c.Parameters["Name"];
				Assert.IsNotNull(name);
				Assert.AreEqual(cust_name, name.Value);

				var code = c.Parameters["Code"];
				Assert.IsNotNull(code);
				Assert.AreEqual(cust_code, code.Value);

				var partyTypeId = c.Parameters["PartyType_PartyTypeId"];
				Assert.IsNotNull(partyTypeId);
				Assert.AreEqual(cust_partyid, partyTypeId.Value);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Validation;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				executed = true;
				return addedGuid;
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);
			CustomerOrganizationForAdd customerAdd = new CustomerOrganizationForAdd
			{
				Name = cust_name,
				PartyType = new PartyTypeKey { PartyTypeId = cust_partyid },
				Code = cust_code
			};

			try
			{
				await repo.Add(customerAdd, userGuid);
				Assert.Fail("Validation exception not thrown");
			}
			catch (GlobalizedValidationException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}

		[TestMethod]
		public async Task CustomerOrganization_Add_OtherFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			Guid addedGuid = Guid.NewGuid();
			string cust_name = "Name";
			string cust_code = "Code";
			int cust_partyid = 2;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), scalarFactory: c =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var name = c.Parameters["Name"];
				Assert.IsNotNull(name);
				Assert.AreEqual(cust_name, name.Value);

				var code = c.Parameters["Code"];
				Assert.IsNotNull(code);
				Assert.AreEqual(cust_code, code.Value);

				var partyTypeId = c.Parameters["PartyType_PartyTypeId"];
				Assert.IsNotNull(partyTypeId);
				Assert.AreEqual(cust_partyid, partyTypeId.Value);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Other;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				executed = true;
				return addedGuid;
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);
			CustomerOrganizationForAdd customerAdd = new CustomerOrganizationForAdd
			{
				Name = cust_name,
				PartyType = new PartyTypeKey { PartyTypeId = cust_partyid },
				Code = cust_code
			};

			try
			{
				await repo.Add(customerAdd, userGuid);
				Assert.Fail("GlobalizedException exception not thrown");
			}
			catch (GlobalizedAuthorizationException)
			{
				Assert.Fail("Wrong exception thrown. Expect GlobalizedException");
			}
			catch (GlobalizedException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}
	}
}

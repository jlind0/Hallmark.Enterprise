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
	public class CustomerOrganizationRepositoryDeleteTests
	{
		[TestMethod]
		public async Task CustomerOrganization_Delete_Success()
		{
            bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId(){PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid()};
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
            {
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				Assert.IsNull(errorCode.Value);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				Assert.IsNull(errorType.Value);
				
	            executed = true;
				return 1;   // # rows affected
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);

			await repo.Delete(customerId, userGuid);
            Assert.IsTrue(executed);
		}
		
		[TestMethod]
		public async Task CustomerOrganization_Delete_AuthorizationFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Authorization;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				executed = true;
				return 1;   // # rows affected
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);

			try
			{
				await repo.Delete(customerId, userGuid);
				Assert.Fail("Authorization exception not thrown");
			}
			catch (GlobalizedAuthorizationException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}

		[TestMethod]
		public async Task CustomerOrganization_Delete_AuthenticationFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Authentication;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				executed = true;
				return 1;   // # rows affected
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);

			try
			{
				await repo.Delete(customerId, userGuid);
				Assert.Fail("Authentication exception not thrown");
			}
			catch (GlobalizedAuthenticationException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}

		[TestMethod]
		public async Task CustomerOrganization_Delete_ValidationFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Validation;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				executed = true;
				return 1;   // # rows affected
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);

			try
			{
				await repo.Delete(customerId, userGuid);
				Assert.Fail("Validation exception not thrown");
			}
			catch (GlobalizedValidationException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}

		[TestMethod]
		public async Task CustomerOrganization_Delete_OtherFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Other;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				executed = true;
				return 1;   // # rows affected
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);

			try
			{
				await repo.Delete(customerId, userGuid);
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

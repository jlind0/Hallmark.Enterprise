using System;
using System.Threading.Tasks;
using HallData.ApplicationViews;
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
	public class CustomerOrganizationRepositoryChangeStatusTests
	{
		[TestMethod]
		public async Task CustomerOrganization_ChangeStatus_Success()
		{
            bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };

			string statustypenamechangeto = "Disabled";
			bool forcechangeto = true;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
            {
				var partyguidParm = c.Parameters["partyguid"];
				Assert.IsNotNull(partyguidParm);
				Assert.AreEqual(customerId.PartyGuid, partyguidParm.Value);

				var customerofpartyguidParm = c.Parameters["customerofpartyguid"];
				Assert.IsNotNull(customerofpartyguidParm);
				Assert.AreEqual(customerId.CustomerOfPartyGuid, customerofpartyguidParm.Value);

				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var statustypename = c.Parameters["statustypename"];
				Assert.IsNotNull(statustypename);
				Assert.AreEqual(statustypenamechangeto, statustypename.Value);

				var force = c.Parameters["force"];
				Assert.IsNotNull(force);
				Assert.AreEqual(forcechangeto, force.Value);

				var ischanged = c.Parameters["ischanged"];
				Assert.IsNotNull(ischanged);
				//Assert.AreEqual(true, ischanged.Value);  // coming back as null
				ischanged.Value = true; // set to true


				var warningmessage = c.Parameters["warningmessage"];
				Assert.IsNotNull(warningmessage);
				Assert.IsNull(warningmessage.Value);

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

			ChangeStatusResult result = await repo.ChangeStatus(customerId, statustypenamechangeto, forcechangeto, userGuid);

            Assert.IsTrue(executed);
			Assert.AreEqual(true, result.StatusChanged);
			Assert.IsNull(result.WarningMessage);
		}

		[TestMethod]
		public async Task CustomerOrganization_ChangeStatus_ValidationFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };

			string statustypenamechangeto = "Disabled";
			bool forcechangeto = true;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
			{
				var partyguidParm = c.Parameters["partyguid"];
				Assert.IsNotNull(partyguidParm);
				Assert.AreEqual(customerId.PartyGuid, partyguidParm.Value);

				var customerofpartyguidParm = c.Parameters["customerofpartyguid"];
				Assert.IsNotNull(customerofpartyguidParm);
				Assert.AreEqual(customerId.CustomerOfPartyGuid, customerofpartyguidParm.Value);

				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var statustypename = c.Parameters["statustypename"];
				Assert.IsNotNull(statustypename);
				Assert.AreEqual(statustypenamechangeto, statustypename.Value);

				var force = c.Parameters["force"];
				Assert.IsNotNull(force);
				Assert.AreEqual(forcechangeto, force.Value);

				var ischanged = c.Parameters["ischanged"];
				Assert.IsNotNull(ischanged);
				ischanged.Value = false; // set to false

				var warningmessage = c.Parameters["warningmessage"];
				Assert.IsNotNull(warningmessage);
				Assert.IsNull(warningmessage.Value);

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
				ChangeStatusResult result = await repo.ChangeStatus(customerId, statustypenamechangeto, forcechangeto, userGuid);
				Assert.Fail("Validation exception not thrown");
			}
			catch (GlobalizedValidationException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}

		[TestMethod]
		public async Task CustomerOrganization_ChangeStatus_OtherFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };

			string statustypenamechangeto = "Disabled";
			bool forcechangeto = true;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
			{
				var partyguidParm = c.Parameters["partyguid"];
				Assert.IsNotNull(partyguidParm);
				Assert.AreEqual(customerId.PartyGuid, partyguidParm.Value);

				var customerofpartyguidParm = c.Parameters["customerofpartyguid"];
				Assert.IsNotNull(customerofpartyguidParm);
				Assert.AreEqual(customerId.CustomerOfPartyGuid, customerofpartyguidParm.Value);

				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var statustypename = c.Parameters["statustypename"];
				Assert.IsNotNull(statustypename);
				Assert.AreEqual(statustypenamechangeto, statustypename.Value);

				var force = c.Parameters["force"];
				Assert.IsNotNull(force);
				Assert.AreEqual(forcechangeto, force.Value);

				var ischanged = c.Parameters["ischanged"];
				Assert.IsNotNull(ischanged);
				ischanged.Value = false; // set to false

				var warningmessage = c.Parameters["warningmessage"];
				Assert.IsNotNull(warningmessage);
				Assert.IsNull(warningmessage.Value);

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
				ChangeStatusResult result = await repo.ChangeStatus(customerId, statustypenamechangeto, forcechangeto, userGuid);
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

		[TestMethod]
		public async Task CustomerOrganization_ChangeStatus_AuthenticationFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };

			string statustypenamechangeto = "Disabled";
			bool forcechangeto = true;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
			{
				var partyguidParm = c.Parameters["partyguid"];
				Assert.IsNotNull(partyguidParm);
				Assert.AreEqual(customerId.PartyGuid, partyguidParm.Value);

				var customerofpartyguidParm = c.Parameters["customerofpartyguid"];
				Assert.IsNotNull(customerofpartyguidParm);
				Assert.AreEqual(customerId.CustomerOfPartyGuid, customerofpartyguidParm.Value);

				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var statustypename = c.Parameters["statustypename"];
				Assert.IsNotNull(statustypename);
				Assert.AreEqual(statustypenamechangeto, statustypename.Value);

				var force = c.Parameters["force"];
				Assert.IsNotNull(force);
				Assert.AreEqual(forcechangeto, force.Value);

				var ischanged = c.Parameters["ischanged"];
				Assert.IsNotNull(ischanged);
				ischanged.Value = false; // set to false

				var warningmessage = c.Parameters["warningmessage"];
				Assert.IsNotNull(warningmessage);
				Assert.IsNull(warningmessage.Value);

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
				ChangeStatusResult result = await repo.ChangeStatus(customerId, statustypenamechangeto, forcechangeto, userGuid);
				Assert.Fail("Authentication exception not thrown");
			}
			catch (GlobalizedAuthenticationException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}

		[TestMethod]
		public async Task CustomerOrganization_ChangeStatus_AuthorizationFailure()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };

			string statustypenamechangeto = "Disabled";
			bool forcechangeto = true;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
			{
				var partyguidParm = c.Parameters["partyguid"];
				Assert.IsNotNull(partyguidParm);
				Assert.AreEqual(customerId.PartyGuid, partyguidParm.Value);

				var customerofpartyguidParm = c.Parameters["customerofpartyguid"];
				Assert.IsNotNull(customerofpartyguidParm);
				Assert.AreEqual(customerId.CustomerOfPartyGuid, customerofpartyguidParm.Value);

				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var statustypename = c.Parameters["statustypename"];
				Assert.IsNotNull(statustypename);
				Assert.AreEqual(statustypenamechangeto, statustypename.Value);

				var force = c.Parameters["force"];
				Assert.IsNotNull(force);
				Assert.AreEqual(forcechangeto, force.Value);

				var ischanged = c.Parameters["ischanged"];
				Assert.IsNotNull(ischanged);
				ischanged.Value = false; // set to false

				var warningmessage = c.Parameters["warningmessage"];
				Assert.IsNotNull(warningmessage);
				Assert.IsNull(warningmessage.Value);

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
				ChangeStatusResult result = await repo.ChangeStatus(customerId, statustypenamechangeto, forcechangeto, userGuid);
				Assert.Fail("Authorization exception not thrown");
			}
			catch (GlobalizedAuthorizationException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}

		[TestMethod]
		public async Task CustomerOrganization_ChangeStatus_ValidationWarning()
		{
			bool executed = false;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };

			string statustypenamechangeto = "Disabled";
			bool forcechangeto = true;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), nonQueryFactory: c =>
			{
				var partyguidParm = c.Parameters["partyguid"];
				Assert.IsNotNull(partyguidParm);
				Assert.AreEqual(customerId.PartyGuid, partyguidParm.Value);

				var customerofpartyguidParm = c.Parameters["customerofpartyguid"];
				Assert.IsNotNull(customerofpartyguidParm);
				Assert.AreEqual(customerId.CustomerOfPartyGuid, customerofpartyguidParm.Value);

				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var statustypename = c.Parameters["statustypename"];
				Assert.IsNotNull(statustypename);
				Assert.AreEqual(statustypenamechangeto, statustypename.Value);

				var force = c.Parameters["force"];
				Assert.IsNotNull(force);
				Assert.AreEqual(forcechangeto, force.Value);

				var ischanged = c.Parameters["ischanged"];
				Assert.IsNotNull(ischanged);
				ischanged.Value = false; // set to false

				var warningmessage = c.Parameters["warningmessage"];
				Assert.IsNotNull(warningmessage);
				warningmessage.Value = "Not Authorized";

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

			ChangeStatusResult result = await repo.ChangeStatus(customerId, statustypenamechangeto, forcechangeto, userGuid);

			Assert.IsTrue(executed);
			Assert.AreEqual(false, result.StatusChanged);
			Assert.AreEqual("Not Authorized", result.WarningMessage);

		}
	}
}

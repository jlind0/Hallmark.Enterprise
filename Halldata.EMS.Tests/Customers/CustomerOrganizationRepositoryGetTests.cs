using System;
using System.Linq;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HallData.Data.Mocks;
using Moq;
using System.Data.SqlClient;
using System.Threading;
using System.Data.Common;
using HallData.EMS.Business;
using HallData.Exceptions;


namespace HallData.EMS.Tests.Customers
{
	[TestClass]
	public class CustomerOrganizationRepositoryGetTests
	{
		[TestMethod]
		public async Task CustomerOrganization_GetMany_Success()
		{
			bool readOnce = true;
			Mock<DbDataReader> drMock = new Mock<DbDataReader>();

			drMock.Setup(c => c.ReadAsync(It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(readOnce))
				.Callback(() => readOnce = false);

			drMock.SetupGet(c => c.FieldCount).Returns(3);
			drMock.Setup(c => c.GetName(0)).Returns("Name");
			drMock.Setup(c => c["Name"]).Returns("Testing 123");
			drMock.Setup(c => c.GetName(1)).Returns("PartyType.PartyTypeId");
			drMock.Setup(c => c["PartyType.PartyTypeId"]).Returns(2);
			drMock.Setup(c => c.GetName(2)).Returns("PartyGuid#");
			drMock.Setup(c => c["PartyGuid#"]).Returns(Guid.NewGuid());
			drMock.Setup(c => c.NextResultAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(false));
            bool executed = false;
            MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), (c, cb) => 
            {
                executed = true;
                Assert.IsNotNull(c.Parameters["__userguid"]);
                var count = c.Parameters["count"];
                Assert.IsNotNull(count);
                count.Value = 1;
                Assert.IsNotNull(c.Parameters["errorType"]);
                Assert.IsNotNull(c.Parameters["errorCode"]);
                Assert.IsNotNull(c.Parameters["queryDescriptor"]);
                Assert.IsNotNull(c.Parameters["viewname"]);
                return drMock.Object; 
            });

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);

			QueryResults<CustomerOrganizationResult> results = await repo.Get("CustomerResultClientServices", new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886"),
				FilterContext<CustomerOrganizationResult>.CreateContext().Like(c => c.Name, "%test%"),
				SortContext<CustomerOrganizationResult>.CreateOrderBy(c => c.Name), new PageDescriptor()
				{
					CurrentPage = 1,
					PageSize = 10
				});
            Assert.IsTrue(executed);
			Assert.IsNotNull(results);
			Assert.AreNotEqual(0, results.Results.Count());
			//Assert.AreNotEqual(10, results.TotalResultsCount);
			//Assert.IsTrue(results.TotalResultsCount > 0);
			Assert.IsTrue(results.Results.All(c => c.Name.ToLower().Contains("test")));
		}

        [TestMethod]
        public async Task CustomerOrganization_GetMany_ValidationFailure()
        {
            Mock<DbDataReader> drMock = new Mock<DbDataReader>();

            drMock.Setup(c => c.ReadAsync(It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(false));
            MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), (c, cb) =>
            {
                Assert.IsNotNull(c.Parameters["__userguid"]);
                var count = c.Parameters["count"];
                Assert.IsNotNull(count);
                //count.Value = 1;
                var errorType = c.Parameters["errorType"];
                Assert.IsNotNull(errorType);
                errorType.Value = (short)ErrorType.Validation;
                var errorCode = c.Parameters["errorCode"];
                Assert.IsNotNull(errorCode);
                errorCode.Value = "INVALID";
                Assert.IsNotNull(c.Parameters["queryDescriptor"]);
                Assert.IsNotNull(c.Parameters["viewname"]);
                return drMock.Object;
            });

            MockDatabase db = new MockDatabase(str => cmd);
            CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);
            try
            {
                QueryResults<CustomerOrganizationResult> results = await repo.Get("CustomerResultClientServices", new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886"),
                    FilterContext<CustomerOrganizationResult>.CreateContext().Like(c => c.Name, "%test%"),
                    SortContext<CustomerOrganizationResult>.CreateOrderBy(c => c.Name), new PageDescriptor()
                    {
                        CurrentPage = 1,
                        PageSize = 10
                    });
                Assert.Fail("Validation exception not thrown");
            }
            catch(GlobalizedValidationException ex)
            {
                Assert.AreEqual("INVALID", ex.ErrorCode);
            }
            catch(Exception)
            {
                throw;
            }
            
        }

		[TestMethod]
		public async Task CustomerOrganization_Get_Success()
		{
			bool readOnce = true;
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			CustomerId customerId = new CustomerId() { PartyGuid = new Guid(), CustomerOfPartyGuid = new Guid() };
			int partyTypeId = 2;

			Mock<DbDataReader> drMock = new Mock<DbDataReader>();

			drMock.Setup(c => c.ReadAsync(It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(readOnce))
				.Callback(() => readOnce = false);

			drMock.SetupGet(c => c.FieldCount).Returns(3);
			drMock.Setup(c => c.GetName(0)).Returns("Name");
			drMock.Setup(c => c["Name"]).Returns("Testing 123");
			drMock.Setup(c => c.GetName(1)).Returns("PartyType.PartyTypeId");
			drMock.Setup(c => c["PartyType.PartyTypeId"]).Returns(2);
			drMock.Setup(c => c.GetName(2)).Returns("PartyGuid#");
			drMock.Setup(c => c["PartyGuid#"]).Returns(Guid.NewGuid());
			drMock.Setup(c => c.NextResultAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(false));
			bool executed = false;
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), (c, cb) =>
			{
				executed = true;
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var partyguid = c.Parameters["partyguid"];
				Assert.IsNotNull(partyguid);
				Assert.AreEqual(customerId.PartyGuid, partyguid.Value);

				var customerofpartyguid = c.Parameters["customerofpartyguid"];
				Assert.IsNotNull(customerofpartyguid);
				Assert.AreEqual(customerId.CustomerOfPartyGuid, customerofpartyguid.Value);

				var partytypeid = c.Parameters["partytypeid"];
				Assert.IsNotNull(partytypeid);
				Assert.AreEqual(partyTypeId, partytypeid.Value);

				return drMock.Object;
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);

			QueryResult<CustomerOrganizationResult> result = await repo.Get(customerId, userGuid);

			Assert.IsTrue(executed);
			Assert.IsNotNull(result);
		
		}

		[TestMethod]
		public async Task CustomerOrganization_GetMany_OtherFailure()
		{
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			Mock<DbDataReader> drMock = new Mock<DbDataReader>();

			drMock.Setup(c => c.ReadAsync(It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(false));
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), (c, cb) =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);
				
				var count = c.Parameters["count"];
				Assert.IsNotNull(count);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Other;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				Assert.IsNotNull(c.Parameters["queryDescriptor"]);
				Assert.IsNotNull(c.Parameters["viewname"]);
				return drMock.Object;
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);
			try
			{
				QueryResults<CustomerOrganizationResult> results = await repo.Get("CustomerResultClientServices", new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886"),
					FilterContext<CustomerOrganizationResult>.CreateContext().Like(c => c.Name, "%test%"),
					SortContext<CustomerOrganizationResult>.CreateOrderBy(c => c.Name), new PageDescriptor()
					{
						CurrentPage = 1,
						PageSize = 10
					});
				Assert.Fail("Other exception not thrown");
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
		public async Task CustomerOrganization_GetMany_AuthenticationFailure()
		{
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			Mock<DbDataReader> drMock = new Mock<DbDataReader>();

			drMock.Setup(c => c.ReadAsync(It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(false));
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), (c, cb) =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var count = c.Parameters["count"];
				Assert.IsNotNull(count);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Authentication;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				Assert.IsNotNull(c.Parameters["queryDescriptor"]);
				Assert.IsNotNull(c.Parameters["viewname"]);
				return drMock.Object;
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);
			try
			{
				QueryResults<CustomerOrganizationResult> results = await repo.Get("CustomerResultClientServices", new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886"),
					FilterContext<CustomerOrganizationResult>.CreateContext().Like(c => c.Name, "%test%"),
					SortContext<CustomerOrganizationResult>.CreateOrderBy(c => c.Name), new PageDescriptor()
					{
						CurrentPage = 1,
						PageSize = 10
					});
				Assert.Fail("Authentication exception not thrown");
			}
			catch (GlobalizedAuthenticationException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}

		[TestMethod]
		public async Task CustomerOrganization_GetMany_AuthorizationFailure()
		{
			Guid userGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886");
			Mock<DbDataReader> drMock = new Mock<DbDataReader>();

			drMock.Setup(c => c.ReadAsync(It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(false));
			MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), (c, cb) =>
			{
				var userguid = c.Parameters["__userguid"];
				Assert.IsNotNull(userguid);
				Assert.AreEqual(userGuid, userguid.Value);

				var count = c.Parameters["count"];
				Assert.IsNotNull(count);

				var errorType = c.Parameters["errorType"];
				Assert.IsNotNull(errorType);
				errorType.Value = (short)ErrorType.Authorization;

				var errorCode = c.Parameters["errorCode"];
				Assert.IsNotNull(errorCode);
				errorCode.Value = "INVALID";

				Assert.IsNotNull(c.Parameters["queryDescriptor"]);
				Assert.IsNotNull(c.Parameters["viewname"]);
				return drMock.Object;
			});

			MockDatabase db = new MockDatabase(str => cmd);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);
			try
			{
				QueryResults<CustomerOrganizationResult> results = await repo.Get("CustomerResultClientServices", new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886"),
					FilterContext<CustomerOrganizationResult>.CreateContext().Like(c => c.Name, "%test%"),
					SortContext<CustomerOrganizationResult>.CreateOrderBy(c => c.Name), new PageDescriptor()
					{
						CurrentPage = 1,
						PageSize = 10
					});
				Assert.Fail("Authorization exception not thrown");
			}
			catch (GlobalizedAuthorizationException ex)
			{
				Assert.AreEqual("INVALID", ex.ErrorCode);
			}
		}

	}
}

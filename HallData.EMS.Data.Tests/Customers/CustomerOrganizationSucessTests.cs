using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Data;
using HallData.EMS.ApplicationViews.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HallData.Data.Mocks;
using Moq;
using System.Data.SqlClient;
using System.Threading;
using System.Data.Common;


namespace HallData.EMS.Data.Tests.Customers
{
	[TestClass]
	public class CustomerOrganizationSucessTests
	{
		[TestMethod]
		public async Task CustomerOrganization_GetMany_Success()
		{
            bool readOnce = true;
            Mock<DbDataReader> drMock = new Mock<DbDataReader>();
            drMock.Setup(c => c.ReadAsync(It.IsAny<CancellationToken>())).Returns(
                () => Task.FromResult(readOnce)).Callback(() => readOnce = false);
            drMock.SetupGet(c => c.FieldCount).Returns(3);
            drMock.Setup(c => c.GetName(0)).Returns("Name");
            drMock.Setup(c => c["Name"]).Returns("Testing 123");
            drMock.Setup(c => c.GetName(1)).Returns("PartyType.PartyTypeId");
            drMock.Setup(c => c["PartyType.PartyTypeId"]).Returns(2);
            drMock.Setup(c => c.GetName(2)).Returns("PartyGuid#");
            drMock.Setup(c => c["PartyGuid#"]).Returns(Guid.NewGuid());
            drMock.Setup(c => c.NextResultAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(false));
            MockCommand cmd = new MockCommand(() => new SqlParameter(), cb => drMock.Object);

            MockDatabase db = new MockDatabase(str => cmd);
			//CustomerOrganizationMockRepositorySuccess repo = new CustomerOrganizationMockRepositorySuccess(db);
			CustomerOrganizationRepository repo = new CustomerOrganizationRepository(db);

            QueryResults<CustomerOrganizationResult> results = await repo.Get("CustomerResultClientServices", new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886"),
				FilterContext<CustomerOrganizationResult>.CreateContext().Like(c => c.Name, "%test%"),
				SortContext<CustomerOrganizationResult>.CreateOrderBy(c => c.Name), new PageDescriptor()
				{
					CurrentPage = 1,
					PageSize = 10
				});

			Assert.IsNotNull(results);
			Assert.AreNotEqual(0, results.Results.Count());
            //Assert.AreNotEqual(10, results.TotalResultsCount);
            //Assert.IsTrue(results.TotalResultsCount > 0);
            Assert.IsTrue(results.Results.All(c => c.Name.ToLower().Contains("test")));
		}
	}
}

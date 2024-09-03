using HallData.Admin.ApplicationViews;
using HallData.Admin.Business;
using HallData.Admin.Data;
using HallData.ApplicationViews;
using HallData.Data.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace HallData.Admin.Tests.HallData.Admin.Data
{
    [TestClass]
    public class DataViewResultRepositryTest
    {
        [TestMethod]
        public async Task TaskChangeStatus_Change_ThrowsException()
        {
            bool executed = false;

            MockDbCommand cmd = new MockDbCommand(() => new SqlParameter(), scalarFactory: c =>
            {
                executed = true;
                return 0;
            });

            MockDatabase db = new MockDatabase(str => cmd);
            DataViewResultRepository repo = new DataViewResultRepository(db);

            try
            {
                ChangeStatusResult result = await repo.ChangeStatus(0, "test");
                Assert.Fail("Failed to throw NotImplementedException.");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("System.NotImplementedException", ex.ToString().Substring(0,30));
            }
        }
    }
}

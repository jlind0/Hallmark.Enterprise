using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HallData.Repository;
using Moq;

namespace HallData.Tests.Repository
{
	[TestClass]
	public class TestRepository
	{

		//protected virtual void PopulateUserIdParameter(DbCommand cmd, Guid? userId)
		//{
		//	if(userId != null)
		//		cmd.AddParameter("__userguid", userId.Value);
		//}

		[TestMethod]
		public void PopulateUserIdParameter_IsNotNull()
		{
//            DbCommand cmd = new DbCommand(@"select * 
//											  from dbo.v_customers 
//											  where [__userguid?] = @UserGuid");

//            Mock<MockRepository> mock = new Mock<MockRepository>();
			//mock.Setup(c => c.).Returns(taskObject);

			//HallData.Repository.Repository repository = new HallData.Repository.Repository();
		}
	}
}

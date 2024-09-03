using System;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using HallData.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HallData.Utilities;

namespace HallData.EMS.Data.Tests.Customers
{
	[TestClass]
	public class StringExtensionsTests
	{
		[TestMethod]
		public void CountCharacterInstancesTest_Success()
		{
			string test = "teststring";
			int found = test.CountCharacterInstances('t');
			int notFound = test.CountCharacterInstances('1');

			Assert.AreEqual(3, found, "Char should be equal to 3");
			Assert.AreEqual(0, notFound, "Char should have not been found.");
		}

	}
}

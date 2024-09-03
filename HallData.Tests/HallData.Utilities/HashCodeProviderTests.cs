using System;
using HallData.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HallData.Tests.HallData.Utilities
{
	public class Person
	{
		public string Name { get; set; }
	}

	[TestClass]
	public class HashCodeProviderTests
	{
		[TestMethod]
		public void BuildHashCode()
		{
			Person a = new Person() { Name = "a" };
			Person b = new Person() { Name = "b" };

			int aHash = a.GetHashCode();
			int bHash = b.GetHashCode();
			int hash = 13;
			int hashCode = (((hash * 7) + aHash) * 7) + bHash;
			int result = HashCodeProvider.BuildHashCode(a, b);

			Assert.AreEqual(hashCode, result);
		}
	}
}

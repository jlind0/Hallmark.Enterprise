using System;
using System.Linq.Expressions;
using System.Reflection;
using HallData.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HallData.Tests.HallData.Utilities
{
	public class TestModel
	{
		public string Name { get; set; }
		public string City { get; set; }
	}


	[TestClass]
	public class ExpressionExtensionsTests
	{
		[TestMethod]
		public void GetMemberExpression_LambdaExpression_IsEqual()
		{
			TestModel model = new TestModel();
			Expression<Func<TestModel, string>> expression = m => m.Name;

			MemberExpression me = expression.GetMemberExpression();

			Assert.AreEqual("Name", me.Member.Name);
		}

		[TestMethod]
		public void GetMemberExpression_MemberExpression_IsEqual()
		{
			TestModel model = new TestModel();
			PropertyInfo key = model.GetType().GetProperty("Name");

			Expression expression = Expression.MakeMemberAccess(Expression.Constant(model), key);

			MemberExpression me = expression.GetMemberExpression();

			Assert.AreEqual("Name", me.Member.Name);
		}

		[TestMethod]
		public void GetMemberExpression_UnaryExpression_Type_IsEqual()
		{
			Expression<Func<int, object>> expression = i => DateTime.Now;

			MemberExpression me = expression.GetMemberExpression();

			Assert.AreEqual(typeof(DateTime), me.Type);
		}

		[TestMethod]
		public void GetMemberExpression_UnaryExpression_Name_IsEqual()
		{
			Expression<Func<int, object>> expression = i => DateTime.Now;

			MemberExpression me = expression.GetMemberExpression();

			Assert.AreEqual("Now", me.Member.Name);
		}

		[TestMethod]
		public void GetPropertyPath_IsEqual()
		{
			TestModel model = new TestModel();
			PropertyInfo key = model.GetType().GetProperty("Name");

			Expression expression = Expression.MakeMemberAccess(Expression.Constant(model), key);

			string path = expression.GetPropertyPath();
			
			Assert.AreEqual("Name", path);
		}
	}
}

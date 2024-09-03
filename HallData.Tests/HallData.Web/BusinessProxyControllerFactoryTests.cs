using System;
using System.Linq;
using System.Threading;
using System.Web.Routing;
using HallData.Business;
using HallData.Tests.HallData.Web.Mocks;
using HallData.Web.Controllers;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HallData.Tests.HallData.Web
{
	/// <summary>
	/// Summary description for BusinessProxyControllerFactoryTests
	/// </summary>
	[TestClass]
	public class BusinessProxyControllerFactoryTests
	{
		public BusinessProxyControllerFactoryTests()
		{
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void RegisterDefault_success()
		{
			IUnityContainer container = new UnityContainer();
			container.RegisterInstance(typeof(IMockBusinessImplementation), new MockBusinessImplementation());
			RouteCollection routes = new RouteCollection();
			BusinessProxyControllerFactory.RegisterDefault<IMockBusinessImplementation>(container, routes);
			Assert.IsNotNull(routes);
			AssertRoute(routes, "mockbusinessimplementationDelete", ServiceMethodTypes.Delete);
			AssertRoute(routes, "mockbusinessimplementationDeleteDefault", ServiceMethodTypes.Delete);
			AssertRoute(routes, "mockbusinessimplementationDeleteHard", ServiceMethodTypes.Delete);
			AssertRoute(routes, "mockbusinessimplementationAdd", ServiceMethodTypes.Add);
			AssertRoute(routes, "mockbusinessimplementationAddTypedDefault", ServiceMethodTypes.Add);
			AssertRoute(routes, "mockbusinessimplementationAddViewDefault", ServiceMethodTypes.Add);
			AssertRoute(routes, "mockbusinessimplementationUpdate", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationUpdateTypedDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationUpdateViewDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatus", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatusTypedViewDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatusViewDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatusForce", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatusForceTypedViewDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatusForceViewDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationGetMany", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetManyTypedView", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetManyTypedViewDefault", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetManyView", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetManyViewDefault", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGet", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetTypedViewDefault", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetViewDefault", ServiceMethodTypes.Get);
		}

		[TestMethod]
		public void RegisterDefault_Ignore_success()
		{
			IUnityContainer container = new UnityContainer();
			container.RegisterInstance(typeof(IMockBusinessImplementation), new MockBusinessImplementation());
			RouteCollection routes = new RouteCollection();
			BusinessProxyControllerFactory.RegisterDefault<IMockBusinessImplementation>(container, routes, ignoreAction: i => i.DeleteSoft(default(int), default(CancellationToken)));
			Assert.IsNotNull(routes);
			Assert.IsNull(routes["mockbusinessimplementationDelete"]);
			Assert.IsNull(routes["mockbusinessimplementationDeleteDefault"]);
			AssertRoute(routes, "mockbusinessimplementationDeleteHard", ServiceMethodTypes.Delete);
			AssertRoute(routes, "mockbusinessimplementationAdd", ServiceMethodTypes.Add);
			AssertRoute(routes, "mockbusinessimplementationAddTypedDefault", ServiceMethodTypes.Add);
			AssertRoute(routes, "mockbusinessimplementationAddViewDefault", ServiceMethodTypes.Add);
			AssertRoute(routes, "mockbusinessimplementationUpdate", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationUpdateTypedDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationUpdateViewDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatus", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatusTypedViewDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatusViewDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatusForce", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatusForceTypedViewDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationChangeStatusForceViewDefault", ServiceMethodTypes.Update);
			AssertRoute(routes, "mockbusinessimplementationGetMany", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetManyTypedView", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetManyTypedViewDefault", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetManyView", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetManyViewDefault", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGet", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetTypedViewDefault", ServiceMethodTypes.Get);
			AssertRoute(routes, "mockbusinessimplementationGetViewDefault", ServiceMethodTypes.Get);
		}

		protected static void AssertRoute(RouteCollection routes, string routeKey, ServiceMethodTypes methodType)
		{
			var route = routes[routeKey] as Route;
			Assert.IsNotNull(route);
			var constraint = route.Constraints["httpMethod"] as HttpMethodConstraint;
			Assert.IsNotNull(constraint);
			string methodname = null;
			switch (methodType)
			{
				case ServiceMethodTypes.Add:
					methodname = "POST";
					break;
				case ServiceMethodTypes.Delete:
					methodname = "DELETE";
					break;
				case ServiceMethodTypes.Get:
					methodname = "GET";
					break;
				case ServiceMethodTypes.Update:
					methodname = "PUT";
					break;
				default:
					throw new NotImplementedException();
			}
			Assert.IsTrue(constraint.AllowedMethods.Any((m) => m == methodname));
		}
	}
}

using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Globalization;
using HallData.Security;
using HallData.Session;
using HallData.Translation;
using HallData.Validation;
using HallData.Web.Session;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HallData.Tests.HallData.Web
{
	[TestClass]
	public class RepositorySessionTests
	{
		private IUnityContainer container;
		private Mock<ISecurityImplementation> mockSecurity;
		private Mock<ISecurityTokenizer> mockTokenizer;

		[TestInitialize()]
		public void TestInitialize()
		{
			// Mock repo in repository session using moq
			// Test getsession

			// Test everything that is in ISession

			mockTokenizer = new Mock<ISecurityTokenizer>();
			mockTokenizer.Setup(s => s.TokenizeUserNameOrganizationId(It.IsAny<string>(), It.IsAny<Guid>())).Returns(() => "token");
			mockTokenizer.Setup(s => s.VerifyToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>())).Returns(() => true);
			mockTokenizer.Setup(s => s.Hash(It.IsAny<string>())).Returns(() => "token");

			mockSecurity = new Mock<ISecurityImplementation>();
			mockSecurity.Setup(s => s.GetSignedInUserSync())
				.Returns(() => new SecurityUser()
				{
					UserGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886"),
					UserName = "lzychowski",
					FirstName = null,
					LastName = null,
					OrganizationGuid = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7")
				});
			mockSecurity.SetupAllProperties();

			ValidationResultFactory.Initialize((v, e) => v);

			container = new UnityContainer();
			container.RegisterType<ISession, RepositorySession>();
			container.RegisterInstance(typeof (ISecurityTokenizer), mockTokenizer.Object);
			container.RegisterInstance(typeof(ISecurityImplementation), mockSecurity.Object);
		}

		// GetSession

		[TestMethod]
		public async Task GetSession_CurrentSessionId_IsNull()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.GetSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SessionState()));

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository.Object);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);

			mockHttpContext.Items["__sessionid"] = null;
			mockHttpContext.Items["__sessionstate"] = null;

			var service = container.Resolve<RepositorySession>();
			SessionState result = await service.GetSession();

			Assert.IsNull(result);
		}

		[TestMethod]
		public async Task GetSession_CurrentSessionState_IsNotNull()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.GetSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SessionState()));

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository.Object);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);

			mockHttpContext.Items["__sessionid"] = Guid.NewGuid();
			mockHttpContext.Items["__sessionstate"] = new SessionState()
			{
				User = new SecurityUser()
				{
					UserName = "Lzychowski"
				}
			};

			var service = container.Resolve<RepositorySession>();
			SessionState result = await service.GetSession();

			Assert.AreEqual("Lzychowski", result.User.UserName);

		}

		[TestMethod]
		public async Task GetSession()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.GetSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SessionState()));

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);

			mockHttpContext.Items["__sessionid"] = Guid.NewGuid();
			mockHttpContext.Items["__sessionstate"] = null;

			var service = container.Resolve<RepositorySession>();
		}

		// GetSessionSync

		[TestMethod]
		public void GetSessionSync()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.GetSessionSync(It.IsAny<Guid>())).Returns(() => new SessionState());

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);
			var service = container.Resolve<RepositorySession>();
		}

		[TestMethod]
		public async Task GetUpdateSession()
		{
			bool executed = false;

			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.GetUpdateSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SessionState())).Callback(() => executed = true);

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);
			var service = container.Resolve<RepositorySession>();
			SessionState result = await service.GetUpdateSession();

			Assert.IsNotNull(result);
			Assert.IsTrue(executed);
		}

		[TestMethod]
		public void GetUpdateSessionSync()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.GetUpdateSessionSync(It.IsAny<Guid>())).Returns(() => new SessionState());

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);
			var service = container.Resolve<RepositorySession>();
		}

		[TestMethod]
		public async Task Login()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.LoginUserWindowAuthentication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SessionState()));

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);
			var service = container.Resolve<RepositorySession>();
		}

		[TestMethod]
		public void LoginSync()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.LoginUserWindowAuthenticationSync(It.IsAny<string>(), It.IsAny<string>())).Returns(() => new SessionState());

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);
			var service = container.Resolve<RepositorySession>();
		}

		[TestMethod]
		public void LoginSync_with_user()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.GetSessionSync(It.IsAny<Guid>())).Returns(() => new SessionState());

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);
			var service = container.Resolve<RepositorySession>();
		}

		[TestMethod]
		public async Task Login_with_user()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.GetSessionSync(It.IsAny<Guid>())).Returns(() => new SessionState());

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);
			var service = container.Resolve<RepositorySession>();
		}

		[TestMethod]
		public async Task Logout()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.Logout(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(true));

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);
			var service = container.Resolve<RepositorySession>();
		}

		[TestMethod]
		public void LogoutSync()
		{
			Mock<ISessionRepository> mockSessionRepository = new Mock<ISessionRepository>();
			MockHttpContextWrapper mockHttpContext = new MockHttpContextWrapper();

			mockSessionRepository.Setup(s => s.LogoutSync(It.IsAny<Guid>())).Returns(() => true);

			container.RegisterInstance(typeof(ISessionRepository), mockSessionRepository);
			container.RegisterInstance(typeof(IHttpContextWrapper), mockHttpContext);
			var service = container.Resolve<RepositorySession>();
		}
	}
}

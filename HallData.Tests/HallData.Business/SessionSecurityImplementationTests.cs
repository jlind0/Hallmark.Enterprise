using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Security;
using HallData.Session;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HallData.Tests.HallData.Business
{
	/// <summary>
	/// Summary description for SessionSecurityImplementationTests
	/// </summary>
	[TestClass]
	public class SessionSecurityImplementationTests
	{
		public SessionSecurityImplementationTests()
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

		[TestMethod]
		public async Task SessionSecurityImplementation_SignIn_Success()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.Login("dweber", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SessionState()
			{
				User = new SecurityUser() {},
				ActivityCount = 5,
				LastActivityDate = new DateTime(),
				CreatedDate = new DateTime(),
				Guid = testSessionGuid,
				IsActive = true
			}
			));
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			Guid? sessionGuid = await ssImpl.SignIn("dweber");
			Assert.IsNotNull(sessionGuid);
			Assert.AreEqual(testSessionGuid, sessionGuid);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_SignOut_Success()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.Logout()).Returns(() => Task.FromResult(true));
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			bool signOutResult = await ssImpl.SignOut(testSessionGuid);
			Assert.IsNotNull(signOutResult);
			Assert.AreEqual(true, signOutResult);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_IsActiveSession_true()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.GetSession(It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SessionState()
				{
					User = new SecurityUser() { },
					ActivityCount = 5,
					LastActivityDate = new DateTime(),
					CreatedDate = new DateTime(),
					Guid = testSessionGuid,
					IsActive = true
				}
			));
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			bool isActiveResult = await ssImpl.IsActiveSession(testSessionGuid);
			Assert.IsNotNull(isActiveResult);
			Assert.AreEqual(true, isActiveResult);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_IsActiveSession_false()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.GetSession(It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SessionState()
				{
					User = new SecurityUser() { },
					ActivityCount = 5,
					LastActivityDate = new DateTime(),
					CreatedDate = new DateTime(),
					Guid = testSessionGuid,
					IsActive = false
				}
			));
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			bool isActiveResult = await ssImpl.IsActiveSession(testSessionGuid);
			Assert.IsNotNull(isActiveResult);
			Assert.AreEqual(false, isActiveResult);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_IsActiveSessionSync_true()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.GetSessionSync())
				.Returns(() => new SessionState()
				{
					User = new SecurityUser() { },
					ActivityCount = 5,
					LastActivityDate = new DateTime(),
					CreatedDate = new DateTime(),
					Guid = testSessionGuid,
					IsActive = true
				}
			);
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			bool isActiveResult = ssImpl.IsActiveSessionSync(testSessionGuid);
			Assert.IsNotNull(isActiveResult);
			Assert.AreEqual(true, isActiveResult);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_SignInSync_Success()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.LoginSync("dweber", It.IsAny<string>(), It.IsAny<string>()))
				.Returns(() => new SessionState()
				{
					User = new SecurityUser() { },
					ActivityCount = 5,
					LastActivityDate = new DateTime(),
					CreatedDate = new DateTime(),
					Guid = testSessionGuid,
					IsActive = true
				}
			);
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			Guid? sessionGuid = ssImpl.SignInSync("dweber");
			Assert.IsNotNull(sessionGuid);
			Assert.AreEqual(testSessionGuid, sessionGuid);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_SignInWindowsAuthSync_Success()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.LoginSync())
				.Returns(() => new SessionState()
				{
					User = new SecurityUser() { },
					ActivityCount = 5,
					LastActivityDate = new DateTime(),
					CreatedDate = new DateTime(),
					Guid = testSessionGuid,
					IsActive = true
				}
			);
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			Guid? sessionGuid = ssImpl.SignInWindowsAuthSync();
			Assert.IsNotNull(sessionGuid);
			Assert.AreEqual(testSessionGuid, sessionGuid);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_SignOutSync_Success()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.LogoutSync())
				.Returns(() => true);
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			bool signOutResult = ssImpl.SignOutSync(testSessionGuid);
			Assert.IsNotNull(signOutResult);
			Assert.AreEqual(true, signOutResult);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_GetSignedInUser_success()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.GetSession(It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SessionState()
				{
					User = new SecurityUser()
					{
						UserGuid = new Guid(),
						UserName = "hdsuser1",
						FirstName = "John",
						LastName = "Doe"
					},
					ActivityCount = 5,
					LastActivityDate = new DateTime(),
					CreatedDate = new DateTime(),
					Guid = testSessionGuid,
					IsActive = true
				}
			));
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			Task<SecurityUser> user = ssImpl.GetSignedInUser();
			Assert.IsNotNull(user);
			Assert.IsNotNull(user.Result);
			Assert.AreEqual("hdsuser1", user.Result.UserName);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_GetSignedInUserSync_true()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.GetSessionSync())
				.Returns(() => new SessionState()
				{
					User = new SecurityUser()
					{
						UserGuid = new Guid(),
						UserName = "hdsuser1",
						FirstName = "John",
						LastName = "Doe"
					},
					ActivityCount = 5,
					LastActivityDate = new DateTime(),
					CreatedDate = new DateTime(),
					Guid = testSessionGuid,
					IsActive = true
				}
			);
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			SecurityUser user = ssImpl.GetSignedInUserSync();
			Assert.IsNotNull(user);
			Assert.AreEqual("hdsuser1", user.UserName);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_MarkSessionActivity_success()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.GetUpdateSession(It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SessionState()
				{
					User = new SecurityUser()
					{
						UserGuid = new Guid(),
						UserName = "hdsuser1",
						FirstName = "John",
						LastName = "Doe"
					},
					ActivityCount = 5,
					LastActivityDate = new DateTime(),
					CreatedDate = new DateTime(),
					Guid = testSessionGuid,
					IsActive = true
				}
			));
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			bool result = await ssImpl.MarkSessionActivity();
			Assert.IsNotNull(result);
			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_MarkSessionActivitySync_success()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.GetUpdateSessionSync())
				.Returns(() => new SessionState()
				{
					User = new SecurityUser()
					{
						UserGuid = new Guid(),
						UserName = "hdsuser1",
						FirstName = "John",
						LastName = "Doe"
					},
					ActivityCount = 5,
					LastActivityDate = new DateTime(),
					CreatedDate = new DateTime(),
					Guid = testSessionGuid,
					IsActive = true
				}
			);
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			bool result = ssImpl.MarkSessionActivitySync();
			Assert.IsNotNull(result);
			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_IsCurrentSessionActive_true()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.IsCurrentSessionActive(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(true));
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			bool result = await ssImpl.IsCurrentSessionActive();
			Assert.IsNotNull(result);
			Assert.AreEqual(true, result);
		}

		[TestMethod]
		public async Task SessionSecurityImplementation_IsCurrentSessionActiveSync_true()
		{
			Guid testSessionGuid = new Guid("E06B48DB-9FC6-4CC5-9D33-952D8A32E264");

			Mock<ISession> mockSession = new Mock<ISession>();
			mockSession.Setup(
				s => s.IsCurrentSessionActiveSync()).Returns(() => true);
			SessionSecurityImplementation ssImpl = new SessionSecurityImplementation(mockSession.Object);
			bool result = ssImpl.IsCurrentSessionActiveSync();
			Assert.IsNotNull(result);
			Assert.AreEqual(true, result);
		}
	}
}

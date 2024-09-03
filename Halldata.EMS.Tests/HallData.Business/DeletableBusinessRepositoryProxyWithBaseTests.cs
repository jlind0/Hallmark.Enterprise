using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Data;
using HallData.EMS.Tests.HallData.Business.Mocks;
using HallData.Security;
using HallData.Validation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Tests.HallData.Business
{
	[TestClass]
	public class DeletableBusinessRepositoryProxyWithBaseTests
	{
		private IUnityContainer container;

		[TestInitialize()]
		public void Initialize()
		{
			Mock<ISecurityImplementation> mockSecurity = new Mock<ISecurityImplementation>();
			mockSecurity.Setup(s => s.GetSignedInUser(It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(new SecurityUser()
				{
					UserGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886"),
					UserName = "lzychowski",
					FirstName = null,
					LastName = null,
					OrganizationGuid = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7")
				}));
			mockSecurity.Setup(s => s.MarkSessionActivity(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(true));
			mockSecurity.Setup(s => s.IsActiveSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.Returns(() => Task.FromResult(true));
			mockSecurity.SetupAllProperties();

			ValidationResultFactory.Initialize((v, e) => v);

			container = new UnityContainer();
			container.RegisterType<IMockBusinessImplementation, MockBusinessImplementation>();
			container.RegisterInstance(typeof (ISecurityImplementation), mockSecurity.Object);
		}

		[TestMethod]
		public async Task BusinessImplementation_GetMany_NotNull()
		{
			PersonResult p1 = new PersonResult() {PartyGuid = Guid.NewGuid()};
			PersonResult p2 = new PersonResult() {PartyGuid = Guid.NewGuid()};
			PersonResult[] people = new PersonResult[] {p1, p2};
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof (IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			QueryResults<PersonResult> results = await mockBusiness.GetMany();

			Assert.IsNotNull(results);
			Assert.AreEqual(2, results.Results.Count());
		}

		[TestMethod]
		public async Task BusinessImplementation_Get_NotNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			PersonResult[] people = new PersonResult[] { p1 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			QueryResult<PersonResult> results = await mockBusiness.Get(id);

			Assert.IsNotNull(results);
			Assert.AreEqual(id, results.Result.PartyGuid);
		}

		[TestMethod]
		public async Task BusinessImplementation_GetView_NotNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			PersonResult[] people = new PersonResult[] { p1 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			QueryResult<JObject> results = await mockBusiness.GetView(id);

			Assert.IsNotNull(results);
			Assert.AreEqual(id, results.Result.GetValue("PartyGuid"));
		}

		[TestMethod]
		public async Task BusinessImplementation_GetManyView_NotNull()
		{
			PersonResult p1 = new PersonResult() { PartyGuid = Guid.NewGuid() };
			PersonResult p2 = new PersonResult() { PartyGuid = Guid.NewGuid() };
			PersonResult[] people = new PersonResult[] { p1, p2 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			QueryResults<JObject> results = await mockBusiness.GetManyView();

			Assert.IsNotNull(results);
			Assert.AreEqual(2, results.Results.Count());
		}

		[TestMethod]
		public async Task BusinessImplementation_ChangeStatusForce_NotNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			PersonResult[] people = new PersonResult[] { p1 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			ChangeStatusQueryResult<PersonResult> result = await mockBusiness.ChangeStatusForce(id, "Disabled");

			Assert.IsNotNull(result);
			Assert.AreEqual(id, result.Result.PartyGuid);
		}

		[TestMethod]
		public async Task BusinessImplementation_ChangeStatus_NotNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			PersonResult[] people = new PersonResult[] { p1 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			ChangeStatusQueryResult<PersonResult> result = await mockBusiness.ChangeStatus(id, "Disabled");

			Assert.IsNotNull(result);
			Assert.AreEqual(id, result.Result.PartyGuid);
		}

		[TestMethod]
		public async Task BusinessImplementation_ChangeStatusForceView_NotNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			PersonResult[] people = new PersonResult[] { p1 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			ChangeStatusQueryResult<JObject> result = await mockBusiness.ChangeStatusForceView(id, "Disabled");

			Assert.IsNotNull(result);
			Assert.AreEqual(id, result.Result.GetValue("PartyGuid"));
		}

		[TestMethod]
		public async Task BusinessImplementation_ChangeStatusView_NotNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			PersonResult[] people = new PersonResult[] { p1 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			ChangeStatusQueryResult<JObject> result = await mockBusiness.ChangeStatusView(id, "Disabled");

			Assert.IsNotNull(result);
			Assert.AreEqual(id, result.Result.GetValue("PartyGuid"));
		}

		[TestMethod]
		public async Task BusinessImplementation_AddView_NotNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			PersonResult[] people = new PersonResult[] { p1 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			PersonForAdd personforAdd = new PersonForAdd()
			{
				Status = new StatusTypeKey { StatusTypeId = 3},
				FirstName = "John",
				LastName = "Smith"
			};
			QueryResult<JObject> result = await mockBusiness.AddView(personforAdd);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Result);
			Assert.IsNotNull(result.Result.GetValue("PartyGuid"));
			Assert.AreEqual("John", result.Result.GetValue("FirstName"));
			Assert.AreEqual("Smith", result.Result.GetValue("LastName"));
		}

		[TestMethod]
		public async Task BusinessImplementation_Add_NotNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			PersonResult[] people = new PersonResult[] { p1 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			PersonForAdd personforAdd = new PersonForAdd()
			{
				Status = new StatusTypeKey { StatusTypeId = 3 },
				FirstName = "John",
				LastName = "Smith"
			};
			QueryResult<PersonResult> result = await mockBusiness.Add(personforAdd);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Result);
			Assert.IsNotNull(result.Result.PartyGuid);
			Assert.AreEqual("John", result.Result.FirstName);
			Assert.AreEqual("Smith", result.Result.LastName);
		}

		[TestMethod]
		public async Task BusinessImplementation_UpdateView_NotNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			PersonResult[] people = new PersonResult[] { p1 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			PersonForUpdate personforUpdate = new PersonForUpdate()
			{
				PartyGuid = id,
				FirstName = "Johnupdated",
				LastName = "Smithupdated"
			};
			QueryResult<JObject> result = await mockBusiness.UpdateView(personforUpdate);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Result);
			Assert.IsNotNull(result.Result.GetValue("PartyGuid"));
			Assert.AreEqual(id, result.Result.GetValue("PartyGuid"));
			Assert.AreEqual("Johnupdated", result.Result.GetValue("FirstName"));
			Assert.AreEqual("Smithupdated", result.Result.GetValue("LastName"));
		}

		[TestMethod]
		public async Task BusinessImplementation_Update_NotNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			PersonResult[] people = new PersonResult[] { p1 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();
			PersonForUpdate personforUpdate = new PersonForUpdate()
			{
				PartyGuid = id,
				FirstName = "Johnupdated",
				LastName = "Smithupdated"
			};
			QueryResult<PersonResult> result = await mockBusiness.Update(personforUpdate);

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Result);
			Assert.IsNotNull(result.Result.PartyGuid);
			Assert.AreEqual(id, result.Result.PartyGuid);
			Assert.AreEqual("Johnupdated", result.Result.FirstName);
			Assert.AreEqual("Smithupdated", result.Result.LastName);
		}

		// deletesoft
		[TestMethod]
		public async Task BusinessImplementation_DeleteSoft_IsNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			Guid id2 = new Guid("ff5d3ea5-1de8-460b-b5c7-5607cf2bdc85");
			PersonResult p2 = new PersonResult() { PartyGuid = id2 };

			PersonResult[] people = new PersonResult[] { p1, p2 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();

			QueryResults<PersonResult> resultsBefore = await mockBusiness.GetMany();

			await mockBusiness.DeleteSoft(id);

			QueryResults<PersonResult> resultsAfter = await mockBusiness.GetMany();

			Assert.IsTrue(resultsAfter.Results.Count() == 1);
			Assert.AreEqual(id2, resultsAfter.Results.First().PartyGuid);
		}

		[TestMethod]
		public async Task BusinessImplementation_DeleteHard_IsNull()
		{
			Guid id = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			PersonResult p1 = new PersonResult() { PartyGuid = id };
			Guid id2 = new Guid("ff5d3ea5-1de8-460b-b5c7-5607cf2bdc85");
			PersonResult p2 = new PersonResult() { PartyGuid = id2 };

			PersonResult[] people = new PersonResult[] { p1, p2 };
			MockPersonRepository repository = new MockPersonRepository(people);

			container.RegisterInstance(typeof(IPersonRepository), repository);
			IMockBusinessImplementation mockBusiness = container.Resolve<IMockBusinessImplementation>();

			mockBusiness.CurrentSessionId = Guid.NewGuid();

			QueryResults<PersonResult> resultsBefore = await mockBusiness.GetMany();

			await mockBusiness.DeleteHard(id);

			QueryResults<PersonResult> resultsAfter = await mockBusiness.GetMany();

			Assert.IsTrue(resultsAfter.Results.Count() == 1);
			Assert.AreEqual(id2, resultsAfter.Results.First().PartyGuid);
		}
	}
}

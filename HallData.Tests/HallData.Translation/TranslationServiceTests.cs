using System;
using System.Data.Services.Client;
using System.Threading;
using System.Threading.Tasks;
using HallData.Globalization;
using HallData.Security;
using HallData.Translation;
using HallData.Translation.Mocks;
using HallData.Validation;
using Microsoft;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HallData.Tests.HallData.Translation
{
	[TestClass]
	public class TranslationServiceTests
	{
		private IUnityContainer container;
		private Mock<ISecurityImplementation> mockSecurity;

		[TestInitialize()]
		public void Initialize()
		{
			mockSecurity = new Mock<ISecurityImplementation>();
			mockSecurity.Setup(s => s.GetSignedInUserSync())
				.Returns(() => new SecurityUser()
				{
					UserGuid = new Guid("B522CE71-F0CF-45D5-A88A-61DACD644886"),
					UserName = "lzychowski",
					FirstName = null,
					LastName = null,
					OrganizationGuid = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7"),
					Culture = Cultures.De
				});
			mockSecurity.SetupAllProperties();

			ValidationResultFactory.Initialize((v, e) => v);

			container = new UnityContainer();
			container.RegisterType<ITranslationService, TranslationService>();
			container.RegisterInstance(typeof(ISecurityImplementation), mockSecurity.Object);
		}

		[TestMethod]
		public void GetErrorMessage()
		{
			Mock<TranslatorContainer> mockTranslator = new Mock<TranslatorContainer>();
			Mock<IGlobalizationRepository> mockRepository = new Mock<IGlobalizationRepository>();
			mockRepository.Setup(s => s.GetErrorMessage(It.IsAny<string>(), Cultures.De))
				.Returns(() => "Das ist nicht richtig.");

			container.RegisterInstance(typeof (TranslatorContainer), mockTranslator.Object);
			container.RegisterInstance(typeof(IGlobalizationRepository), mockRepository.Object);
			var service = container.Resolve<ITranslationService>();

			string errorMessage = service.GetErrorMessage("ERROR_CODE");

			Assert.AreEqual("Das ist nicht richtig.", errorMessage);
		}

		//[TestMethod]
		//public void TranslationService_Translate_IsExecuted_IsEqual()
		//{
		//	Mock<DataServiceQuery<Microsoft.Translation>> mockTranslationQuery = new Mock<DataServiceQuery<Microsoft.Translation>>();
		//	mockTranslationQuery.Setup(s => s.Execute()).Returns(new Microsoft.Translation[]
		//	{
		//		new Microsoft.Translation()
		//		{
		//			Text = "Mein name ist Tim H."
		//		}
		//	});

		//	Mock<TranslatorContainer> mockTranslator = new Mock<TranslatorContainer>();

		//	bool executed = false;

		//	mockTranslator.Setup(s => s.Translate(It.IsAny<string>(), "de", "en")).Returns(() => mockTranslationQuery.Object)
		//		.Callback(() => executed = true);

		//	Mock<IGlobalizationRepository> mockGlobalization = new Mock<IGlobalizationRepository>();

		//	container.RegisterInstance(typeof (TranslatorContainer), mockTranslator.Object);
		//	container.RegisterInstance(typeof(IGlobalizationRepository), mockGlobalization.Object);
		//	ITranslationService service = container.Resolve<ITranslationService>();

		//	string translation = service.Translate("My name is Tim H.");

		//	Assert.IsTrue(executed);
		//	Assert.AreEqual("Mein name ist Tim H.", translation);
		//}
	}
}

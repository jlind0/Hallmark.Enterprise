using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Globalization;
using HallData.Security;
using Microsoft;
using HallData.Business;

namespace HallData.Translation
{
	public class TranslationService : ITranslationService
	{
		protected TranslatorContainer Translator { get; private set; }
		protected IGlobalizationRepository Repository { get; private set; }
		protected ISecurityImplementation Security { get; private set; }
		public TranslationService(IGlobalizationRepository repository, ISecurityImplementation security, TranslatorContainer translator)
		{
			this.Translator = translator;
			this.Repository = repository;
			this.Security = security;
		}

		public string Translate(string englishMessage)
		{
			var user = this.Security.GetSignedInUserSync();

			if (user == null)
				return englishMessage;
			string culture = "en";

			switch (user.Culture)
			{
				case Cultures.De:
				case Cultures.De_De: culture = "de"; break;
				case Cultures.Es:
				case Cultures.Es_ES:
				case Cultures.Es_MX: culture = "es"; break;
				case Cultures.Fr:
				case Cultures.Fr_CA:
				case Cultures.Fr_Fr: culture = "fr"; break;
			}

			if (culture == "en")
				return englishMessage;

			return this.Translator.Translate(englishMessage, "en", culture).Execute().Select(t => t.Text).FirstOrDefault();
		}

		public string GetErrorMessage(string errorCode)
		{
			Cultures culture = Cultures.En;
			var user = this.Security.GetSignedInUserSync();
			if (user != null)
				culture = user.Culture;
			return this.Repository.GetErrorMessage(errorCode, culture);
		}
	}
}

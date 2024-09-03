using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Globalization;
using HallData.Business;
using HallData.Security;

namespace HallData.Translation.Mocks
{
	public class MockTranslationService : ITranslationService
	{
		public string Translate(string englishMessage)
		{
			return englishMessage;
		}

		public string GetErrorMessage(string errorCode)
		{
			return errorCode;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Website.Security;

namespace HallData.EMS.Tests.User
{
	[TestClass]
	public class AddUserTest
	{

		private string sessionId = null;
		private string baseUrl = "http://localhost:8080";

		public void SessionCreate_ShouldReturnSessionId()
		{
			createSession();
		}

		[TestMethod]
		public void AddUser()
		{
			string sessionId = createSession();
			string resource = "/HallData.EMS.Web/api/users";

			RestClient client = new RestClient(this.baseUrl);
			RestRequest request = new RestRequest(resource, Method.POST);
			request.Timeout = 600000;
			request.AddParameter("text/json", "{\"PrimaryEmail\": {\"ContactMechanismTypeName\": \"Work\",\"EmailAddress\": \"brtest007@test.com\"},\"Username\": \"LZ_07_07_2015_1\",\"Password\": \"test123456\",\"FirstName\": \"Brandon\",\"LastName\": \"Rogers\",\"Status\": {\"StatusTypeId\":3}}", ParameterType.RequestBody);
			request.AddHeader("session.id", sessionId);
			IRestResponse response = client.Execute(request);
			var r = response;
		}

		public string createSession()
		{
			string userName = "Lzychowski";
			Guid organizationId = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
			EmsTokenizer tokenizer = new EmsTokenizer();
			string token = tokenizer.TokenizeUserNameOrganizationId(userName, organizationId);
			Assert.IsNotNull(token, "Token is null");
			RestClient client = new RestClient(this.baseUrl);
			string resource = "/HallData.EMS.Web/api/Users/Lzychowski/Sessions/Create?token={token}";
			RestRequest request = new RestRequest(resource, Method.GET);
			request.AddUrlSegment("token", token);
			IRestResponse response = client.Execute(request);
			this.sessionId = JsonConvert.DeserializeObject<String>(response.Content); ;
			Assert.IsNotNull(this.sessionId);
			return sessionId;
		}
	}
}

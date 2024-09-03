using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HallData.EMS.Security.Tokenizer;
using HallData.EMS.Business;
using RestSharp;
using RestSharp.Contrib;
using Newtonsoft.Json;
namespace HallData.EMS.Tests
{
    [TestClass]
    public class TestSession
    {
        [TestMethod]
        public void SessionCreate_ShouldReturnSessionId()
        {
            string userName = "thunnewell";
            Guid organizationId = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
            EmsTokenizer tokenizer = new EmsTokenizer();
            string token = tokenizer.TokenizeUserNameOrganizationId(userName, organizationId);
            Assert.IsNotNull(token, "Token is null");
            //UserImplementation user = new UserImplementation();
            string baseUrl = "http://localhost:8080";
            RestClient client = new RestClient(baseUrl);
            string resource = "/HallData.EMS.Web/api/Users/thunnewell/Sessions/Create?token={token}";
            RestRequest request = new RestRequest(resource, Method.GET);
            request.AddUrlSegment("token", token);
            IRestResponse response = client.Execute(request);
            string sessionid = JsonConvert.DeserializeObject<String>(response.Content); ;
            Assert.IsNotNull(sessionid);
        }
    }
}

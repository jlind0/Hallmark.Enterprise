using System;
using HallData.EMS.Security.Tokenizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;

namespace HallData.EMS.Tests
{
    [TestClass]
    public class CategoryTest
    {
        [TestMethod]
        public void GetAllCategories_ShouldReturnCategoryList()
        {
            //string baseUrl = "http://localhost:8080";
            //RestClient client = new RestClient(baseUrl);
            //string resource = "/HallData.EMS.Web/api/categories";
            //RestRequest request = new RestRequest(resource, Method.GET);
            //IRestResponse response = client.Execute(request);
            //string json = JsonConvert.DeserializeObject<String>(response.Content); ;
            //Assert.IsNotNull(json);
        }
    }
}

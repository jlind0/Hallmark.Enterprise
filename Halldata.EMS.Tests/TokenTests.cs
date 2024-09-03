using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HallData.EMS.Security.Tokenizer;

namespace HallData.EMS.Tests
{
    [TestClass]
    public class TestTokenGenerator
    {
        [TestMethod]
        public void TokenGenerator_ShouldReturnToken()
        {
            string userName = "thunnewell";
            Guid organizationId = new Guid("6063C0A5-4B41-4189-AE18-455F092ABCA7");
            EmsTokenizer tokenizer = new EmsTokenizer();
            string token = tokenizer.TokenizeUserNameOrganizationId(userName, organizationId);
            Assert.IsNotNull(token,"Token is null");
        }
    }
}

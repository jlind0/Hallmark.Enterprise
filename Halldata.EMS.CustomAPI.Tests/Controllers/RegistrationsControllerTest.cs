using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Halldata.EMS.CustomAPI;
using Halldata.EMS.CustomAPI.apazine.Controllers;

namespace Halldata.EMS.CustomAPI.Tests.Controllers
{
    [TestClass]
    public class RegistrationsControllerTest
    {
        [TestMethod]
        public void TestIndex_ShouldReturnRegistrationInfo()
        {
            RegistrationsController controller = new RegistrationsController();
            JsonResult result = controller.Index("AvvD5Tt6Uy78jH1277Df3F", "jsford@up.com", "peking") as JsonResult;
            Assert.AreEqual(((Halldata.EMS.CustomAPI.apazine.Models.RegistrationInfo)(result.Data)).FirstName, "JOSHUA");
            Assert.AreEqual(((Halldata.EMS.CustomAPI.apazine.Models.RegistrationInfo)(result.Data)).LastName, "FORD");
        }
        [TestMethod]
        public void TestIndex_ShouldReturnDebugRegistrationInfo()
        {
            RegistrationsController controller = new RegistrationsController();
            JsonResult result = controller.Index("AvvD5Tt6Uy78jH1277Df3F", "jsford@up.com", "peking", "true") as JsonResult;
            Assert.AreEqual(((Halldata.EMS.CustomAPI.apazine.Models.RegistrationInfo)(result.Data)).FirstName, "JOSHUA");
            Assert.AreEqual(((Halldata.EMS.CustomAPI.apazine.Models.DebugRegistrationInfo)(result.Data)).AccountNumber, "619371016");
        }
    }
}

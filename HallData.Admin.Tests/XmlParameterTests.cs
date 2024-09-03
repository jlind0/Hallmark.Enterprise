using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HallData.Data;
using HallData.Admin.ApplicationViews;
using System.Collections.Generic;
using HallData.ApplicationViews;
using System.Data.SqlTypes;

namespace HallData.Admin.Tests
{
    [TestClass]
    public class XmlParameterTests
    {
        [TestMethod]
        public void ApplicationViewSpec_Parameters_Populated()
        {
            Database db = DatabaseFactory.CreateDatabase("hds");
            ApplicationViewSpecForAddUpdate spec = new ApplicationViewSpecForAddUpdate();
            spec.PageOptions = new List<PageOption>();
            spec.PageOptions.Add(new PageOption() { PageSize = 10 });
            spec.PageOptions.Add(new PageOption() { PageSize = 25 });
            spec.PageOptions.Add(new PageOption() { PageSize = 50 });
            var cmd = db.CreateStoredProcCommand("test");
            cmd.MapParameters(spec, db, ViewOperations.Add);
            var parm = cmd.Parameters["PageOptions"];
            Assert.IsNotNull(parm);
            SqlXml xml = parm.Value as SqlXml;
            Assert.IsNotNull(xml);
            Assert.IsNotNull(xml.Value);

        }
    }
}

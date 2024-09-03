using Halldata.EMS.CustomAPI.apazine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Halldata.EMS.CustomAPI.apazine.Controllers
{
    public class RegistrationsController : Controller
    {
        public ActionResult Index(string API_Key, string UserName, string Password, string debug = "")
        {
            bool d = debug == "true" ? true : false;
            if (API_Key == Properties.Settings.Default.API_Key)
            {
                RegistrationInfo registrationinfo = new RegistrationModel().Lookup(UserName, Password, d);
                return Json(registrationinfo, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Error = "Invalid API Key" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
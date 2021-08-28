using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CarsBiddingUsingBootstrap.Controllers
{
    public class HelperController : Controller
    {
        public JsonResult IsUserAuthorize()
        {
            return Json(User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
        }
    }
}
using CarsBiddingUsingBootstrap.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Controllers
{
    public class LanguageController : Controller
    {
        public RedirectResult ChangeLanguage(string lang,string returnUrl)
        {
            HttpCookie langCookie = new HttpCookie("culture", lang);
            langCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(langCookie);
            return Redirect(returnUrl);
        }
    }
}
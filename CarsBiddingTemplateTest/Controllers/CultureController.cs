using CarsBiddingUsingBootstrap.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Controllers
{
    public class CultureController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            /*
             * way2: you can pu this code in Application_BeginRequest() event 
             * in Global.asax file which Application_BeginRequest() event  will
             * fire with every application request
             */
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                lang = LanguageManager.GetDefaultLanguage();
            }
            LanguageManager.SetLanguage(lang);
            return base.BeginExecuteCore(callback, state);
        }
    }
}
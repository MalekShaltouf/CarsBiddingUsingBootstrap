using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarsBiddingUsingBootstrap
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            /*
            * this special case => sometimes we close the application => so 
            * when re-open the application again => will note that application 
            * still login => but the AllUserNotification static property will be 
            * null because we fill it always in SignIn[Post] action 
            * so in this case we want to re-populate AllUserNotification property
            * 
            * why we did it Session_Start Event? => because this event fire when new session start
            * 
            * the session that i mean => is session has not any relation with user login but 
            * it is session_id that asp.net give it for every user open application() => 
            * example on session_id that asp.net give it to user when open the application
            * in Chrome will give you session_id and when open the application in another 
            * browser such as : Microsft Edge the asp.net will give you another new Session_id
            * 
            * so when new Session_Start Event fire we want to check if user is Authenticaticated or 
            * not => which if user Authenticated & the AllUserNotification static property is null
            * we want to fill it.
            */
            bool IsUserAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated;
            if (IsUserAuthenticated  && NotificationHistoryViewModel.AllUserNotification == null)
            {
                int UserId = int.Parse(User.Identity.Name.Split('|').LastOrDefault());
                NotificationHistoryViewModel.PopulateAllUserNotificationInMemory(UserId);
            }
        }
    }
}

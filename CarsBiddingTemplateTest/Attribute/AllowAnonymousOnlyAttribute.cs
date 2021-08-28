using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Attribute
{
    public class AllowAnonymousOnlyAttribute : AuthorizeAttribute
    {
        /*
         * when use this attribute at action method or at Controller level 
         * this person to access to action method if he is not authenticated(just allow for anonymous user)
         * 
         * i wanted to use it at SignIn action which i wanted to prevent authenticated
         * user to access to it but there are problem that when we use it then
         * the user tried to access to SignIn page will redirect it to SignIn Page
         * which we specified it in web.config file 
         * 
         * <authentication mode="Forms">
         *   <forms loginUrl="Home/Index"></forms>
         * </authentication>
         * 
         * so because that we used this AllowAnonymousOnlyAttribute so when redirect to SignIn Page
         * the compiler will find that SignIn have AllowAnonymousOnlyAttribute so will return to you
         * "HTTP Error 401.0 - Unauthorized 
         * You do not have permission to view this directory or page."
         * 
         * way2 to do this:
         * we will check  in SignIn action if user Athenticated will redirect it to a certain
         *    page such as:
         * 
         * if(!User.Identity.IsAthenticated){
         *      RedirectToAction(....)
         * }
         */
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // make sure the user is not authenticated. If it's not, return true. Otherwise, return false
            if (HttpContext.Current.User.Identity.IsAuthenticated) 
            {
                return false;
            }
            return true;
        }
    }
}
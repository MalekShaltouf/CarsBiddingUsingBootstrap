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
            try
            {
                return Json(User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[GET] IsUserAuthorize action, Helper Controller");
                throw ex;
            }
        }
        public JsonResult IsUserCarOwner(int CarId)
        {
            try
            {
                using (CarsBiddingEntities context = new CarsBiddingEntities())
                {
                    //step1:we want to return Car Owner
                    int? CarOwnerUserId = context.Cars_Info.SingleOrDefault(car => car.CarId == CarId).UserId;
                    int? UserId = int.Parse(User.Identity.Name.Split('|').LastOrDefault());

                    return Json(CarOwnerUserId == UserId, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[GET] IsUserCarOwner action, Helper Controller");
                throw ex;
            }

        }
    }
}
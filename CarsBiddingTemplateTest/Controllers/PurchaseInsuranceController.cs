using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Controllers
{
    public class PurchaseInsuranceController : Controller
    {
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PurchaseInsurance(InsuranceViewModel model)
        {
            NotificationParameter notification = new NotificationParameter();
            try
            {
                if (ModelState.IsValid) 
                {
                    using (CarsBiddingEntities context = new CarsBiddingEntities())
                    {
                        Insurance insurance = new Insurance();
                        insurance.CarId = model.CarId;
                        insurance.IsInsuranceForPurchase = true;
                        insurance.UserId = int.Parse(User.Identity.Name.Split('|').LastOrDefault());

                        context.Insurances.Add(insurance);
                        context.SaveChanges();

                        notification.Type = CarsBiddingUsingBootstrap.Localization.SUCCESS;
                        notification.Msg = CarsBiddingUsingBootstrap.Localization.InsurancePaidSuccessfully;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Post] PurchaseInsurance action,PurchaseInsurance Controller");
                notification.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                notification.Msg = ex.Message;
            }
            
            return Json(notification);
        }
    }
}
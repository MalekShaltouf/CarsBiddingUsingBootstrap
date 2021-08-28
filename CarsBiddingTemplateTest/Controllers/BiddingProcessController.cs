using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Controllers
{
    [Authorize]
    public class BiddingProcessController : Controller
    {
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BiddingProcess(BiddingViewModel biddingModel)
        {
            NotificationParameter notification = new NotificationParameter();
            try
            {
                if (ModelState.IsValid) 
                {
                    if (biddingModel.NewPrice <= biddingModel.CurrentPrice)
                    {
                        /*
                         * here repeat validation but on serverside level
                         */
                        notification.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                        notification.Msg = CarsBiddingUsingBootstrap.Localization.NewPriceValidation;
                        /*
                         * here we send notification object not biddingModel object
                         * because we just need Type & Msg property
                         */
                        return Json(notification);
                    }
                    using (CarsBiddingEntities context = new CarsBiddingEntities()) 
                    {
                        Bidding bidding = new Bidding()
                        {
                            UserId = int.Parse(User.Identity.Name.Split('|').LastOrDefault()),
                            CarId = (int)biddingModel.CarId,
                            CurrentPrice = biddingModel.NewPrice
                        };
                        context.Entry(bidding).State = EntityState.Modified;
                        context.SaveChanges();

                        biddingModel.Type = CarsBiddingUsingBootstrap.Localization.SUCCESS;
                        biddingModel.Msg = CarsBiddingUsingBootstrap.Localization.BidProcessSuccessfully;
                    }
                }
            }
            catch (Exception ex) 
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Post] BiddingProcess action,BiddingProcess Controller");
                biddingModel.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                biddingModel.Msg = ex.Message;
            }
            return Json(biddingModel);
        }
        public JsonResult IsInsurancePaid(int? carId) 
        {
            using (CarsBiddingEntities context = new CarsBiddingEntities()) 
            {
                int UserId = int.Parse(User.Identity.Name.Split('|').LastOrDefault());
                return Json(context.Insurances.Any(Insurance => Insurance.UserId == UserId && Insurance.CarId == carId),JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LatestPrice(int? CarId) 
        {
            using (CarsBiddingEntities context = new CarsBiddingEntities()) 
            {
                int UserId = int.Parse(User.Identity.Name.Split('|').LastOrDefault());
                int? currentPrice = context.Biddings.SingleOrDefault(bidding => bidding.UserId == UserId && bidding.CarId == CarId).CurrentPrice; 
                return Json(currentPrice,JsonRequestBehavior.AllowGet);
            }
        }
    }
}
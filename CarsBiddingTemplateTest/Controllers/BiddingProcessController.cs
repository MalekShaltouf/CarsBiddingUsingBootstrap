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
                    using (CarsBiddingEntities context = new CarsBiddingEntities())
                    {
                        Bidding PreviousbiddingProcess = context.Biddings.AsNoTracking().SingleOrDefault(bid => bid.CarId == biddingModel.CarId);
                        /*
                         * why we used AsNoTracking()??
                         * because if you trying to return old recorde then try to update on it
                         * using Entity FrameWork will return to you this error "Attaching an entity of type 'CarsBiddingUsingBootstrap.Models.Bidding'
                         * failed because another entity of the same type already has the same primary key value. This can happen when using the 'Attach'
                         * method or setting the state of an entity to 'Unchanged' or 'Modified' if any entities in the graph have conflicting key values.
                         * This may be because some entities are new and have not yet received database-generated key values. In this case use the 'Add' method
                         * or the 'Added' entity state to track the graph and then set the state of non-new entities to 'Unchanged' or 'Modified' as appropriate."
                         * 
                         * so to in order to be able to return old recorde then update on it we need to use
                         * AsNoTracking() function when trying to return old recorde
                         */
                        Bidding bidding = new Bidding()
                        {
                            UserId = int.Parse(User.Identity.Name.Split('|').LastOrDefault()),
                            CarId = biddingModel.CarId,
                            UserType = Convert.ToInt32(BiddingUserType.LastUserBidToCar),
                            CurrentPrice = biddingModel.NewPrice
                        };
                        context.Entry(bidding).State = EntityState.Modified;
                        context.SaveChanges();

                        /*
                         * after any user increase car price and there are previsou user
                         * also increase on car
                         * example to explain => suppose the owner he want enter car with initial 
                         * price = 5000 then =>UserA => increase price to 6000 then 
                         * UserB => increase price to 7000 so we want to send to previous user(UserA)
                         * notification that car price has been increased 
                         * 
                         * spicial case:when previous user(UserA) is same current user(UserB is same UserA)
                         * so in this case we don't need to send a notification
                         */
                        /*[start]
                         * because the Native message contain mix arabic * english 
                         * we needed to use  ((char)0x200E).ToString() to handle the
                         * problem that occurring because English (LTR) & Arabic(RTL)
                         * without use ((char)0x200E).ToString() the message will not
                         * display in correct way
                         */

                        //step1:we want to check if there are previous user or not & the current user not same previous user
                        if (PreviousbiddingProcess.UserType == Convert.ToInt32(BiddingUserType.LastUserBidToCar) && int.Parse(User.Identity.Name.Split('|').LastOrDefault()) != PreviousbiddingProcess.UserId)
                        {
                            //here mean that there are previous user

                            //step2:we want to get carinfo
                            CarsInfoViewModel carmodel = context.Cars_Info.Select(car => new CarsInfoViewModel()
                            {
                                UserId = car.UserId,
                                CarId = car.CarId,
                                TypeOfCar = car.TypeOfCar,
                                MainPhoto = car.MainPhoto,
                                YearOfManufacture = car.YearOfManufacture
                            }).SingleOrDefault(car => car.CarId == PreviousbiddingProcess.CarId);
                            double? increaseamount = biddingModel.NewPrice - PreviousbiddingProcess.CurrentPrice;
                            string lrm = ((char)0x200e).ToString();
                            string nativemessage = "." + biddingModel.NewPrice + " " + lrm + "ليصبح السعر النهائي " + lrm + increaseamount + " " + lrm + "بقيمة " + lrm + carmodel.TypeOfCar + " " + carmodel.YearOfManufacture + " " + lrm + "تم زيادة سعر سيارة ";
                            //[end]
                            string englishmessage = "The price of " + carmodel.TypeOfCar + " " + carmodel.YearOfManufacture + " car has been increased by " + increaseamount + " so the final price will be " + biddingModel.NewPrice;
                            int? previoususerid = PreviousbiddingProcess.UserId;
                            NotificationHistory previoususernotification = NotificationHistoryViewModel.PopulateNotificationInfo(previoususerid, carmodel.CarId, englishmessage, nativemessage, carmodel.MainPhoto);
                            context.NotificationHistories.Add(previoususernotification);
                            context.SaveChanges();
                        }

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
                return Json(context.Insurances.Any(Insurance => Insurance.UserId == UserId && Insurance.CarId == carId), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LatestPrice(int? CarId)
        {
            using (CarsBiddingEntities context = new CarsBiddingEntities())
            {
                double? currentPrice = context.Biddings.SingleOrDefault(bidding => bidding.CarId == CarId).CurrentPrice;
                return Json(currentPrice, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult IsNewPriceGreaterThanCurrentPrice(double NewPrice, double CurrentPrice) 
        {
            if (NewPrice <= CurrentPrice) 
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
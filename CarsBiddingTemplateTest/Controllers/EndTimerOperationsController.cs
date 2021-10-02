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
    public class EndTimerOperationsController : Controller
    {
        // GET: EndTimerOperations
        public ActionResult EndTimerOperations(int CarId)
        {
            //step1:we want to make Timer_Status is End (false)
            CarsInfoViewModel carsInfoViewModel = new CarsInfoViewModel();
            try
            {
                //throw new Exception();
                using (CarsBiddingEntities context = new CarsBiddingEntities())
                {
                    /*
                     * how to update one filed using EFramework
                     */
                    //step1:should be fill PK columns in Cars_Info object
                    Cars_Info car_info = new Cars_Info()
                    {
                        CarId = CarId,
                        Timer_Status = false
                    };
                    context.Cars_Info.Attach(car_info);
                    context.Entry(car_info).Property(c => c.Timer_Status).IsModified = true;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[GET] EndTimerOperations action,EndTimerOperations Controller");
                carsInfoViewModel.Type = "ERROR";
                carsInfoViewModel.LocalizedType = Localization.ERROR;
                carsInfoViewModel.Msg = ex.Message;
            }
            return Json(carsInfoViewModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult isTimerEnd(int CarId)
        {
            using (CarsBiddingEntities context = new CarsBiddingEntities())
            {
                bool? isTimerEnd = !context.Cars_Info.SingleOrDefault(car => car.CarId == CarId).Timer_Status;
                return Json(isTimerEnd, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GenerateNotification(int CarId)
        {
            CarsInfoViewModel carModel = new CarsInfoViewModel();
            try
            {
                using (CarsBiddingEntities context = new CarsBiddingEntities())
                {
                    //step1: we  want to retuern UserId for Car Owner & Car info 
                    carModel = context.Cars_Info.Select(car => new CarsInfoViewModel()
                    {
                        UserId = car.UserId,
                        CarId = car.CarId,
                        TypeOfCar = car.TypeOfCar,
                        MainPhoto = car.MainPhoto,
                        YearOfManufacture = car.YearOfManufacture
                    }).SingleOrDefault(car => car.CarId == CarId);

                    //step2:we want to return UserId for user that won the car & BiddingUserType
                    Bidding bid = context.Biddings.SingleOrDefault(b => b.CarId == CarId);

                    //step2:we want to Generate Notification for Owner & Winner Car

                    /*[start]
                     * because the Native message contain mix arabic * english 
                     * we needed to use  ((char)0x200E).ToString() to handle the
                     * problem that occurring because English (LTR) & Arabic(RTL)
                     * without use ((char)0x200E).ToString() the message will not
                     * display in correct way
                     */
                    string LRM = ((char)0x200E).ToString();
                    string NativeMessage = null;
                    //[End]
                    string EnglishMessage = null;
                    int? CarOwnerUserId = carModel.UserId;
                    if (bid.UserType == Convert.ToInt32(BiddingUserType.CarOwner))
                    {
                        /*
                         * if the BiddingUserType is CarOwner that's mean the timer end &
                         * there aren't any one bidding to car so we want to send notification 
                         * just for Car Owner that car not sold
                         */

                        NativeMessage = "." + "لانه لا يوجد أحد قام بالمزايدة عليها " + LRM + carModel.TypeOfCar + " " + carModel.YearOfManufacture + " " + LRM + "انتهى الوقت ولم يتم بيع سيارة ";
                        EnglishMessage = "The Timer End and Your " + carModel.TypeOfCar + " " + carModel.YearOfManufacture + " car not sold because there aren't any one has bid on it.";
                        NotificationHistory CarOwnerNotificationHis = NotificationHistoryViewModel.PopulateNotificationInfo(CarOwnerUserId, CarId, EnglishMessage, NativeMessage, carModel.MainPhoto);
                        context.NotificationHistories.Add(CarOwnerNotificationHis);
                    }
                    else
                    {
                        /*
                         * here mean that car sold successfully so we want 
                         * to send notification for Car Owner & Winner 
                         */
                        //A- for Owner Car
                        NativeMessage = "." + "بنجاح " + LRM + carModel.TypeOfCar + " " + carModel.YearOfManufacture + " " + LRM + "تم بيع سيارة ";
                        EnglishMessage = "Your " + carModel.TypeOfCar + " " + carModel.YearOfManufacture + " car sold successfully.";
                        NotificationHistory CarOwnerNotificationHistory = NotificationHistoryViewModel.PopulateNotificationInfo(CarOwnerUserId, CarId, EnglishMessage, NativeMessage, carModel.MainPhoto);
                        context.NotificationHistories.Add(CarOwnerNotificationHistory);

                        //B for winner car
                        NativeMessage = carModel.TypeOfCar + " " + carModel.YearOfManufacture + " لقد فزت بمزاد سيارة،" + LRM + "تهانينا";
                        EnglishMessage = "Congrats,you won in " + carModel.TypeOfCar + " " + carModel.YearOfManufacture + " auction";
                        //B.1:we want to return UserId for user that won the car
                        int? CarWinnerUserId = bid.UserId;
                        NotificationHistory CaWinnerNotificationHistory = NotificationHistoryViewModel.PopulateNotificationInfo(CarWinnerUserId, CarId, EnglishMessage, NativeMessage, carModel.MainPhoto);
                        context.NotificationHistories.Add(CaWinnerNotificationHistory);
                    }
                    context.SaveChanges();
                    carModel.Type = "WARNING";
                    carModel.LocalizedType = Localization.WARNING;
                    carModel.Msg = Localization.AuctionTimeEnded;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[GET] GenerateNotification action,EndTimerOperations Controller");
                carModel.Type = "ERROR";
                carModel.LocalizedType = Localization.ERROR;
                carModel.Msg = ex.Message;
            }
            return Json(carModel, JsonRequestBehavior.AllowGet);
        }
    }
}
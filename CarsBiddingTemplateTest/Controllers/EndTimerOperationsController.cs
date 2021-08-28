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
                using (CarsBiddingEntities context = new CarsBiddingEntities()) 
                {
                    /*
                     * this query return car Info
                     * 
                     * 1- in this query we used case when using linq in order to 
                     *    convert properties such as TypeOfTransmissionGear from numbers to 
                     *    string ex: 0 => mean NormalGear,1 => AutoGear
                     * 
                     * 2- in this query we use ViewModel to show data because EFramework 
                     *    prevent us to store data in Model that EFramework Generated it in other word
                     *    in Model represent table.(if use Cars_Info model the EFraemwork will return
                     *    "The entity or complex type 'SampleModel.Employee' cannot be constructed in a LINQ to Entities query.")
                     *    
                     * 3- in this query we did inner join with Bidding table in order to get
                     *    Current Price column.
                     */
                    CarsInfoViewModel CarModel = (from car in context.Cars_Info
                                                  join bidding in context.Biddings
                                                  on car.CarId equals bidding.CarId
                                                  where car.CarId == CarId
                                                  select new CarsInfoViewModel
                                                  {
                                                      CarId = car.CarId,
                                                      TypeOfCar = car.TypeOfCar,
                                                      ColorOfCar = car.ColorOfCar,
                                                      EngineCapacity = car.EngineCapacity,
                                                      YearOfManufacture = car.YearOfManufacture,
                                                      InitialPrice = car.InitialPrice,
                                                      CarChecking = car.CarChecking,
                                                      Description = car.Description,
                                                      MainPhoto = car.MainPhoto,
                                                      Photo1 = car.Photo1,
                                                      Photo2 = car.Photo2,
                                                      Photo3 = car.Photo3,
                                                      Photo4 = car.Photo4,
                                                      Photo5 = car.Photo5,
                                                      InsuranceForSale = car.InsuranceForSale,
                                                      UserId = car.UserId,
                                                      Kilometers = car.Kilometers,
                                                      Create_Date = car.Create_Date,
                                                      CurrentPrice = bidding.CurrentPrice,
                                                      CarCustoms =
                                                      (
                                                          car.CarCustoms == false ? CarsBiddingUsingBootstrap.Localization.WithoutCustoms : CarsBiddingUsingBootstrap.Localization.WithCustoms
                                                      ),
                                                      CarInsurance =
                                                      (
                                                          car.CarInsurance == 0 ? CarsBiddingUsingBootstrap.Localization.WithoutInsurance : car.CarInsurance == 1 ? CarsBiddingUsingBootstrap.Localization.MandatoryInsurance : CarsBiddingUsingBootstrap.Localization.ComprehensiveInsurance
                                                      ),
                                                      CarLicense =
                                                      (
                                                          car.CarLicense == false ? CarsBiddingUsingBootstrap.Localization.WithoutLicense : CarsBiddingUsingBootstrap.Localization.WithLicense
                                                      ),
                                                      TypeOfTransmissionGear =
                                                      (
                                                          car.TypeOfTransmissionGear == false ? CarsBiddingUsingBootstrap.Localization.NormalGear : CarsBiddingUsingBootstrap.Localization.AutomaticGear
                                                      ),
                                                  }).FirstOrDefault();

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
                carsInfoViewModel.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                carsInfoViewModel.Msg = ex.Message;
            }
            return View("~/Views/Car/CarDetails.cshtml", carsInfoViewModel);
        }
        public JsonResult isTimerEnd(int CarId) 
        {
            using(CarsBiddingEntities context = new CarsBiddingEntities()) 
            {
                bool? isTimerEnd = !context.Cars_Info.SingleOrDefault(car => car.CarId == CarId).Timer_Status;
                return Json(isTimerEnd, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GenerateNotificationForOwner(int CarId)
        {
            try
            {
                using (CarsBiddingEntities context = new CarsBiddingEntities()) 
                {
                    //step1: we  want to retuern UserId for Car Owner
                    CarsInfoViewModel carModel = context.Cars_Info.Select(car => new CarsInfoViewModel() 
                    {
                        CarId = car.CarId,
                        TypeOfCar = car.TypeOfCar,
                        MainPhoto = car.MainPhoto,
                        YearOfManufacture = car.YearOfManufacture
                    }).SingleOrDefault(car => car.CarId == CarId);
                    /*[start]
                     * because the Native message contain mix arabic * english 
                     * we needed to use  ((char)0x200E).ToString() to handle the
                     * problem that occurring because English (LTR) & Arabic(RTL)
                     * without use ((char)0x200E).ToString() the message will not
                     * display in correct way
                     */
                    string LRM = ((char)0x200E).ToString();  
                    string nativeMessage = "." + "بنجاح " + LRM + carModel.TypeOfCar + " " + carModel.YearOfManufacture + " " + LRM + "تم بيع سيارة ";
                    //[End]
                    NotificationHistory notificationHis = new NotificationHistory()
                    {
                        UserId = carModel.CarId,
                        EnglishMessage = "Your " + carModel.TypeOfCar + carModel.YearOfManufacture  + "car sold successfully.",
                        NativeMessage = nativeMessage,
                        NotificationStatus = false,//false mean that notification not opened yet.
                        Time = DateTime.Now,
                        MainPhoto = carModel.MainPhoto
                    };
                    context.NotificationHistories.Add(notificationHis);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[GET] GenerateNotificationForOwner action,EndTimerOperations Controller");
            }
            return Json("");
        }

    }
}
using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Controllers
{
    public class GalleryController : Controller
    {
        /*
         * in this controller we put Type & Msg in ViewBag
         * not in NotificationParameter because Gallery view 
         * is list of object(IPagedList<CarsInfoViewModel>) not one object
         * so in this case it's hard to use Type & Msg property that exits in 
         * NotificationParameter object so we use ViewBag
         */
        // GET: Gallery
        public ActionResult Gallery(int? page)
        {
            try
            {
                /*
                 * here we use Model View 'CarsInfoViewModel' to show data 
                 * because we use join
                 * if we use join and try to store data in 
                 * model represent table as Cars_Info the EFramework will return this 
                 * error "The entity or complex type 'CarsBiddingModel.Cars_Info' cannot be constructed in a LINQ to Entities query."
                 */
                ViewBag.TypeOfTransmissionGear = Helper.fillTypeOfTransmissionGearDropDownList();
                List<CarsInfoViewModel> cars = null;
                using (CarsBiddingEntities context = new CarsBiddingEntities())
                {
                    cars = (
                        from car in context.Cars_Info
                        join bidding in context.Biddings
                        on car.CarId equals bidding.CarId
                        where car.Timer_Status == true
                        select new CarsInfoViewModel
                        {
                            CarId = car.CarId,
                            TypeOfCar = car.TypeOfCar,
                            ColorOfCar = car.ColorOfCar,
                            YearOfManufacture = car.YearOfManufacture,
                            MainPhoto = car.MainPhoto,
                            CurrentPrice = bidding.CurrentPrice
                        }).ToList();

                    if (cars.Count == 0)
                    {
                        ViewBag.SearchOrGalleryNotFound = CarsBiddingUsingBootstrap.Classes.GalleryEmptySource.GalleryDontHaveData;
                    }
                    ViewBag.NotFoundDataMsg = CarsBiddingUsingBootstrap.Localization.NotFoundData;
                    /*
                     * [Start]
                     * we need this list in order to => after when user doing search then navigate between pages
                     * we need to keep searchedlist (the same data that searhed it by user) which when the user click
                     * on pages will go to [POST] ChangePage action method => so we want when navigate between pages after search process
                     * send searchedlist to [POST] ChangePage action method to display same searchedlist data.
                     */
                    ViewBag.searchResult = JsonConvert.SerializeObject(cars);
                    //End
                    return View(cars.ToPagedList(page ?? 1, 12));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Get] Gallery action,Gallery Controller");
                ViewBag.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                ViewBag.Msg = ex.Message;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Gallery(string CarTypetxt, string CarColortxt, int? ManufactureYeartxt, int? Pricetxt, string TypeOfTransmissionGeartxt, int? page, string SelectedSearchTypeValue)
        {
            try
            {
                /*
                 * we did this way in order to reduce number of unused if-sta
                 * which in old way for ex: select 3 type of search
                 * we should to execute may be about 6-11 if-sta to reach to what's 
                 * we want but in this way we reduce it => in same ex
                 * we should to execute may be about 1-6 if-sta
                 */
                using (CarsBiddingEntities context = new CarsBiddingEntities())
                {
                    ViewBag.TypeOfTransmissionGear = Helper.fillTypeOfTransmissionGearDropDownList();
                    ViewBag.SelectedSearchTypeValue = SelectedSearchTypeValue;
                    ViewBag.RequestType = "POST";
                    if (CarTypetxt == string.Empty)
                    {
                        /*
                         * this case wiil occured when we choose one type search(CarType)
                         * then click on searchBtn without enter any data.
                         */
                        CarTypetxt = null;
                    }
                    List<CarsInfoViewModel> cars = null;
                    List<CarsInfoViewModel> allcars = context.Cars_Info.Where(c => c.Timer_Status == true).Join(context.Biddings, car => car.CarId, bid => bid.CarId, (car, bid) => new CarsInfoViewModel()
                    {
                        CarId = car.CarId,
                        TypeOfCar = car.TypeOfCar,
                        ColorOfCar = car.ColorOfCar,
                        YearOfManufacture = car.YearOfManufacture,
                        MainPhoto = car.MainPhoto,
                        CurrentPrice = bid.CurrentPrice,
                        TypeOfTransmissionGear = car.TypeOfTransmissionGear == true ? "1" : "0",

                    }).ToList();

                    List<object> allTextInpuVariable = new List<object>()
                    {
                        CarTypetxt,
                        CarColortxt,
                        ManufactureYeartxt,
                        Pricetxt,
                        TypeOfTransmissionGeartxt
                    };
                    List<object> allTextInpuWithValue = new List<object>();
                    List<string> variableNameForInpuWithValue = new List<string>();
                    for (int i = 0; i < allTextInpuVariable.Count; i++)
                    {
                        if (allTextInpuVariable[i] != null)
                        {
                            allTextInpuWithValue.Add(allTextInpuVariable[i]);
                        }
                    }




                    switch (allTextInpuWithValue.Count)
                    {
                        case 1:
                            //1 type of search => so that mean the search type is CarType (always when the count of allTextInpuWithValue == 1 that mean thae search type is CarType)
                            cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower())).ToList();
                            break;
                        case 2:
                            //2 type of search => always one of them is CarType and the  other we should check to know what's the search type
                            if (CarColortxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower())).ToList();
                            }
                            else if (ManufactureYeartxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt).ToList();
                            }
                            else if (Pricetxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.CurrentPrice == Pricetxt).ToList();
                            }
                            else if (TypeOfTransmissionGeartxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.TypeOfTransmissionGear.ToString() == TypeOfTransmissionGeartxt).ToList();
                            }
                            break;
                        case 3:
                            //3 type of search => always one of them is CarType and the  others we should check to know what's the search type for them
                            if (CarColortxt != null && ManufactureYeartxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt).ToList();
                            }
                            else if (CarColortxt != null && Pricetxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.CurrentPrice == Pricetxt).ToList();
                            }
                            else if (CarColortxt != null && TypeOfTransmissionGeartxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.TypeOfTransmissionGear.ToString() == TypeOfTransmissionGeartxt).ToList();
                            }
                            else if (ManufactureYeartxt != null && Pricetxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.CurrentPrice == Pricetxt).ToList();
                            }
                            else if (ManufactureYeartxt != null && TypeOfTransmissionGeartxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.TypeOfTransmissionGear.ToString() == TypeOfTransmissionGeartxt).ToList();
                            }
                            else if (Pricetxt != null && TypeOfTransmissionGeartxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.CurrentPrice == Pricetxt && car.TypeOfTransmissionGear.ToString() == TypeOfTransmissionGeartxt).ToList();
                            }
                            break;
                        case 4:
                            //4 type of search => always one of them is CarType and the  others we should check to know what's the search type for them
                            if (CarColortxt != null && ManufactureYeartxt != null && Pricetxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.CurrentPrice == Pricetxt).ToList();
                            }
                            else if (CarColortxt != null && ManufactureYeartxt != null && TypeOfTransmissionGeartxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.TypeOfTransmissionGear.ToString() == TypeOfTransmissionGeartxt).ToList();
                            }
                            else if (CarColortxt != null && Pricetxt != null && TypeOfTransmissionGeartxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.CurrentPrice == Pricetxt && car.TypeOfTransmissionGear.ToString() == TypeOfTransmissionGeartxt).ToList();
                            }
                            else if (ManufactureYeartxt != null && Pricetxt != null && TypeOfTransmissionGeartxt != null)
                            {
                                cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.CurrentPrice == Pricetxt && car.TypeOfTransmissionGear.ToString() == TypeOfTransmissionGeartxt).ToList();
                            }
                            break;
                        case 5:
                            //5 type of search(all type)
                            cars = allcars.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.CurrentPrice == Pricetxt && car.TypeOfTransmissionGear.ToString() == TypeOfTransmissionGeartxt).ToList();
                            break;
                        default:
                            cars = allcars;
                            break;
                    }
                    ViewBag.NotFoundDataMsg = CarsBiddingUsingBootstrap.Localization.SearchNotFound;
                    if (cars.Count == 0)
                    {
                        ViewBag.SearchOrGalleryNotFound = CarsBiddingUsingBootstrap.Classes.GalleryEmptySource.SearchDontHaveData;
                    }
                    /*
                     * [Start]
                     * we need this list in order to => after when user doing search then navigate between pages
                     * we need to keep searchedlist (the same data that searhed it by user) which when the user click
                     * on pages will go to [POST] ChangePage action method => so we want when navigate between pages after search process
                     * send searchedlist to [POST] ChangePage action method to display same searchedlist data.
                     */
                    ViewBag.searchResult = JsonConvert.SerializeObject(cars);
                    //End
                    return View(cars.ToPagedList(page ?? 1, 12));
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[POST] Gallery action,Gallery Controller");
                ViewBag.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                ViewBag.Msg = ex.Message;
            }
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePage(int? page, string SearchedResult)
        {

            using (CarsBiddingEntities context = new CarsBiddingEntities())
            {

                List<CarsInfoViewModel> cars = null;
                try
                {
                    cars = JsonConvert.DeserializeObject<List<CarsInfoViewModel>>(SearchedResult);
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[POST] ChangePage action,Gallery Controller");
                    return Json(new string[] { "ERROR", ex.Message });
                }

                return PartialView("~/Views/Gallery/PagingCarPanel.cshtml", cars.ToPagedList(page ?? 1, 12));
            }


        }
        //public ViewResult Gallery(string CarTypetxt, string CarColortxt, int? ManufactureYeartxt, int? Pricetxt, string TypeOfTransmissionGeartxt, int? page)
        //{
        //    try
        //    {
        //        //Old Way
        //        using (CarsBiddingEntities context = new CarsBiddingEntities())
        //        {
        //            List<Cars_Info> cars = null;
        //            ViewBag.TypeOfTransmissionGear = Helper.fillTypeOfTransmissionGearDropDownList();
        //            //1 type of search
        //            if (CarTypetxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower())).ToList();
        //            }

        //            //2 type of search
        //            else if (CarTypetxt != null && CarColortxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower())).ToList();
        //            }
        //            else if (CarTypetxt != null && ManufactureYeartxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt).ToList();
        //            }
        //            else if (CarTypetxt != null && Pricetxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.InitialPrice == Pricetxt).ToList();
        //            }
        //            else if (CarTypetxt != null && TypeOfTransmissionGeartxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.TypeOfTransmissionGear == TypeOfTransmissionGeartxt).ToList();
        //            }

        //            //3 type of search
        //            else if (CarTypetxt != null && CarColortxt != null && ManufactureYeartxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt).ToList();
        //            }
        //            else if (CarTypetxt != null && CarColortxt != null && Pricetxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.InitialPrice == Pricetxt).ToList();
        //            }
        //            else if (CarTypetxt != null && CarColortxt != null && TypeOfTransmissionGeartxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.TypeOfTransmissionGear == TypeOfTransmissionGeartxt).ToList();
        //            }
        //            else if (CarTypetxt != null && ManufactureYeartxt != null && Pricetxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.InitialPrice == Pricetxt).ToList();
        //            }
        //            else if (CarTypetxt != null && ManufactureYeartxt != null && TypeOfTransmissionGeartxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.TypeOfTransmissionGear == TypeOfTransmissionGeartxt).ToList();
        //            }
        //            else if (CarTypetxt != null && Pricetxt != null && TypeOfTransmissionGeartxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.InitialPrice == Pricetxt && car.TypeOfTransmissionGear == TypeOfTransmissionGeartxt).ToList();
        //            }


        //            //4 type of search
        //            else if (CarTypetxt != null && CarColortxt != null && ManufactureYeartxt != null && Pricetxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.InitialPrice == Pricetxt).ToList();
        //            }
        //            else if (CarTypetxt != null && CarColortxt != null && ManufactureYeartxt != null && TypeOfTransmissionGeartxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.TypeOfTransmissionGear == TypeOfTransmissionGeartxt).ToList();
        //            }
        //            else if (CarTypetxt != null && CarColortxt != null && Pricetxt != null && TypeOfTransmissionGeartxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.InitialPrice == Pricetxt && car.TypeOfTransmissionGear == TypeOfTransmissionGeartxt).ToList();
        //            }
        //            else if (CarTypetxt != null && ManufactureYeartxt != null && Pricetxt != null && TypeOfTransmissionGeartxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.InitialPrice == Pricetxt && car.TypeOfTransmissionGear == TypeOfTransmissionGeartxt).ToList();
        //            }


        //            //5 type of search
        //            else if(CarTypetxt != null && CarColortxt != null && ManufactureYeartxt != null && Pricetxt != null && TypeOfTransmissionGeartxt != null)
        //            {
        //                cars = context.Cars_Info.Where(car => car.TypeOfCar.StartsWith(CarTypetxt.ToLower()) && car.ColorOfCar.StartsWith(CarColortxt.ToLower()) && car.YearOfManufacture == ManufactureYeartxt && car.InitialPrice == Pricetxt && car.TypeOfTransmissionGear == TypeOfTransmissionGeartxt).ToList();
        //            }




        //            return View(cars.ToPagedList(page ?? 1, 12));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Get] Gallery action,Gallery Controller");
        //        ViewBag.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
        //        ViewBag.Msg = ex.Message;
        //    }
        //    return View();

        //}



    }
}
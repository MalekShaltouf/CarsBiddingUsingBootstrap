using CarsBiddingUsingBootstrap.Models;
using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ViewResult Index()
        {
            using (CarsBiddingEntities context = new CarsBiddingEntities())
            {
                List<CarsInfoViewModel> cars = context.Cars_Info.Join(context.Biddings,car => car.CarId,bid => bid.CarId,(car,bid) => new CarsInfoViewModel 
                {
                    CarId = car.CarId,
                    TypeOfCar = car.TypeOfCar,
                    ColorOfCar = car.ColorOfCar,
                    YearOfManufacture = car.YearOfManufacture,
                    Timer_Status = car.Timer_Status,
                    MainPhoto = car.MainPhoto,
                    CurrentPrice = bid.CurrentPrice
                }).Where(car => car.Timer_Status == true).Take(9).ToList();
                
                return View(cars);
            }
        }

    }
}
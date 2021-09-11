using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Controllers
{
    public class CarController : Controller
    {
        [Authorize]
        // GET: Car
        public ViewResult NewCar()
        {
            CarsInfoViewModel carsInfoViewModel = new CarsInfoViewModel();
            carsInfoViewModel.TypeOfTransmissionGearMenu = Helper.fillTypeOfTransmissionGearDropDownList();
            carsInfoViewModel.CarCustomsMenu = Helper.fillCarCustomsDropDownList();
            carsInfoViewModel.CarInsuranceMenu = Helper.fillCarInsuranceDropDownList();
            carsInfoViewModel.CarLicenseMenu = Helper.fillCarLicenseDropDownList();

            return View(carsInfoViewModel);
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult NewCar(CarsInfoViewModel carsInfoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Cars_Info cars_info = new Cars_Info();
                    carsInfoViewModel.TypeOfTransmissionGearMenu = Helper.fillTypeOfTransmissionGearDropDownList();
                    carsInfoViewModel.CarCustomsMenu = Helper.fillCarCustomsDropDownList();
                    carsInfoViewModel.CarInsuranceMenu = Helper.fillCarInsuranceDropDownList();
                    carsInfoViewModel.CarLicenseMenu = Helper.fillCarLicenseDropDownList();
                    carsInfoViewModel.RequestType = "POST";
                    if (carsInfoViewModel.UploadAdditionalPhoto.Count() > 5)
                    {
                        carsInfoViewModel.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                        carsInfoViewModel.Msg = CarsBiddingUsingBootstrap.Localization.ImageNumberValidation;
                        return View(carsInfoViewModel);
                    }

                    string CarNameFolder = null;
                    string CarNameFolderPath;

                    /* Upload Single File */
                    if (Helper.IsFileImage(carsInfoViewModel.UploadMainPhoto.ContentType))
                    {
                        if (Helper.IsSizeValid(carsInfoViewModel.UploadMainPhoto.ContentLength))
                        {
                            CarNameFolder = carsInfoViewModel.TypeOfCar + DateTime.Now.ToString("YYmmssfff");
                            CarNameFolderPath = Server.MapPath(string.Format("~/CarImage/{0}/", CarNameFolder));
                            if (!Directory.Exists(CarNameFolderPath))
                            {
                                Directory.CreateDirectory(CarNameFolderPath);

                                string MainPhotoFolderPath = "~/CarImage/" + CarNameFolder + "/MainPhoto";
                                cars_info.MainPhoto = MainPhotoFolderPath + "/" + carsInfoViewModel.UploadMainPhoto.FileName;
                                MainPhotoFolderPath = Server.MapPath(MainPhotoFolderPath);
                                if (!Directory.Exists(MainPhotoFolderPath))
                                {
                                    Directory.CreateDirectory(MainPhotoFolderPath);
                                    string ImagePath = MainPhotoFolderPath + "/" + carsInfoViewModel.UploadMainPhoto.FileName;
                                    carsInfoViewModel.UploadMainPhoto.SaveAs(ImagePath);
                                }
                            }
                        }
                        else
                        {
                            carsInfoViewModel.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                            carsInfoViewModel.Msg = CarsBiddingUsingBootstrap.Localization.ImageSize;
                            return View(carsInfoViewModel);
                        }
                    }
                    else
                    {
                        carsInfoViewModel.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                        carsInfoViewModel.Msg = CarsBiddingUsingBootstrap.Localization.FileTypeValidation;
                        return View(carsInfoViewModel);
                    }
                    /* Upload Single File */

                    /*Upload Multiple File*/

                    string MultiPhotoFolderPath = "~/CarImage/" + CarNameFolder + "/MultiPhoto";
                    MultiPhotoFolderPath = Server.MapPath(MultiPhotoFolderPath);
                    string[] arr = new string[5];
                    if (!Directory.Exists(MultiPhotoFolderPath))
                    {
                        Directory.CreateDirectory(MultiPhotoFolderPath);
                    }

                    int i = 0;
                    foreach (var file in carsInfoViewModel.UploadAdditionalPhoto)
                    {
                        if (Helper.IsFileImage(file.ContentType))
                        {
                            if (Helper.IsSizeValid(file.ContentLength))
                            {
                                string ImagePath = MultiPhotoFolderPath + "/" + file.FileName;
                                arr[i] = file.FileName;
                                i++;
                                file.SaveAs(ImagePath);
                            }
                            else
                            {
                                carsInfoViewModel.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                                carsInfoViewModel.Msg = CarsBiddingUsingBootstrap.Localization.ImageSizeV2;
                                return View(carsInfoViewModel);
                            }
                        }
                        else
                        {
                            carsInfoViewModel.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                            carsInfoViewModel.Msg = CarsBiddingUsingBootstrap.Localization.FileTypeValidation;
                            return View(carsInfoViewModel);
                        }
                    }
                    /*Upload Multiple File*/
                    int UserId = int.Parse(User.Identity.Name.Split('|')[1]);

                    cars_info.Photo1 = "~/CarImage/" + CarNameFolder + "/MultiPhoto/" + arr[0];
                    cars_info.Photo2 = "~/CarImage/" + CarNameFolder + "/MultiPhoto/" + arr[1];
                    cars_info.Photo3 = "~/CarImage/" + CarNameFolder + "/MultiPhoto/" + arr[2];
                    cars_info.Photo4 = "~/CarImage/" + CarNameFolder + "/MultiPhoto/" + arr[3];
                    cars_info.Photo5 = "~/CarImage/" + CarNameFolder + "/MultiPhoto/" + arr[4];
                    cars_info.InsuranceForSale = true;
                    cars_info.TypeOfCar = carsInfoViewModel.TypeOfCar.ToLower();
                    cars_info.ColorOfCar = carsInfoViewModel.ColorOfCar.ToLower();
                    cars_info.EngineCapacity = carsInfoViewModel.EngineCapacity;
                    cars_info.YearOfManufacture = carsInfoViewModel.YearOfManufacture;
                    cars_info.InitialPrice = carsInfoViewModel.InitialPrice;
                    cars_info.CarChecking = carsInfoViewModel.CarChecking;
                    cars_info.Description = carsInfoViewModel.Description;
                    cars_info.CarInsurance = carsInfoViewModel.CarInsuranceInput;
                    cars_info.Kilometers = carsInfoViewModel.Kilometers;
                    cars_info.Create_Date = DateTime.Now;
                    cars_info.UserId = UserId;
                    cars_info.TypeOfTransmissionGear = carsInfoViewModel.TypeOfTransmissionGear == "1" ? true : false;
                    cars_info.CarCustoms = carsInfoViewModel.CarCustoms == "1" ? true : false;
                    cars_info.CarLicense = carsInfoViewModel.CarLicense == "1" ? true : false;

                   
                    /* [Start 1]
                     * we wants to store initial price in currentPrice column
                       in Bidding table as a default value
                     */
                    int CarIdForNewCar;
                    using (CarsBiddingEntities context = new CarsBiddingEntities())
                    {
                        if (context.Cars_Info.Count() > 0) // check if Cars_Info contains Cars
                        {
                            int lastCarId = context.Cars_Info.Max(c => c.CarId); // Max function return max value in CarId column
                            CarIdForNewCar = lastCarId + 1;// this line represent CarId for new Car
                        }
                        else
                        {
                            //if Cars_Info table not contain any cars So it is Carid = 1 
                            CarIdForNewCar = 1;
                        }
                        /*[start 2]*/
                        /* we wants fill column bidding table */

                        Bidding bd = new Bidding() 
                        {
                            CarId = CarIdForNewCar,
                            UserId = UserId,
                            UserType = Convert.ToInt32(BiddingUserType.CarOwner),
                            CurrentPrice = carsInfoViewModel.InitialPrice
                        };

                        /*[end 2]*/
                        /*[Start 1]*/

                        context.Cars_Info.Add(cars_info);
                        context.SaveChanges();
                        context.Biddings.Add(bd);
                        context.SaveChanges();

                        carsInfoViewModel.Type = CarsBiddingUsingBootstrap.Localization.SUCCESS;
                        carsInfoViewModel.Msg = CarsBiddingUsingBootstrap.Localization.AddNewCarSuccessfully;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Post] NewCar action,Car Controller");
                carsInfoViewModel.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                carsInfoViewModel.Msg = ex.Message;
            }
            
            return View(carsInfoViewModel);
        }

        public ActionResult CarDetails(int? Id)
        {
            CarsInfoViewModel carsInfoViewModel = new CarsInfoViewModel();
            try
            {
                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
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
                                                  where car.CarId == Id
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
                    CarModel.Auction_End_Date = Convert.ToDateTime(CarModel.Create_Date).AddDays(7);
                    /*[start]
                     * here we want to collect the image that contains path path in array in order to 
                     * loop in it to generate image-hodler for it in View.
                     */
                    List<string> ImagesPathList = new List<string>();
                    string[] imgarray = { CarModel.MainPhoto, CarModel.Photo1, CarModel.Photo2, CarModel.Photo3, CarModel.Photo4, CarModel.Photo5 };
                    for (int i = 0; i < imgarray.Length; i++)
                    {
                        if (imgarray[i] != null)
                        {
                            ImagesPathList.Add(imgarray[i]);
                        }
                    }
                    ViewBag.ImagesPathList = ImagesPathList;//this array represent number of images that uploaded from user 
                    //[End]
                    return View(CarModel);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[GET] CarDetails action,Car Controller");
                carsInfoViewModel.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                carsInfoViewModel.Msg = ex.Message;
            }
            return View(carsInfoViewModel);
        }


    }
}
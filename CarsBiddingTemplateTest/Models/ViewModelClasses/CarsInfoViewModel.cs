using CarsBiddingUsingBootstrap.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Models.ViewModelClasses
{
    public class CarsInfoViewModel : NotificationParameter
    {
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string TypeOfCar { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [RegularExpression(@"^[\d]{4}$", ErrorMessageResourceName = "ManufactureYearValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public Nullable<int> YearOfManufacture { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string EngineCapacity { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string CarChecking { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string ColorOfCar { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [Range(1, Double.PositiveInfinity, ErrorMessageResourceName = "PostiveNumberValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public Nullable<double> InitialPrice { get; set; }
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [Range(1, Double.PositiveInfinity, ErrorMessageResourceName = "PostiveNumberValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public Nullable<int> Kilometers { get; set; }

        //[Display(Name = "TypeOfTransmissionGear", ResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string TypeOfTransmissionGear { get; set; }
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string CarCustoms { get; set; }
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string CarLicense { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [RegularExpression(@"^[\d]{12}$", ErrorMessage = "Please enter valid credit card.")]
        public Nullable<double> CreditCardNumber { get; set; }
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public HttpPostedFileBase UploadMainPhoto { get; set; }
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [Display(Name = "AdditionalPhoto", ResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public IEnumerable<HttpPostedFileBase> UploadAdditionalPhoto { get; set; }
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public Nullable<int> CarInsuranceInput { get; set; }
        public int CarId { get; set; }
        public string Description { get; set; }
        public string MainPhoto { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }
        public string Photo3 { get; set; }
        public string Photo4 { get; set; }
        public string Photo5 { get; set; }
        public Nullable<bool> InsuranceForSale { get; set; }
        public Nullable<int> UserId { get; set; }
        public string CarInsurance { get; set; }
        public Nullable<double> CurrentPrice { get; set; }
        public Nullable<DateTime> Create_Date { get; set; }
        public string Auction_End_Date { get; set; }
        public Nullable<bool> Timer_Status { get; set; }
        public Nullable<int> PageNumber { get; set; }
        public string RequestType { get; set; }
        public InsuranceViewModel InsuranceViewModel { get; set; }
        public BiddingViewModel BiddingViewModel { get; set; }
        public List<SelectListItem> TypeOfTransmissionGearMenu {get;set;}
        public List<SelectListItem> CarCustomsMenu { get;set; }
        public List<SelectListItem> CarInsuranceMenu { get;set; }
        public List<SelectListItem> CarLicenseMenu { get;set; }



    }
}
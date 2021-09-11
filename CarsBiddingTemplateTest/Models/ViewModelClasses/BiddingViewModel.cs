using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Models.ViewModelClasses
{
    public class BiddingViewModel : NotificationParameter
    {
        public Nullable<int> UserId { get; set; }
        public int CarId { get; set; }
        [Remote("IsNewPriceGreaterThanCurrentPrice", "BiddingProcess",AdditionalFields = "CurrentPrice",ErrorMessageResourceName = "NewPriceValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [Range(1, Double.PositiveInfinity, ErrorMessageResourceName = "PostiveNumberValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public Nullable<int> NewPrice { get; set; }
        public Nullable<double> CurrentPrice { get; set; }
    }
}
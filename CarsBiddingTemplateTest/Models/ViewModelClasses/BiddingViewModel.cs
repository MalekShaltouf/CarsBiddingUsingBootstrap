using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarsBiddingUsingBootstrap.Models.ViewModelClasses
{
    public class BiddingViewModel : NotificationParameter
    {
        public Nullable<int> UserId { get; set; }
        public Nullable<int> CarId { get; set; }
        [Range(1, Double.PositiveInfinity, ErrorMessageResourceName = "PostiveNumberValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public Nullable<int> NewPrice { get; set; }
        public Nullable<int> CurrentPrice { get; set; }
    }
}
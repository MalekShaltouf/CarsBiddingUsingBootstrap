using CarsBiddingUsingBootstrap.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarsBiddingUsingBootstrap.Models.ViewModelClasses
{
    public class ContactViewModel : NotificationParameter
    {
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string Name { get; set; }
        [RegularExpression(@"[A-z]+[\w]*@[\w]+\.[A-z]{2,3}", ErrorMessageResourceName = "EmailValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string Email { get; set; }
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string Message { get; set; }
        public string RequestType { get; set; }

    }
}
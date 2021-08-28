using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarsBiddingUsingBootstrap.Models.ViewModelClasses
{
    public class SignInViewModel : NotificationParameter
    {
        [RegularExpression(@"([A-z]+[\w]*@[\w]+\.[A-z]{2,3}|([A-z0-9._]+|[\u0621-\u064A0-9._]+))", ErrorMessageResourceName = "UserNameEmailValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        public string UserName { get; set; }
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string RequestType { get; set; }
    }
}
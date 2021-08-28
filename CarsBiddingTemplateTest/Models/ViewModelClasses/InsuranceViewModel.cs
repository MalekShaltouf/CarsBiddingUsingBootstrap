using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarsBiddingUsingBootstrap.Models.ViewModelClasses
{
    public class InsuranceViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        [RegularExpression(@"^[\d]{12}$", ErrorMessage = "Please enter valid credit card.")]
        public string CreditCardNumber { get; set; }

        public int CarId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Models.MetaDataClasses
{
    /*
     * we replace this class with SignUpViewModel in order to 
     * applied OOP Rules
     */


    //[MetadataType(typeof(UserMetaDataType))]
    //public partial class User
    //{
    //    [NotMapped]
    //    [Display(ResourceType = typeof(CarsBiddingUsingBootstrap.Localization), Name = "RepeatPassword")]
    //    [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "CompareValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
    //    [Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
    //    [DataType(DataType.Password)]
    //    public string RepeatPassword { get; set; }
    //}
    //public class UserMetaDataType 
    //{
        //[Display(ResourceType = typeof(CarsBiddingUsingBootstrap.Localization),Name ="FirstName")]
        //[Required(ErrorMessageResourceName = "RequiredFieldValidation",ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //[RegularExpression(@"([A-z]{1,9}|[\u0621-\u064A]{1,9})", ErrorMessageResourceName = "NameValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //public string Fname { get; set; }

        //[RegularExpression(@"([A-z]{1,9}|[\u0621-\u064A]{1,9})", ErrorMessageResourceName = "NameValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //[Display(ResourceType = typeof(CarsBiddingUsingBootstrap.Localization), Name = "LastName")]
        //[Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //public string Lname { get; set; }
        /*
         * Explain the Regular Expression
         * 1-^(?=.*?[A-Za-z\u0621-\u064A])
         * A-[A-Z] => make you enter character from A-Z UpperCase
         * B-[a-z] => make you enter character from a-z lowerCase
         * C-[\u0621-\u064A] => make you enter arabic character
         * 
         * so when we put A & B & C in one []=> such as [A-Za-z\u0621-\u064A] this mean that
         * we can enter A-Z or a-z or arabic character
         * 
         * 2-(?=.*?[0-9])
         * A-[0-9] => make you enter number 0-9
         * 
         * 3-{8,} => make you enter at least 8
         * 
         * [\s]) => prevent you enter space.
         * 
         * ^(?!.*[\s])(?=.*?[A-Za-z\u0621-\u064A])(?=.*?[0-9]).{8,}$ => make you enter 
         * at least 8 (English character & number & without space) or (Arabic character & number & without space)
         */
        //[RegularExpression(@"^(?!.*[\s])(?=.*?[A-Za-z\u0621-\u064A])(?=.*?[0-9]).{8,}$",ErrorMessageResourceName = "PasswordValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //[Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }
        //[Remote("IsUserNameUnique", "Login",ErrorMessageResourceName ="UniqueFieldValidaion",ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //[RegularExpression(@"([A-z0-9._]+|[\u0621-\u064A0-9._]+)", ErrorMessageResourceName = "UserNameValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //[Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //public string UserName { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //public Nullable<System.DateTime> Birthday { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //public string Gender { get; set; }
        //[Remote("IsEmailUnique", "Login", ErrorMessageResourceName = "UniqueFieldValidaion", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //[RegularExpression(@"[A-z]+[\w]*@[\w]+\.[A-z]{2,3}", ErrorMessageResourceName = "EmailValidation",ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //[DataType(DataType.EmailAddress)]
        //[Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //public string Email { get; set; }

        //[RegularExpression(@"(07[\d]{8})", ErrorMessageResourceName = "PhoneNumberValidation",ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //[Required(ErrorMessageResourceName = "RequiredFieldValidation", ErrorMessageResourceType = typeof(CarsBiddingUsingBootstrap.Localization))]
        //public string PhoneNumber { get; set; }

    //}
}
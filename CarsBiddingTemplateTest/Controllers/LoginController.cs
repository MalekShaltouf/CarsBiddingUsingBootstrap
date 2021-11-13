using CarsBiddingUsingBootstrap.Attribute;
using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CarsBiddingUsingBootstrap.Controllers
{
    public class LoginController : CultureController
    {

        // GET: Login
        [AllowAnonymousOnly]
        public ActionResult SignUp()
        {
            SignUpViewModel signUpViewModel = new SignUpViewModel();
            signUpViewModel.GenderMenu = Helper.fillGenderDropDownList();
            ViewBag.ShowLoadScreen = true;
            return View(signUpViewModel);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ViewResult SignUp(SignUpViewModel signUpViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (CarsBiddingEntities context = new CarsBiddingEntities())
                    {
                        User userModel = new User();
                        userModel.Fname = signUpViewModel.Fname.ToLower();
                        userModel.Lname = signUpViewModel.Lname.ToLower();
                        userModel.UserName = signUpViewModel.UserName.ToLower();
                        userModel.Email = signUpViewModel.Email.ToLower();
                        userModel.Password = signUpViewModel.Password;
                        userModel.Birthday = signUpViewModel.Birthday;
                        userModel.Gender = signUpViewModel.Gender;
                        userModel.PhoneNumber = signUpViewModel.PhoneNumber;
                        context.Users.Add(userModel);
                        context.SaveChanges();

                        signUpViewModel = new SignUpViewModel();
                        signUpViewModel.Type = "SUCCESS";
                        signUpViewModel.LocalizedType = Localization.SUCCESS;
                        signUpViewModel.Msg = Localization.DefinedUserSuccessfully;
                        ModelState.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Post] Sign Up action,Login Controller");
                signUpViewModel.Type = "ERROR";
                signUpViewModel.LocalizedType = Localization.ERROR;
                signUpViewModel.Msg = ex.Message;
            }
            signUpViewModel.GenderMenu = Helper.fillGenderDropDownList();
            signUpViewModel.RequestType = "POST";
            return View(signUpViewModel);
        }
        public JsonResult IsUserNameUnique(string UserName)
        {
            using (CarsBiddingEntities context = new CarsBiddingEntities())
            {
                bool IsUserNameExists = context.Users.Any(user => user.UserName.ToLower() == UserName.ToLower());
                return Json(!IsUserNameExists, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult IsEmailUnique(string Email)
        {
            using (CarsBiddingEntities context = new CarsBiddingEntities())
            {
                bool IsEmailExists = context.Users.Any(user => user.Email.ToLower() == Email.ToLower());
                return Json(!IsEmailExists, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymousOnly]
        public ActionResult SignIn()
        {
            ViewBag.ShowLoadScreen = true;
            return View(new SignInViewModel());
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SignIn(SignInViewModel signInModel)
        {
            /*
             * Note:if i use User Model in this view then 
             * the ModelState.IsValid will return false because rest of property required 
             * such as Fname,Lname,Gender,...
             * and in this view i want just UsreName & Password
             * 
             * so for this reason i create view model consists just from Uname & Pass Property
             */
            try
            {
                if (ModelState.IsValid)
                {
                    using (CarsBiddingEntities contaxt = new CarsBiddingEntities())
                    {
                        //we want to allow to user to enter to system using UserName or Email as he wants.
                        User user = contaxt.Users.SingleOrDefault(u => u.UserName == signInModel.UserName.ToLower() && u.Password == signInModel.Password);
                        if (signInModel.UserName.Contains("@"))
                        {
                            //here means that user enter email as userName
                            user = contaxt.Users.SingleOrDefault(u => u.Email == signInModel.UserName.ToLower() && u.Password == signInModel.Password);
                        }
                        if (user != null)
                        {
                            FormsAuthentication.SetAuthCookie(user.UserName + "|" + user.UserId, true);
                            return RedirectToAction("Index", "Home");

                        }
                        signInModel = new SignInViewModel();
                        signInModel.Type = "ERROR";
                        signInModel.LocalizedType = Localization.ERROR;
                        signInModel.Msg = Localization.UserLoginValidation;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Post] Sign In action,Login Controller");
                signInModel.Type = "ERROR";
                signInModel.LocalizedType = Localization.ERROR;
                signInModel.Msg = ex.Message;
            }
            signInModel.RequestType = "POST";
            return View(signInModel);
        }
        public RedirectToRouteResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }

    }
}
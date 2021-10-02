using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Controllers
{
    public class ContactController : CultureController
    {
        // GET: Contact
        public ViewResult Contact()
        {
            return View(new ContactViewModel());
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ViewResult Contact(ContactViewModel ContactModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(ContactModel.Message);
                    sb.AppendLine();
                    sb.Append(" By: " + ContactModel.Name + " => " + ContactModel.Email);

                    MailMessage msg = new MailMessage("CarsBidding@gmail.com", "CarsBidding@gmail.com");
                    msg.Body = sb.ToString();
                    msg.Subject = "Msg From CarsBidding Users";

                    SmtpClient smclient = new SmtpClient();
                    smclient.Send(msg);

                    ModelState.Clear();
                    ContactModel = new ContactViewModel();
                    ContactModel.Type = "SUCCESS";
                    ContactModel.LocalizedType = Localization.SUCCESS;
                    ContactModel.Msg = Localization.ContactResponseMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Post] Contact action,Contact Controller");
                ContactModel.Type = "ERROR";
                ContactModel.LocalizedType = Localization.ERROR;
                ContactModel.Msg = ex.Message;
            }
            ContactModel.RequestType = "POST";
            return View(ContactModel);
        }
    }
}
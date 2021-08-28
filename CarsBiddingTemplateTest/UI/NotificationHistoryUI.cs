using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarsBiddingUsingBootstrap.UI
{
    public class NotificationHistoryUI
    {
        /*
         * we called this classas 'NotificationHistoryUI'
         * because it's represent Notification UI Component in other word
         * it's represent notification DropDownMenu contents that exists 
         * in Navbar
         * 
         * UI mean => User Interface
         */
        public string AllNotiHisAsHtmlString { get; set; }
        public int NumberOfNotOpenedNotification { get; set; }

    }
}
using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarsBiddingUsingBootstrap.Models.ViewModelClasses
{
    public class NotificationHistoryViewModel : NotificationParameter, IWinner_Owner_Car_Details
    {
        public int NotificationId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string EnglishMessage { get; set; }
        public string NativeMessage { get; set; }
        public string Eng_ArMessage { get; set; }
        public string MainPhoto { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string TimeSince { get; set; }
        public Nullable<bool> NotificationStatus { get; set; }
        public Nullable<int> NotificationType { get; set; }
        public Nullable<int> CarId { get; set; }
        public int FinalPrice { get; set; }
        public int WinnerUserId { get; set; }
        public int OwnerUserId { get; set; }
        public CarOwnerDetails carOwnerDetails { get; set; }
        public CarWinnerDetails carWinnerDetails { get; set; }
    }
}
using CarsBiddingUsingBootstrap.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarsBiddingUsingBootstrap.Models.ViewModelClasses
{
    public class NotificationHistoryViewModel : WinnerOwnerCarDetails
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
        public int WinnerUserId { get; set; }
        public int OwnerUserId { get; set; }
        public CarOwnerDetails carOwnerDetails { get; set; }
        public CarWinnerDetails carWinnerDetails { get; set; }

        public static void makeNotificationOpened(string NotificationId)
        {
            try
            {
                using (CarsBiddingEntities context = new CarsBiddingEntities())
                {
                    NotificationHistory notificationHistory = new NotificationHistory() { NotificationId = int.Parse(NotificationId), NotificationStatus = true };
                    context.NotificationHistories.Attach(notificationHistory);
                    context.Entry(notificationHistory).Property(x => x.NotificationStatus).IsModified = true;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "makeNotificationOpened function,NotificationHistoryViewModel class");
            }
        }
        public static NotificationHistory PopulateNotificationInfo(int? UserId, int CarId, string englishMsg, string nativeMsg,string Photo)
        {
            try
            {
                NotificationHistory notificationHistory = new NotificationHistory()
                {
                    CarId = CarId,
                    UserId = UserId,
                    EnglishMessage = englishMsg,
                    NativeMessage = nativeMsg,
                    NotificationStatus = false,//false mean that notification not opened yet.
                    Time = DateTime.Now,
                    MainPhoto = Photo,
                    NotificationType = Convert.ToInt32(CarsBiddingUsingBootstrap.Classes.NotificationType.CarOwnerNotification)
                };
                return notificationHistory;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "PopulateNotificationInfo function,NotificationHistoryViewModel class");
                throw ex;
            }
        }
    }
}
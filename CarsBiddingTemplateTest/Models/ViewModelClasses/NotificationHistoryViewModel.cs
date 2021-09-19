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
        public static List<NotificationHistoryViewModel> AllUserNotification { get; set; }

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

        public static void PopulateAllUserNotificationInMemory(int UserId) 
        {
            try
            {
                using (CarsBiddingEntities context = new CarsBiddingEntities())
                {
                    /*
                    * Why we are use AsEnumerable()?
                    * Entity Framework is trying to execute your projection on the SQL side,
                    * where there is no equivalent to GetTimeSince(Convert.ToDateTime(noti.Time)) or int.Parse() functions in Database.
                    * so wo we need to use AsEnumerable() to force evaluation of that part with Linq to Objects.
                    */
                    AllUserNotification = context.NotificationHistories.AsEnumerable().Where(noti => noti.UserId == UserId).Select(noti => new NotificationHistoryViewModel()
                    {
                        NotificationId = noti.NotificationId,
                        UserId = noti.UserId,
                        EnglishMessage = noti.EnglishMessage,
                        NativeMessage = noti.NativeMessage,
                        Eng_ArMessage = Localization.isRTL == "true" ? noti.NativeMessage : noti.EnglishMessage,
                        MainPhoto = noti.MainPhoto,
                        NotificationStatus = noti.NotificationStatus,
                        Time = noti.Time,
                        TimeSince = Helper.GetTimeSince(Convert.ToDateTime(noti.Time))
                    }).OrderByDescending(noti => noti.Time).ToList();
                }
            }
            catch (Exception ex) 
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[GET] PopulateAllUserNotificationInMemory function,NotificationHistoryViewModel Class");
            }
        }
    }
}
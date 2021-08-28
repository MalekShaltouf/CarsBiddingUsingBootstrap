using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using CarsBiddingUsingBootstrap.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CarsBiddingUsingBootstrap.Controllers
{
    [Authorize]
    public class NotificationHistoryController : Controller
    {
        public ActionResult GetAllUserNotification()
        {
            List<NotificationHistoryViewModel> AllNotificationHistory = new List<NotificationHistoryViewModel>();
            NotificationHistoryUI notificationHistoryUI = new NotificationHistoryUI();
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
                    AllNotificationHistory = context.NotificationHistories.AsEnumerable().Where(noti => noti.UserId == int.Parse(User.Identity.Name.Split('|').LastOrDefault())).Select(noti => new NotificationHistoryViewModel()
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
                    }).OrderBy(noti => noti.Time).ToList();

                    notificationHistoryUI.AllNotiHisAsHtmlString = Helper.RenderViewToString(this.ControllerContext, "GetAllUserNotification", AllNotificationHistory);
                    //we want to get number of Notification that not opened yet. 
                    notificationHistoryUI.NumberOfNotOpenedNotification = AllNotificationHistory == null ? 0 : AllNotificationHistory.Where(noti => noti.NotificationStatus == false).Count();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[GET] GetAllUserNotification action,Helper Controller");
            }

            return Json(notificationHistoryUI, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotificationDetails(string Id) 
        {
            //Id represent NotificationId
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<NotificationHistoryViewModel> allNotificationHistory = null;
            try
            {
                using (CarsBiddingEntities context = new CarsBiddingEntities()) 
                {
                    /*
                     * we want to return all Notification Details
                     * but we want to make the selected Notification 
                     * as a top of AllNotification so in order to do that & we want to return CurrentPrice(FinalPrice) 
                     * we will use this sql query:
                     * 'select NotHis.*,bid.CurrentPrice from NotificationHistory NotHis inner join Bidding bid on 
                     *  NotHis.CarId = bid.CarId order by (case NotificationId when 3 then 0 else 1 end),[Time] desc'
                     *  
                     *  explain to this expression => NotHis.*,bid.CurrentPrice from NotificationHistory NotHis inner join Bidding bid on 
                     *  NotHis.CarId = bid.CarId
                     *  
                     *  NotHis.* => this mean return all NotificationHistory table columns
                     *  bid.CurrentPrice = > return just CurrentPrice column from Bidding table
                     *  
                     *  
                     * explain to this expression =>  order by (case NotificationId when 3 then 0 else 1 end),[Time] desc
                     * we will sort the data based on 2 columns(NotificationId,Time) 
                     * 
                     * (case NotificationId when selectedNotificationId then 0 else 1 end) => this expression will 
                     * returns 0 if the row is the one you want or 1 if it isn't so by this expression will make the 
                     * selectedNotificationId as top of list
                     * 
                     * but also we need to order data based on another column[Time] => which we want to sort 
                     * remaining data based on Time Descending(تنازلي) 
                     * 
                     * summary:this query return all NotificationDetails with CurrentPrice(Final Price)
                     * sorted based on selected NotificationId & Time Descending 
                     * note:this query we will write it using Linq Query
                     */

                    /*
                     * Why we are use AsEnumerable()?
                     * Entity Framework is trying to execute your projection on the SQL side,
                     * where there is no equivalent to GetTimeSince(Convert.ToDateTime(noti.Time)) or int.Parse() functions in Database.
                     * so wo we need to use AsEnumerable() to force evaluation of that part with Linq to Objects.
                     */
                    allNotificationHistory = context.NotificationHistories.AsEnumerable().Join(context.Biddings,noti => noti.CarId, bid => bid.CarId,(noti,bid) => new NotificationHistoryViewModel() 
                    {
                        NotificationId = noti.NotificationId,
                        UserId = noti.UserId,
                        CarId = noti.CarId,
                        EnglishMessage = noti.EnglishMessage,
                        NativeMessage = noti.NativeMessage,
                        MainPhoto = noti.MainPhoto,
                        Time = noti.Time,
                        NotificationStatus = noti.NotificationStatus,
                        NotificationType = noti.NotificationType,
                        FinalPrice = Convert.ToInt32(bid.CurrentPrice),
                        WinnerUserId = bid.UserId
                    })
                    .OrderBy(noti => noti.NotificationId == int.Parse(Id) ? 0 : 1)
                    .ThenByDescending(noti => noti.Time).ToList();//we use ThenBy function in order to add second column that we we need it to sort data based on it

                    /*
                     * now we want to loop in every record & check if NotificationType 
                     * CarOwnerNotification or CarWinnerNotification in order return data based on 
                     * NotificationType
                     */

                    foreach (NotificationHistoryViewModel notiHis in allNotificationHistory) 
                    {
                        switch (notiHis.NotificationType) 
                        {
                            case 1:
                                /*
                                 * 1 mean => CarOwnerNotification => that mean =>  that we need to
                                 * return User Winner info for Car owner
                                 */
                                User WinnerUserInfo = context.Users.SingleOrDefault(user => user.UserId == notiHis.WinnerUserId);
                                notiHis.carWinnerDetails = new CarWinnerDetails();
                                notiHis.carWinnerDetails.WinnerName = WinnerUserInfo.Fname + " " + WinnerUserInfo.Lname;
                                notiHis.carWinnerDetails.WinnerEmail = WinnerUserInfo.Email;
                                notiHis.carWinnerDetails.WinnerPhoneNumber = WinnerUserInfo.PhoneNumber;
                                break;
                            case 2:
                                /*
                                 * 2 mean => CarWinnerNotification => that mean =>  that we need to
                                 * return Car Owner info for User that win the car
                                 */

                                //step1 we want to know the car owner =>
                                notiHis.OwnerUserId = Convert.ToInt32(context.Cars_Info.SingleOrDefault(car => car.CarId == notiHis.CarId).UserId);
                                User CarOwnerInfo = context.Users.SingleOrDefault(user => user.UserId == notiHis.OwnerUserId);
                                notiHis.carOwnerDetails = new CarOwnerDetails();
                                notiHis.carOwnerDetails.OwnerName = CarOwnerInfo.Fname + " " + CarOwnerInfo.Lname;
                                notiHis.carOwnerDetails.OwnerEmail = CarOwnerInfo.Email;
                                notiHis.carOwnerDetails.OwnerPhoneNumber = CarOwnerInfo.PhoneNumber;
                                break;
                        }

                    }
                }
            }
            catch (Exception ex) 
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Post] NewCar action,Car Controller");
                //carsInfoViewModel.Type = CarsBiddingUsingBootstrap.Localization.ERROR;
                //carsInfoViewModel.Msg = ex.Message;
            }
            

            return View(allNotificationHistory);
        }
    }
}
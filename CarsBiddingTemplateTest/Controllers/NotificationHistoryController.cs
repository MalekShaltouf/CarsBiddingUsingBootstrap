using CarsBiddingUsingBootstrap.Classes;
using CarsBiddingUsingBootstrap.Models;
using CarsBiddingUsingBootstrap.Models.ViewModelClasses;
using CarsBiddingUsingBootstrap.UI;
using PagedList;
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
            List<NotificationHistoryViewModel> AllNotificationHistory = null;
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
                    }).OrderByDescending(noti => noti.Time).ToList();

                    notificationHistoryUI.AllNotiHisAsHtmlString = Helper.RenderViewToString(this.ControllerContext, "GetAllUserNotification", AllNotificationHistory);
                    //we want to get number of Notification that not opened yet. 
                    notificationHistoryUI.NumberOfNotOpenedNotification = AllNotificationHistory == null ? 0 : AllNotificationHistory.Where(noti => noti.NotificationStatus == false).Count();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[GET] GetAllUserNotification action,NotificationHistory Controller");
            }

            return Json(notificationHistoryUI, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotificationDetails(string Id, int? page,int? NotificationSatus) 
        {
            //Id represent NotificationId
            if (Id == null || NotificationSatus == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<NotificationHistoryViewModel> allNotificationHistory = null;
            List<NotificationHistory> allNotificationHistoryTableRows = null;
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
                    allNotificationHistoryTableRows = context.NotificationHistories.AsEnumerable().ToList();

                    /*
                     * as a step 1 => we want to check if NotificationId exists in 
                     * Database or not => becuase suppose that user open directly NotificationDetails Page 
                     * with not exists NotificationId such as: NotificationHistory/NotificationDetails/100000
                     * which there are not NotificationId = 100000 so in this case we want to prevent user to
                     * open the page
                     */
                    bool isNotificationIdExists = allNotificationHistoryTableRows.Any(noti => noti.NotificationId == int.Parse(Id));
                    if (!isNotificationIdExists) 
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    }

                    /*
                     * step2:we want to make notification as a opened Notification
                     */
                    if (NotificationSatus == Convert.ToInt32(NotificationStatus.NotificationNotOpenYet)) 
                    {
                        NotificationHistoryViewModel.makeNotificationOpened(Id);
                    }
                    /*
                     * we want to return all Notification Details for user that login to system
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

                    int UserId = int.Parse(User.Identity.Name.Split('|').LastOrDefault());//this represent UserId for user that login to system
                    allNotificationHistory = allNotificationHistoryTableRows.Join(context.Biddings,noti => noti.CarId, bid => bid.CarId,(noti,bid) => new NotificationHistoryViewModel() 
                    {
                        NotificationId = noti.NotificationId,
                        UserId = noti.UserId,
                        CarId = noti.CarId,
                        EnglishMessage = noti.EnglishMessage,
                        NativeMessage = noti.NativeMessage,
                        Eng_ArMessage = Localization.isRTL == "true" ? noti.NativeMessage : noti.EnglishMessage,
                        MainPhoto = noti.MainPhoto,
                        Time = noti.Time,
                        NotificationStatus = noti.NotificationStatus,
                        NotificationType = noti.NotificationType,
                        FinalPrice = Convert.ToInt32(bid.CurrentPrice),
                        WinnerUserId = bid.UserId
                    })
                    .Where(item => item.UserId == UserId)
                    .OrderBy(noti => noti.NotificationId == int.Parse(Id) ? 0 : 1)
                    .ThenByDescending(noti => noti.Time).ToList();//we use ThenBy function in order to add second column that we we need it to sort data based on it
                }
            }
            catch (Exception ex) 
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Get] NotificationDetails action,NotificationHistory Controller");
            }
            return View(allNotificationHistory.ToPagedList(page ?? 1, 12));
        }
        public PartialViewResult GetCarWinnerInfo(int WinnerUserId) 
        {
            CarWinnerDetails carWinnerDetails  = new CarWinnerDetails();
            try
            {
                using (CarsBiddingEntities context = new CarsBiddingEntities()) 
                {
                    User WinnerUserInfo = context.Users.SingleOrDefault(user => user.UserId == WinnerUserId);
                    carWinnerDetails.WinnerName = WinnerUserInfo.Fname + " " + WinnerUserInfo.Lname;
                    carWinnerDetails.WinnerEmail = WinnerUserInfo.Email;
                    carWinnerDetails.WinnerPhoneNumber = WinnerUserInfo.PhoneNumber;
                }
            }
            catch (Exception ex) 
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Get] GetCarWinnerInfo action,NotificationHistory Controller");
            }
            return PartialView("~/Views/NotificationHistory/CarWinnerInfo.cshtml", carWinnerDetails);
        }
        public PartialViewResult GetCarOwnerInfo(int CarId) 
        {
            CarOwnerDetails carOwnerDetails = new CarOwnerDetails();
            try
            {
                using (CarsBiddingEntities context = new CarsBiddingEntities()) 
                {
                    //step1 we want to know the car owner =>
                    int OwnerUserId = Convert.ToInt32(context.Cars_Info.SingleOrDefault(car => car.CarId == CarId).UserId);
                    //step2 return Owner Info
                    User CarOwnerInfo = context.Users.SingleOrDefault(user => user.UserId == OwnerUserId);
                    carOwnerDetails.OwnerName = CarOwnerInfo.Fname + " " + CarOwnerInfo.Lname;
                    carOwnerDetails.OwnerEmail = CarOwnerInfo.Email;
                    carOwnerDetails.OwnerPhoneNumber = CarOwnerInfo.PhoneNumber;
                }
            }
            catch (Exception ex) 
            {
                ErrorLog.WriteInLog(ex.Message, ex.StackTrace, "[Get] GetCarOwnerInfo action,NotificationHistory Controller");
            }
            return PartialView("~/Views/NotificationHistory/CarOwnerInfo.cshtml", carOwnerDetails);
        }
    }
}
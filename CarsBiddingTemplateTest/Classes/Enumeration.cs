using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarsBiddingUsingBootstrap.Classes
{
    public enum GalleryEmptySource 
    {
        GalleryDontHaveData = 0,
        SearchDontHaveData = 1//mean that after did search ther no data
    }
    public enum IsAuthenticated 
    {
        NotAuthenticated = 0,
        Authenticated = 1
    }
    public enum NotificationType 
    {
        CarOwnerNotification = 1,
        CarWinnerNotification = 2
    }

    public enum NotificationStatus
    {
        NotificationNotOpenYet = 0,
        NotificationOpened = 1,
    }
}
function HowLinkEvent(ModelContent) {

    addNotification("INFO", null, ModelContent);
    document.querySelector(".gallery_pics").onclick = function (e) {
        if (e.target.tagName == "IMG") {

            let CloseIcon = document.createElement("span");
            CloseIcon.setAttribute("Id", "CloseIcon");

            CloseIcon.innerHTML = "&times;";
            document.querySelector(".gallery_pics").insertBefore(CloseIcon, document.querySelector(".gallery_pics img"));

            document.querySelector("#NotificationPopUp .modal-dialog").classList.add("modal-xl");
            this.classList.add("fullscreen");

            //this refer to $(.gallery_pics) element
        }
        else if (e.target.tagName == "SPAN") {
            $("#CloseIcon").parent().removeClass("fullscreen");
            document.querySelector("#NotificationPopUp .modal-dialog").classList.remove("modal-xl");
            document.getElementById("CloseIcon").remove();
        }
    }
}
function NewCarformValidation(msgNotification) {
    let additionalPhoto = document.querySelector(".additionalPhoto .file-upload-input").getAttribute("title");

    if (additionalPhoto != null) {
        if (additionalPhoto.split(',').length > 5) {
            addNotification("ERROR", msgNotification);
            return false;
        }
    }
    return true;
}

function addNotEmptyClass()
{
        /*
        * when the bootstrap modal show the lable become over of textbox value
        * so in order to solve that we want to add 'not-empty' class to
        * textbox that contains value which the 'not-empty' class will make lable
        * above of textbox
        */
        $('.form-control').each((i, e) => {
            if (e.value != '') {
                $(".form-group").eq(i).addClass("not-empty");
            }
        });
}

function GetAllUserNotification() {
    $.ajax({
        url: '/NotificationHistory/GetAllUserNotification',
        type: 'GET',
        success: function (response) {
            //the response represent notificationHistoryUI object

            //step1:fill Notification in navbar <ul>
            $("#user-notification").html(response.AllNotiHisAsHtmlString);

            //step2:fill number of notification that not opened yet.
            if (response.NumberOfNotOpenedNotification == 0) {
                //if then NumberOfNotOpenedNotification == 0 we will hide the NumberOfNotification red circle
                $('.bell').addClass("remove-bell-before-pseudo-element");
            } else
            {
                
                $('.bell').addClass("bell-before-content");
                $('.bell').attr('data-before-selector', response.NumberOfNotOpenedNotification);
            }
        }
    });
}
$(function () {

    $('.view a').Chocolat();//enable image slider
    $('[data-toggle="tooltip"]').tooltip();//enable tooltip that exists in PurchaseInsurance & Bidding Modal(popUp)
    isTimerEnd();
    isUserCarOwner();
    $('#PurchaseInsurance').on('shown.bs.modal', function (e) {
        /*
         * here we will open collapse after one second from open
         * bootstrap Modal.
         */
        setTimeout(
            function () {
                $("#InsuranceDetails").addClass("in");
                $("#InsuranceDetails").removeAttr("style");
                $("#InsuranceDetails").attr("aria-expanded", "true");
            }, 1000);
    });

    /*[start]
    * here we use onsubmit jquery event,not the onsubmit pure js
    * because the onsubmit pure js not working correctly when you
    * using jquery.unobtrusive-ajax.min.js script
    */
    $("#PurchaseInsuranceForm").on("submit", function (e) {
        if ($("#PurchaseInsuranceForm").valid()) {
            /*
             * when click on confirmBtn & the
             * FormValidation was valid so we want to close
             * PurchaseInsuranceModal
             */
            $('#PurchaseInsurance').modal('hide');
        }
    });

    $("#PurchaseInsurance #PurchaseCloseBtn").click(function () {
        if ($("#InsuranceDetails").hasClass("in")) {
            /*
             * here when click on close btn we delete
             * "in" class from collapse => in order to if user
             * re-again open the popup => the collapse open after 1 second
             * if we didn't remove class => then re-again open popup =? you will
             * note that collapse already open
             */
            $("#InsuranceDetails").removeClass("in");
        }
    });
    $("#BiddingProcessForm").on("submit", function (e) {
        if ($("#BiddingProcessForm").valid()) {
            /*
             * when click on confirmBtn & the
             * FormValidation was valid so we want to close
             * BiddingProcessModal
             */
            $('#BiddingProcess').modal('hide');
        }
    });
    $(".CarDetails").on("click", "#biddingBtn", function () {
        /*
         * here we want to check if user Authoirze or not
         *
         * Authoirze mean => is user login in or not
         *
         * if user Authoirze so we will allow to him to
         * access to PurchaseInsurance page(popup)
         */
        $.ajax({
            url: "/Helper/IsUserAuthorize",
            type: "GET",
            success: function (response) {
                if (response == true) {
                    $(document).ready(function () {

                        /*[start]
                         * when open Modal as first time then write something
                         * in CreditCard Input then close Modal then re-again
                         * open Modal will note that CreditCard label open
                         * so we want to remove "not-empty"class every time
                         * before open Modal
                         */
                        $(".PageModal .modal-body .form-group").removeClass("not-empty");
                        //[End]

                        /*[start]
                         * here we want every time open Model(bootstrap PopUp)
                         * cleaer CreditCard Input & CreditCard MsgError
                         */
                        $(".PageModal .form-control").val('');
                        $(".PageModal #InputMsgError").empty();
                        //[End]

                        $.ajax({
                            url: '/BiddingProcess/IsInsurancePaid',
                            type: 'GET',
                            data: { carId: document.getElementById("CarId").value },
                            success: function (response) {
                                if (response == true) {
                                    //here will open BiddingProcess Modal
                                    /*
                                     * we want to get latest Current Price
                                     * in order display it in BiddingProcess Bootstrap Modal
                                     */
                                    $.ajax({
                                        url: '/BiddingProcess/LatestPrice',
                                        type: 'GET',
                                        data: { carId: document.getElementById("CarId").value },
                                        success: function (response) {
                                            document.getElementById("CurrentPrice").value = response;
                                            document.getElementById("currentPriceHeader").textContent = `${response}${document.querySelector("#BiddingProcessLocalization").getAttribute("data-jd")}`;
                                        }
                                    });
                                    document.getElementById("NewPriceValidation").textContent = '';
                                    /*[start]
                                    * this code open BiddingProcess Bootstrap Modal
                                    */
                                    $('#BiddingProcess').modal({
                                        backdrop: 'static',
                                        keyboard: false
                                    });
                                    //[End]
                                }
                                else {
                                    //here will open PurchaseInsurance Modal
                                    /*[start]
                                    * this code open PurchaseInsurance Bootstrap Modal
                                    */
                                    $('#PurchaseInsurance').modal({
                                        backdrop: 'static',
                                        keyboard: false
                                    });
                                    //[End]
                                }
                            }
                        });
                    });
                }
                else {
                    addNotification("ERROR", document.querySelector('#BiddingProcessErrorLocalization').getAttribute("data-bidding-process-msg"), document.getElementById("ErrorLocalized").textContent);
                }
            }
        });
    });
    $("#BiddingProcess").keyup(function (e) {
        if (e.keyCode == 8 && document.getElementById("NewPrice").value == '') {
            //e.keyCode == 8 mean backspace(<- btn)
            document.getElementById("NewPriceValidation").textContent = '';
        }
    });
    let endDate = document.getElementById("Auction_End_Date").value;
    $("#timer").countdown(endDate, function (event) {
        $("#days").text(
            event.strftime('%D')
        );
        $("#hours").text(
            event.strftime('%H')
        );
        $("#minutes").text(
            event.strftime('%M')
        );
        $("#seconds").text(
            event.strftime('%S')
        );
    });
    $('#timer').countdown(endDate).on('finish.countdown', function (event) {
        //step1:we want to remove BiddingProcess Btn and display warning alert
        $("#biddingBtn").remove();
       
     
        //step2:we want to make Timer_Status flag false
        $.ajax({
            url: '/EndTimerOperations/EndTimerOperations',
            type: 'GET',
            data: { CarId: $("#CarId").val() },
            success: function (response)
            {
                if (response.Type == "ERROR") {
                    addNotification("ERROR", response.Msg, document.getElementById("ErrorLocalized").textContent);
                } 
                else
                {
                    /*
                     * we want to send notification to Owner Car & Buyer winning car
                     * case 1 => when Car Sold Successfully we will do:
                     * A- send Notification for onwer car that car sold Successfully and send last price
                     * & buyer info
                     *
                     * B- send Notification for Buyer winning that he is won & owner car info
                     *
                     * case 2 => if car not sold successfully (The car did not receive any bid)
                     * will send notification for onwer car that Failed to sell the car
                     *
                     * case 3 => when the wining buyer cancel the sell Process
                     *
                     * case4 => when the owner car cancel sell Process after become there are wininng buyer
                     *
                     * case 3 & case 4 will be handing in future
                     * 
                     * case 5 => after any user increase car price and there are previous user
                     * also increase on car so we want to send to previous user(UserA)
                     * notification that car price has been increased => we handled it in 
                     * BiddingProcess Controller(in BiddingProcess [POST] action)
                     */
                    generateNotification();
                }
            }
        });
    });
});
function generateNotification()
{
    $.ajax({
        url: '/EndTimerOperations/GenerateNotification',
        type: 'GET',
        data: { CarId: $("#CarId").val()},
        success: function (response)
        {
            debugger;
            let Type = null,
                Msg = null,
                localizedType = null;
            if (response.Type == "ERROR") {
                Type = "ERROR";
                localizedType = response.localizedType;
                Msg = response.Msg;
                
            }
            else
            {
                Type = "WARNING";
                localizedType = response.localizedType;
                Msg = response.Msg;
            }
            addNotification(Type, Msg, localizedType);
        }
    });
}
function isTimerEnd()
{
    $.ajax({
        url: '/EndTimerOperations/isTimerEnd',
        type: 'GET',
        data: { CarId: $("#CarId").val() },
        success: function (response) {
            if (response)
            {
                addNotification("WARNING", $("#TimerStatusLocalization").data("timer-status-localization"), $("#WarningLocalized").text());
                $("#biddingBtn").remove();
            }
        }
    });
}
function BiddingOnSuccess(response) {
    addNotification(response.Type, response.Msg, response.LocalizedType);
}
function isUserCarOwner()
{
    /*
     * here we want to check if User that open CarDetails Page
     * is same user that own the car or not
     * 
     * if yes => we want to remove BiddingProcess btn => because 
     * we don't want to make car owner to prticipate in bidding process
     * on his own car
     * 
     * to execute this check the user should be login
     */
    $.ajax({
        url: "/Helper/IsUserAuthorize",
        type: "GET",
        success: function (response)
        {
            if (response)
            {
                /*
                 * response == true => that mean that user Authenticated so
                 * in this case will execute the check
                 */
                $.ajax({
                    url: "/Helper/IsUserCarOwner",
                    data: { CarId: document.getElementById("CarId").value},
                    type: "GET",
                    success: function (response) {
                        if (response)
                        {
                            /*
                             * response == true => that mean that user that open CarDetails
                             * is same CarOwnerUser so we will remove BiddingProcess Btn
                             */
                            $("#biddingBtn").remove();
                        }
                    }
                });
            }
        }
    });
}
function PurchaseOnSuccess(response) {
    if (response.Type == "SUCCESS") {
        $("#NotificationPopUp #CloseBtn").one("click", function ()
        {
            //why we use $("#NotificationPopUp #CloseBtn").one("click") event?? you will the explain at bottom of page
           /*
           * we want to get latest Current Price
           * in order display it in BiddingProcess Bootstrap Modal
           */
            $.ajax({
                url: '/BiddingProcess/LatestPrice',
                type: 'GET',
                data: { carId: document.getElementById("CarId").value },
                success: function (response) {
                    document.getElementById("CurrentPrice").value = response;
                    document.getElementById("currentPriceHeader").textContent = `${response}${document.querySelector("#BiddingProcessLocalization").getAttribute("data-jd")}`;
                }
            });
            /*[start]
            * this code open BiddingProcess Bootstrap Modal
            */
            $('#BiddingProcess').modal({
                backdrop: 'static',
                keyboard: false
            });
            //[End]
        });
    }
    addNotification(response.Type, response.Msg, response.LocalizedType);
}
let type = document.querySelector("#NotificationParameterLocalization").getAttribute("data-type"),
    msg = document.querySelector("#NotificationParameterLocalization").getAttribute("data-msg");
if (type == "ERROR") {
    addNotification(type, msg, document.getElementById("ErrorLocalized").textContent);
}


/*
 * why we use $("#NotificationPopUp #CloseBtn").one("click") event??
 * 
 * as a first $("element").one("click") => this event occured just one time
 * we use it because we want to execute this event just one time(just when 
 * PurchaseOnSuccess return  response.Type = "SUCCESS") so if we use
 * normal click event such as $("#NotificationPopUp #CloseBtn").click("click") => so 
 * yes after PurchaseOnSuccess return  response.Type = "SUCCESS" will execute the click event
 * but for every time we click on $("#NotificationPopUp #CloseBtn") not just one time
 * for example after that will show Warning popup => so when click on close 
 * will execute click event again although we don't want that so for that reason we use 
 * $("#NotificationPopUp #CloseBtn").one("click") event
 * 
 * summary we want to execute the code that exists in $("#NotificationPopUp #CloseBtn").one("click") 
 * function for one time so for that reason we used it
 */
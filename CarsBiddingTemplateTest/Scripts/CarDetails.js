$(function () {

    $('.view a').Chocolat();//enable image slider
    $('[data-toggle="tooltip"]').tooltip();//enable tooltip that exists in PurchaseInsurance & Bidding Modal(popUp)
    isTimerEnd();

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
        debugger;
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
                    addNotification("ERROR", document.querySelector('#BiddingProcessErrorLocalization').getAttribute("data-bidding-process-msg"));
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
                    addNotification("ERROR", response.Msg);
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
            let Type = null,
                Msg = null;
            if (response.Type == "ERROR") {
                Type = "ERROR";
                Msg = response.Msg;
            }
            else
            {
                Type = "WARNING";
                Msg = $("#TimerStatusLocalization").data("timer-status-localization");
            }
            addNotification(Type, Msg);
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
                addNotification("WARNING", $("#TimerStatusLocalization").data("timer-status-localization"));
                $("#biddingBtn").remove();
            }
        }
    });
}
function BiddingOnSuccess(response) {
    addNotification(response.Type, response.Msg);
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
    addNotification(response.Type, response.Msg);
}
let type = document.querySelector("#NotificationParameterLocalization").getAttribute("data-type"),
    msg = document.querySelector("#NotificationParameterLocalization").getAttribute("data-msg");
if (type == "ERROR") {
    addNotification(type, msg);
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
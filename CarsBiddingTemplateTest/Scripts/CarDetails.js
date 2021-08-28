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
        addNotification("WARNING", $("#TimerStatusLocalization").data("timer-status-localization"));
     
        //step2:we want to make Timer_Status flag false
        $.ajax({
            url: '/EndTimerOperations/EndTimerOperations',
            type: 'GET',
            data: { CarId: $("#CarId").val() },
            success: function (response)
            {
            }
        });

        
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

        //case1 => A
        generateNotificationForOwner();
    });
});
function generateNotificationForOwner()
{
    $.ajax({
        url: '/EndTimerOperations/GenerateNotificationForOwner',
        type: 'GET',
        data: { CarId: $("#CarId").val() },
        success: function (response)
        {

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
            debugger;
            if (response)
            {
                addNotification("WARNING", $("#TimerStatusLocalization").data("timer-status-localization"));
                $("#biddingBtn").remove();
            }
        }
    });
}
function BiddingOnSuccess(response) {
    if (response.Type == "SUCCESS") {
        document.getElementById("currentPriceHeader").textContent = response.NewPrice;
    }
    document.getElementById("NewPrice").value = '';
    document.querySelector("#BiddingProcess .modal-body .form-group").classList.remove("not-empty");
    addNotification(response.Type, response.Msg);
}

function PurchaseOnSuccess(response) {
    if (response.Type == "SUCCESS") {
        document.querySelector("#NotificationPopUp #CloseBtn").onclick = function () {
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
        }
    }
    addNotification(response.Type, response.Msg);
}
let type = document.querySelector("#NotificationParameterLocalization").getAttribute("data-type"),
    msg = document.querySelector("#NotificationParameterLocalization").getAttribute("data-msg");
if (type == "ERROR") {
    addNotification(type, msg);
}
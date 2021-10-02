function addNotification(type, msg = null, localizedType, info = null) {
    switch (type)
    {
        case "SUCCESS":
            document.getElementById("Notificationheader").style.backgroundColor = "#c1e2b3";
            document.getElementById("Notificationheader").style.color = "#155724";
            document.getElementById("NotificationBodyHead").style.color = "#155724";

            document.getElementById("NotificationBodyHead").innerHTML = msg;
            break;
        case "ERROR":
            document.getElementById("Notificationheader").style.backgroundColor = "#f2dede";
            document.getElementById("Notificationheader").style.color = "#a94442";
            document.getElementById("NotificationBodyHead").style.color = "#a94442";

            document.getElementById("NotificationBodyHead").innerHTML = msg;
            break;
        case "WARNING":
            document.getElementById("Notificationheader").style.backgroundColor = "#fff3cd";
            document.getElementById("Notificationheader").style.color = "#664d03";
            document.getElementById("NotificationBodyHead").style.color = "#664d03";

            document.getElementById("NotificationBodyHead").innerHTML = msg;
            break;
        default:
            //type == "INFO" => ex on it => when click on How to Add New Car? link
            document.getElementById("Notificationheader").style.backgroundColor = "#17a2b8";
            document.getElementById("Notificationheader").style.color = "#fff";

            let imgElement = document.createElement("img"),
                InfoModalDiv = document.createElement("div"),
                collapseBodyDiv = InfoModalDiv.cloneNode(),
                cardDiv = InfoModalDiv.cloneNode(),
                anchorLinkElement = document.createElement("a"),
                spanOne = document.createElement("span"),
                spanTwo = spanOne.cloneNode(),
                spanThree = spanOne.cloneNode(),
                spanFour = spanOne.cloneNode(),
                spanFive = spanOne.cloneNode(),
                spanSix = spanOne.cloneNode(),
                spanSeven = spanOne.cloneNode(),
                spanEight = spanOne.cloneNode(),
                spanNine = spanOne.cloneNode(),
                spanTen = spanOne.cloneNode(),
                spanOneText = document.createTextNode(info.split('|')[0]),
                spanTwoText = document.createTextNode(info.split('|')[1]),
                anchorLinkText = document.createTextNode(info.split('|')[2]),
                spanThreeText = document.createTextNode(info.split('|')[3]),
                spanFourText = document.createTextNode(info.split('|')[4]),
                spanFiveText = document.createTextNode(info.split('|')[5]),
                spanSixText = document.createTextNode(info.split('|')[6]),
                spanSevenText = document.createTextNode(info.split('|')[7]),
                spanEightText = document.createTextNode(info.split('|')[8]),
                spanNineText = document.createTextNode(info.split('|')[9]),
                spanTenText = document.createTextNode(info.split('|')[10]),
                spanarr = [spanThree, spanFour, spanFive, spanSix, spanSeven];
            galleryPicsHolderDiv = document.createElement("div"),
                galleryPicsDiv = galleryPicsHolderDiv.cloneNode();

            InfoModalDiv.setAttribute("id", "InfoModal");
            spanOne.setAttribute("class", "bold-element");
            spanEight.setAttribute("class", "bold-element");
            spanEight.setAttribute("style", "display:inline-block;margin-top:15px");
            spanNine.setAttribute("style", "display:inline-block;margin-top:15px");
            spanTen.setAttribute("class", "bold-element block-element");
            spanTen.setAttribute("style", "margin-top:15px");



            for (index in spanarr) {
                spanarr[index].setAttribute("class", "text-gray block-element");
            }

            anchorLinkElement.setAttribute("data-toggle", "collapse");
            anchorLinkElement.setAttribute("href", "#About50JdDetails");
            anchorLinkElement.setAttribute("role", "button");
            anchorLinkElement.setAttribute("aria-expanded", "false");
            anchorLinkElement.setAttribute("aria-controls", "About50JdDetails");
            anchorLinkElement.setAttribute("style", "margin-left:164px;display:inline-block");


            collapseBodyDiv.setAttribute("class", "collapse");
            collapseBodyDiv.setAttribute("id", "About50JdDetails");

            cardDiv.setAttribute("class", "card");

            spanOne.appendChild(spanOneText);
            spanTwo.appendChild(spanTwoText);
            spanThree.appendChild(spanThreeText);
            spanFour.appendChild(spanFourText);
            spanFive.appendChild(spanFiveText);
            spanSix.appendChild(spanSixText);
            spanSeven.appendChild(spanSevenText);
            spanEight.appendChild(spanEightText);
            spanNine.appendChild(spanNineText);
            spanTen.appendChild(spanTenText);



            cardDiv.appendChild(spanThree);
            cardDiv.appendChild(spanFour);
            cardDiv.appendChild(spanFive);
            cardDiv.appendChild(spanSix);
            cardDiv.appendChild(spanSeven);

            collapseBodyDiv.appendChild(cardDiv);

            anchorLinkElement.appendChild(anchorLinkText);

            spanTwo.appendChild(anchorLinkElement);


            InfoModalDiv.appendChild(spanOne);
            InfoModalDiv.appendChild(spanTwo);
            InfoModalDiv.appendChild(collapseBodyDiv);
            InfoModalDiv.appendChild(spanEight);
            InfoModalDiv.appendChild(spanNine);
            InfoModalDiv.appendChild(spanTen);




            imgElement.setAttribute("src", "/images/AddNewCarEx.png");
            imgElement.setAttribute("alt", "AddNewCarExample Photo");
            imgElement.setAttribute("class", "img-responsive");

            galleryPicsDiv.setAttribute("class", "gallery_pics scrollStyle");
            galleryPicsHolderDiv.setAttribute("class", "gallery_pics_holder");



            galleryPicsDiv.appendChild(imgElement);
            galleryPicsHolderDiv.appendChild(galleryPicsDiv);
            InfoModalDiv.appendChild(galleryPicsHolderDiv);

            document.getElementById("NotificationBody").appendChild(InfoModalDiv);
            break;
    }
    
    document.getElementById("NotificationPopUpTitle").textContent = localizedType;


    $(document).ready(function () {
        $('#NotificationPopUp').modal({
            backdrop: 'static',
            keyboard: false
        })
    });
}
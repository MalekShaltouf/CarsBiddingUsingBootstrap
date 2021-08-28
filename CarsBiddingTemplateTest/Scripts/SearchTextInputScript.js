$(function () {

    $('.SearchType').change(function () {
        let selectedSearchType = $(this).val(),
            SearchTextBoxOnScreenRow = document.getElementById("SearchTextBoxOnScreen"),
            MainSearch = document.getElementById("MainSearch").cloneNode(true),
            MainSearchOnScreen = document.getElementById("MainSearchOnScreen"),
            SearchType = document.getElementById("SearchType"),
            SearchBtn = document.getElementById("SearchBtn").cloneNode(true),
            SelectedsearchTypeName = $(".SearchType :selected");//here we get selectedSearchType(in same time also represnt class name for Concerned Search Textbox)

        if (document.getElementById("ErrorMsg") != null) {
            document.getElementById("ErrorMsg").remove();
        }

        SearchType.removeAttribute('class');
        SearchType.classList.add("col-md-2");
        SearchBtn.classList.add("col-md-1");

        $(".Main-Gallery .form-group > label").css("font-size", "10px");//here we do reset font-siz for lable

        if (selectedSearchType == null) {

            let MainSearchOnBehindScenes = document.getElementById("MainSearchOnBehindScenes").cloneNode(true);
            MainSearchOnBehindScenes.removeAttribute('id');
            MainSearchOnBehindScenes.setAttribute("id", "MainSearchOnScreen");

            SearchTextBoxOnScreenRow.innerHTML = '';

            if (!SearchTextBoxOnScreenRow.classList.contains("formInputs")) {
                SearchTextBoxOnScreenRow.classList.add("formInputs");
            }
            MainSearchOnBehindScenes.querySelector(".form-control").setAttribute("name", "CarTypetxt");
            SearchTextBoxOnScreenRow.appendChild(MainSearchOnBehindScenes);
            SearchTextBoxOnScreenRow.appendChild(SearchType);
        }
        else {
            MainSearch.querySelector(".form-control").setAttribute("name", "CarTypetxt");
            if (selectedSearchType.length == 1) {
                /*
                 * note => when get Element we use cloneNode() => because we just want to take
                 * copy from element when will use insertBefore() or appendChild() which if we
                 * don't did it => when use insertBefore() or appendChild() function the element
                 * will remove from SearchTextBoxBehindScenes and we don't want that.
                 */
                let concernedSearchTextBox = document.getElementById(SelectedsearchTypeName.data("search-type-name")).cloneNode(true);

                if (selectedSearchType.includes("4")) {
                    /*
                     * 4 mean if user choose  "Type Of Transmission Gear" as a search type
                     * so we want to increase SerachType DDL Width
                     */
                    SearchType.removeAttribute('class');
                    SearchType.classList.add("col-md-3");
                }

                //this code just for case1 when click on first type from SearchType DDL
                if (MainSearchOnScreen != null) {
                    MainSearchOnScreen.remove();
                }

                /*
                 * this code just for case2 when selected 2 type from SearchType DDL then
                 * unclick on one of them => so the selected type from SearchType DDL 
                 * will become one => in this case we use this code
                 */
                let allTempSearchTextBox = document.querySelectorAll(".tempSearchTextBox");
                for (let i = 0; i < allTempSearchTextBox.length; i++) {
                    allTempSearchTextBox[i].remove();
                }

                concernedSearchTextBox.querySelector(".form-control").setAttribute("name", `${SelectedsearchTypeName.data("search-type-name")}txt`);

                MainSearch.classList.add("col-md-4");
                MainSearch.classList.add("tempSearchTextBox");
                concernedSearchTextBox.classList.add("col-md-4");
                concernedSearchTextBox.classList.add("tempSearchTextBox");
                SearchBtn.classList.add("tempSearchTextBox");

                /*
                 * this code just for case2 when selected 2 type from SearchType DDL then
                 * unclick on one of them => so the selected type from SearchType DDL 
                 * will become one => in this case we use this code
                 */
                if (!SearchTextBoxOnScreenRow.classList.contains("formInputs")) {
                    SearchTextBoxOnScreenRow.classList.add("formInputs");
                }

                SearchTextBoxOnScreenRow.insertBefore(concernedSearchTextBox, SearchType);
                SearchTextBoxOnScreenRow.insertBefore(MainSearch, concernedSearchTextBox);
                SearchTextBoxOnScreenRow.appendChild(SearchBtn);

            }
            else if (selectedSearchType.length == 2) {

                let searchTypeNameOne = SelectedsearchTypeName.eq(0).data("search-type-name"),
                    searchTypeNamedTwo = SelectedsearchTypeName.eq(1).data("search-type-name"),
                    concernedSearchTextBoxOne = document.getElementById(searchTypeNameOne).cloneNode(true),
                    concernedSearchTextBoxTwo = document.getElementById(searchTypeNamedTwo).cloneNode(true),
                    concernedSearchTextBoxArr = [concernedSearchTextBoxOne, concernedSearchTextBoxTwo];


                if (selectedSearchType.includes("4")) {
                    /*
                     * 4 mean if user choose  "Type Of Transmission Gear" as a search type
                     */

                    /*
                     * now we want to know  what's the variable that contains  "Type Of Transmission Gear"
                     * element concernedSearchTextBoxOne or concernedSearchTextBoxTwo variable?
                     * 
                     * then we want to reduce SerachType DDL Width
                     */
                    for (let i = 0; i < selectedSearchType.length; i++) {
                        if (concernedSearchTextBoxArr[i].getAttribute("id") == "TypeOfTransmissionGear") {
                            document.querySelector(".multi-select-button").classList.add("custom-multi-select-button");
                        }
                    }

                }

                SearchTextBoxOnScreenRow.classList.remove("formInputs");
                //this code for 2 cases 1&2 
                let allTempSearchTextBox = document.querySelectorAll(".tempSearchTextBox");
                for (let i = 0; i < allTempSearchTextBox.length; i++) {
                    allTempSearchTextBox[i].remove();
                }

                concernedSearchTextBoxOne.querySelector(".form-control").setAttribute("name", `${searchTypeNameOne}txt`);
                concernedSearchTextBoxTwo.querySelector(".form-control").setAttribute("name", `${searchTypeNamedTwo}txt`);

                MainSearch.classList.add("col-md-3");
                MainSearch.classList.add("tempSearchTextBox");
                concernedSearchTextBoxOne.classList.add("col-md-3");
                concernedSearchTextBoxOne.classList.add("tempSearchTextBox");
                concernedSearchTextBoxTwo.classList.add("col-md-3");
                concernedSearchTextBoxTwo.classList.add("tempSearchTextBox");
                SearchBtn.classList.add("tempSearchTextBox");


                SearchTextBoxOnScreenRow.insertBefore(concernedSearchTextBoxOne, SearchType);
                SearchTextBoxOnScreenRow.insertBefore(concernedSearchTextBoxTwo, SearchType);
                SearchTextBoxOnScreenRow.insertBefore(MainSearch, concernedSearchTextBoxOne);
                SearchTextBoxOnScreenRow.appendChild(SearchBtn);

            }
            else if (selectedSearchType.length == 3) {
                let searchTypeNameOne = SelectedsearchTypeName.eq(0).data("search-type-name"),
                    searchTypeNamedTwo = SelectedsearchTypeName.eq(1).data("search-type-name"),
                    searchTypeNamedThree = SelectedsearchTypeName.eq(2).data("search-type-name"),
                    concernedSearchTextBoxOne = document.getElementById(searchTypeNameOne).cloneNode(true),
                    concernedSearchTextBoxTwo = document.getElementById(searchTypeNamedTwo).cloneNode(true),
                    concernedSearchTextBoxthree = document.getElementById(searchTypeNamedThree).cloneNode(true);


                document.querySelector(".multi-select-button").classList.add("custom-multi-select-button");


                let allTempSearchTextBox = document.querySelectorAll(".tempSearchTextBox");
                for (let i = 0; i < allTempSearchTextBox.length; i++) {
                    allTempSearchTextBox[i].remove();
                }

                concernedSearchTextBoxOne.querySelector(".form-control").setAttribute("name", `${searchTypeNameOne}txt`);
                concernedSearchTextBoxTwo.querySelector(".form-control").setAttribute("name", `${searchTypeNamedTwo}txt`);
                concernedSearchTextBoxthree.querySelector(".form-control").setAttribute("name", `${searchTypeNamedThree}txt`);


                MainSearch.classList.add("col-md-2");
                MainSearch.classList.add("tempSearchTextBox");
                concernedSearchTextBoxOne.classList.add("col-md-2");
                concernedSearchTextBoxOne.classList.add("tempSearchTextBox");
                concernedSearchTextBoxTwo.classList.add("col-md-2");
                concernedSearchTextBoxTwo.classList.add("tempSearchTextBox");
                concernedSearchTextBoxthree.classList.add("col-md-2");
                concernedSearchTextBoxthree.classList.add("tempSearchTextBox");
                SearchType.classList.add("col-md-offset-1");
                SearchBtn.classList.add("tempSearchTextBox");

                SearchTextBoxOnScreenRow.insertBefore(concernedSearchTextBoxOne, SearchType);
                SearchTextBoxOnScreenRow.insertBefore(concernedSearchTextBoxTwo, SearchType);
                SearchTextBoxOnScreenRow.insertBefore(concernedSearchTextBoxthree, SearchType);
                SearchTextBoxOnScreenRow.insertBefore(MainSearch, concernedSearchTextBoxOne);
                SearchTextBoxOnScreenRow.appendChild(SearchBtn);

                if (selectedSearchType.includes("4")) {
                    /*
                     * 4 mean if user choose  "Type Of Transmission Gear" as a search type
                     * so we want to reduce label for all input
                     */

                    $(".Main-Gallery .form-group > label").css("font-size", "7px");

                }
            }
            else if (selectedSearchType.length == 4) {
                let searchTypeNameOne = SelectedsearchTypeName.eq(0).data("search-type-name"),
                    searchTypeNamedTwo = SelectedsearchTypeName.eq(1).data("search-type-name"),
                    searchTypeNamedThree = SelectedsearchTypeName.eq(2).data("search-type-name"),
                    searchTypeNamedFour = SelectedsearchTypeName.eq(3).data("search-type-name"),
                    concernedSearchTextBoxOne = document.getElementById(searchTypeNameOne).cloneNode(true),
                    concernedSearchTextBoxTwo = document.getElementById(searchTypeNamedTwo).cloneNode(true),
                    concernedSearchTextBoxthree = document.getElementById(searchTypeNamedThree).cloneNode(true),
                    concernedSearchTextBoxFour = document.getElementById(searchTypeNamedFour).cloneNode(true),
                    concernedSearchTextBoxArr = [concernedSearchTextBoxOne, concernedSearchTextBoxTwo, concernedSearchTextBoxthree, concernedSearchTextBoxFour];



                if (selectedSearchType.includes("4")) {
                    /*
                     * 4 mean if user choose  "Type Of Transmission Gear" as a search type
                     * 
                     * 
                     * although that we always we will choose Type Of Transmission Gear because 
                     * we in selectedSearchType.length == 4 so we put this if-sta[if(selectedSearchType.length == 4)]
                     * for dynamic reasons => which maybe later in future we add more than 4 search type 
                     * so if in this case we need to use if-sta[if(selectedSearchType.length == 4)] to know
                     * if we choose "Type Of Transmission Gear" or not
                     */

                    /*
                     * now we want to know what's the variable that contains  "Type Of Transmission Gear"
                     * element concernedSearchTextBoxOne or concernedSearchTextBoxTwo variable?
                     * 
                     * then we want to reduce col bootstrap class for all inputs
                     */
                    for (let i = 0; i < selectedSearchType.length; i++) {
                        if (concernedSearchTextBoxArr[i].getAttribute("id") != "TypeOfTransmissionGear") {
                            concernedSearchTextBoxArr[i].classList.add("custom-col");
                        }
                    }

                }
                let allTempSearchTextBox = document.querySelectorAll(".tempSearchTextBox");
                for (let i = 0; i < allTempSearchTextBox.length; i++) {
                    allTempSearchTextBox[i].remove();
                }

                concernedSearchTextBoxOne.querySelector(".form-control").setAttribute("name", `${searchTypeNameOne}txt`);
                concernedSearchTextBoxTwo.querySelector(".form-control").setAttribute("name", `${searchTypeNamedTwo}txt`);
                concernedSearchTextBoxthree.querySelector(".form-control").setAttribute("name", `${searchTypeNamedThree}txt`);
                concernedSearchTextBoxFour.querySelector(".form-control").setAttribute("name", `${searchTypeNamedFour}txt`);

                MainSearch.classList.add("col-md-2");
                MainSearch.classList.add("tempSearchTextBox");
                MainSearch.classList.add("custom-col");
                concernedSearchTextBoxOne.classList.add("col-md-2");
                concernedSearchTextBoxOne.classList.add("tempSearchTextBox");
                concernedSearchTextBoxTwo.classList.add("col-md-2");
                concernedSearchTextBoxTwo.classList.add("tempSearchTextBox");
                concernedSearchTextBoxthree.classList.add("col-md-2");
                concernedSearchTextBoxthree.classList.add("tempSearchTextBox");
                concernedSearchTextBoxFour.classList.add("col-md-2");
                concernedSearchTextBoxFour.classList.add("tempSearchTextBox");
                SearchType.classList.add("custom-col");
                SearchBtn.classList.add("tempSearchTextBox");

                SearchTextBoxOnScreenRow.insertBefore(concernedSearchTextBoxOne, SearchType);
                SearchTextBoxOnScreenRow.insertBefore(concernedSearchTextBoxTwo, SearchType);
                SearchTextBoxOnScreenRow.insertBefore(concernedSearchTextBoxthree, SearchType);
                SearchTextBoxOnScreenRow.insertBefore(concernedSearchTextBoxFour, SearchType);
                SearchTextBoxOnScreenRow.insertBefore(MainSearch, concernedSearchTextBoxOne);
                SearchTextBoxOnScreenRow.appendChild(SearchBtn);

                $(".Main-Gallery .form-group > label").css("font-size", "7px");
            }

            $('.yearpicker').datepicker({
                minViewMode: 2,
                format: 'yyyy',
                orientation: 'auto bottom'//this prop make date picker always open from bottom orientation
            });
        }
        /*
             * because we use cloneNode() the process that were 
             * responsible to add not-empty(when focus on input) class and remve it(when blur it)
             * it is no longer working so will do it manually
             */
        $('.form-control').on("focus", function () {

            if ($(this).parent().attr("class") === "form-group") {
                $(this).parent().addClass('not-empty');
            }
            else {
                $(this).parent().parent().addClass('not-empty');
            }
        });
        $('.form-control').on("blur", function () {
            if ($(this).val() == '') {
                if ($(this).parent().attr("class") === "form-group not-empty") {
                    $(this).parent().removeClass('not-empty');
                }
                else {
                    $(this).parent().parent().removeClass('not-empty');
                }
            }
        });
    });
});
function submitEvent(inputRequiredMsg, positiveNumberMsg) {
    /*
     * we don't use mvc validation because the name of inputs differs from 
     * property name (which we changed it from js) so when changed it using js
     * the mvc validation will not work so for this reason we will write validation manually
     **/
    let allSearchTextInput = document.querySelectorAll('#SearchTextBoxOnScreen .form-control'),
        rowDiv = document.createElement("div"),
        col12Div = rowDiv.cloneNode(),
        errorMsgSpan = document.createElement("span"),
        errorMsgSpanText = null,
        searchTextBoxOnScreenDiv = document.getElementById("SearchTextBoxOnScreen");

    document.getElementById("SelectedSearchTypeValue").value = '';

    if (document.getElementById("ErrorMsg") != null) {
        document.getElementById("ErrorMsg").remove();
    }

    rowDiv.setAttribute("id", "ErrorMsg");
    rowDiv.setAttribute("class", "row text-center");
    col12Div.setAttribute("class", "col-md-12");
    errorMsgSpan.setAttribute("class", "text-danger");
    
    if (allSearchTextInput.length > 1) {
        /*[start]
         * here Required Validation for inputs => when number of seach greater than 1
         */
        for (let i = 0; i < allSearchTextInput.length; i++) {
            document.getElementById("SelectedSearchTypeValue").value += allSearchTextInput[i].parentElement.parentElement.getAttribute("data-search-type-ddl-value") == null ? allSearchTextInput[i].parentElement.parentElement.parentElement.getAttribute("data-search-type-ddl-value") + "|" : allSearchTextInput[i].parentElement.parentElement.getAttribute("data-search-type-ddl-value") + "|";

            if (allSearchTextInput[i].value == '') {

                errorMsgSpanText = document.createTextNode(inputRequiredMsg);
                errorMsgSpan.appendChild(errorMsgSpanText);
                col12Div.appendChild(errorMsgSpan);
                rowDiv.appendChild(col12Div);
                searchTextBoxOnScreenDiv.appendChild(rowDiv);

                return false;
            }
        }
        //[End]

        /*
         * [start]
         * here PositiveNumber Validation for Price Inputs
         */
        
        let priceInput = document.querySelector("#SearchTextBoxOnScreen #PriceTextSearch");
        if (priceInput != null)
        {
            /*
             * != null => this check in order to check if we choose PriceInput as one 
             * of searchType or not.
             */
            if (parseInt(priceInput.value) < 1) {
                errorMsgSpanText = document.createTextNode(positiveNumberMsg);

                errorMsgSpan.appendChild(errorMsgSpanText);
                col12Div.appendChild(errorMsgSpan);
                rowDiv.appendChild(col12Div);
                searchTextBoxOnScreenDiv.appendChild(rowDiv);

                return false;
            }
        }
        //[End]
    }

    
    
}
function displaySearchInputText(SelectedSearchTypeValue) {
    /*
     * this func display same number of SearchInputText that was
     * before we did search process
     *
     * in other meaning => when we doing post search process then => return to
     * view => this fucn => will re-display same number of SearchInputText
     */
    SelectedSearchTypeValue = SelectedSearchTypeValue.slice(0, SelectedSearchTypeValue.length - 1);//here we Remove last Character from string(last char is |)
    let SelectedSearchTypeValueArr = SelectedSearchTypeValue.split('|');

    $(function () {
        for (let i = 1; i < SelectedSearchTypeValueArr.length; i++) {
            /*
             * i start from 1 because always first element of SelectedSearchTypeValueArr will be 0 which represent
             * CarType we give it zero because it don't have item in SearchType DDL
             */
            $(`.SearchType option[value=${SelectedSearchTypeValueArr[i]}]`).prop('selected', 'selected').change();
        }
    });
}
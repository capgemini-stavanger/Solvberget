(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/loginpage/loginpage.html", {

        ready: function (element, options) {
            document.getElementById("outputMsgPinReq").innerHTML = "";

            document.getElementById("loginSubmit").addEventListener("click", login);
            document.getElementById("forgotPinCodeExpand").addEventListener("click", showPinRequestContainer);
            document.getElementById("cancelExpandButton").addEventListener("click", hidePinRequestContainer);
            document.getElementById("submitPinRequestButton").addEventListener("click", requestPinCode);

            $("#loginLoading").hide();
            $("#pinRequestLoading").hide();

        },

        unload: function () {

        },

        updateLayout: function (element) {
 
        }
    });

    function showPinRequestContainer() {
        $("#pinRequestContainer").show();
    }

    function hidePinRequestContainer() {
        $("#pinRequestContainer").hide();
    }

    function login() {
        $("#loginLoading").show();
        $("#loginErrorMessage").text("");

        var userId = $("#loginUserId").val();
        var verification = $("#loginPin").val();
        var context = { that: this };

        $("loginSubmit").attr("disabled", "disabled");
        var url = Data.serverBaseUrl + "/User/GetUserInformation/" + userId + "/" + verification;
        Solvberget.Queue.QueueDownload("login", { url: url }, ajaxDoLoginCallback, context, true);
    }


    function ajaxDoLoginCallback(request) {
        var response = request.responseText == "" ? "" : JSON.parse(request.responseText);

        if (response.IsAuthorized) {

            $("#loginSuccessMessage").text("Innlogging vellykket! Vent litt...");

            var borrowerId = response.BorrowerId;
            var libraryId = response.Id;
            var notifications = response.Notifications;

            if (borrowerId != undefined && libraryId != undefined) {

                var applicationData = Windows.Storage.ApplicationData.current;

                if (applicationData)
                    var roamingSettings = applicationData.roamingSettings;

                if (roamingSettings) {
                    roamingSettings.values["BorrowerId"] = borrowerId;
                    roamingSettings.values["LibraryUserId"] = libraryId;
                }

                window.localStorage.setItem("BorrowerId", borrowerId);
                window.localStorage.setItem("LibraryUserId", libraryId);

                if (notifications) {
                    if (!Notifications.areNotificationsSeen()) {
                        for (var i = 0; i < notifications.length; i++) {
                            Toast.showToast(notifications[i].Title, notifications[i].Content);
                        }
                    }
                    Notifications.setUserNotifications(notifications);
                }

                LoginFlyout.updateAppBarButton();
            }

            setTimeout(function () {
                WinJS.Navigation.navigate("/pages/mypage/mypage.html");
                LiveTile.liveTile();
            }, 1200);

        }
        else {
            $("#loginErrorMessage").text("Feil lånernummer/pin");
            $("#loginSubmit").removeAttr("disabled");
        }

        $("#loginLoading").hide();
    }

    function requestPinCode() {
        $("#pinRequestLoading").show();
        var requestUrlBase = window.Data.serverBaseUrl + "/User/RequestPinCodeToSms/";
        var userId = $("#userIdPinReq").val();
        var url = requestUrlBase + userId;

        Solvberget.Queue.QueueDownload("pin", { url: url }, sendPinRequestCallback, null, true);
    }

    function sendPinRequestCallback(request, context) {
        var response = request.responseText == "" ? "" : JSON.parse(request.responseText);

        var outputMsgPinReq = document.getElementById("outputMsgPinReq");
        if (response.Success == true) {
            if (outputMsgPinReq != undefined) {
                if (response.Reply) {
                    outputMsgPinReq.innerHTML = response.Reply;
                }
                else {
                    outputMsgPinReq.innerHTML = "Din PIN-kode har blitt sendt på SMS.";
                }
            }
            $("#outputMsgPinReq").removeClass("error");
            $("#outputMsgPinReq").addClass("success");
            $("#submitPinRequestButton").removeAttr("disabled");
            $("#cancelFlyoutButton").html("Lukk");
            $("#pinRequestLoading").hide();

        }
        else {
            if (outputMsgPinReq != undefined) {
                if (response.Reply) {
                    outputMsgPinReq.innerHTML = response.Reply;
                }
                else {
                    outputMsgPinReq.innerHTML = "Forespørselen kunne ikke utføres.";
                }
            }
            $("#outputMsgPinReq").removeClass("success");
            $("#outputMsgPinReq").addClass("error");
            $("#submitPinRequestButton").removeAttr("disabled");
            $("#cancelFlyoutButton").html("Lukk");
            $("#pinRequestLoading").hide();
        }

    }

    

})();

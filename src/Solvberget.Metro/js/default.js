(function () {
    "use strict";

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;
    var nav = WinJS.Navigation;
    WinJS.strictProcessing();
    var messageDialog;
    WinJS.Binding.optimizeBindingReferences = true;

    // Gracefull exit
    app.onerror = function (customEventObject) {

        if (customEventObject.type === "error") {

            // Get the error message and name for this exception
            if (customEventObject.detail.error == null) {
                exceptionError("Beklager, det oppstod en feil. \n\nTeknisk informasjon:\n\n " + customEventObject.detail.exception.message, "Feilmelding (" + customEventObject.detail.exception.name + ")");
                return true;
            }

            if (customEventObject.detail.error.status == 500) {
                exceptionError("Kunne ikke hente data fra webtjener.", "Feil ved tilkobling til webtjener");
                return true;
            }

            exceptionError("Beklager, det oppstod en feil. \n\nTeknisk informasjon:\n\nFeil: " + customEventObject.detail.error.name +
                "\nStatus: " + customEventObject.detail.error.status +
                "\nMelding: " + customEventObject.detail.error.message,
                "Feilmelding");

        }

        // Tell windows that we have taken care of the exception
        return true;

    };

    function exceptionError(msg, title) {

        // Check if the message dialog is not already showing
        if (!messageDialog) {

            // Create the message dialog and set its content
            messageDialog = new Windows.UI.Popups.MessageDialog(msg, title);

            // Add commands and set their command handlers
            messageDialog.commands.append(
                new Windows.UI.Popups.UICommand("Lukk", closeCommandInvoked));

            // Set the command that will be invoked by default
            messageDialog.defaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.cancelCommandIndex = 0;

            // Show the message dialog
            messageDialog.showAsync();
        }
    };
    function closeCommandInvoked(command) {
        // Reset message dialog
        messageDialog = undefined;

        // Go home
        Data.navigateToHome();
    };

    // as AppBarCommands marked with secondary section aren't accessible through the DOM
    // we need to make the AppBar and its commands programatically
    function createAppBar() {
        var appBar = document.getElementById("appBar");
        var appBarCtrl = appBar.winControl;

        var homeCmd = new WinJS.UI.AppBarCommand(null,
        {
            id: 'toHomeButton',
            label: 'Hjem',
            section: 'primary',
            tooltip: 'Gå hjem',
            icon: 'home'
        });
        homeCmd.onclick = Data.navigateToHome;

        var myPageCmd = new WinJS.UI.AppBarCommand(null, {
            id: 'toMyPageButton',
            label: 'Min side',
            section: 'primary',
            tooltip: 'Gå til Min side',
            icon: 'contact'
        });
        myPageCmd.onclick = Data.navigateToMypage;

        var searchCmd = new WinJS.UI.AppBarCommand(null, {
            id: 'toSearchButton',
            label: 'Søk',
            section: 'primary',
            tooltip: 'Søk i biblioteket',
            icon: 'find'
        });
        searchCmd.onclick = Data.navigateToSearch;

        var loginCmd = new WinJS.UI.AppBarCommand(null, {
            id: 'cmdLoginFlyout',
            label: 'Logg ut',
            section: 'secondary',
            tooltip: 'Logg ut'

        });
        loginCmd.onclick = doLogin;

        var eventsCmd = new WinJS.UI.AppBarCommand(null, {
            id: 'toEventsButton',
            label: 'Arrangementer',
            section: 'secondary',
            tooltip: 'Gå til arrangementer'
        });
        eventsCmd.onclick = Data.navigateToEvents;

        var blogsCmd = new WinJS.UI.AppBarCommand(null, {
            id: 'toBlogsPageButton',
            label: 'Blogger',
            section: 'secondary',
            tooltip: 'Gå til blogger'
        });
        blogsCmd.onclick = Data.navigateToBlogs;

        var newsCmd = new WinJS.UI.AppBarCommand(null, {
            id: 'toNewsButton',
            label: 'Nyheter',
            section: 'secondary',
            tooltip: 'Nyheter fra Sølvberget'
        });
        newsCmd.onclick = Data.navigateToNews;

        var recommendationsCmd = new WinJS.UI.AppBarCommand(null, {
            id: 'toListsButton',
            label: 'Anbefalinger og topplister',
            section: 'secondary',
            tooltip: 'Gå til anbefalinger og topplister'
        });
        recommendationsCmd.onclick = Data.navigateToLists;

        var openingHoursCmd = new WinJS.UI.AppBarCommand(null, {
            id: 'toOpeningHoursButton',
            label: 'Åpningstider',
            section: 'secondary',
            tooltip: 'Gå til åpningstider'
        });
        openingHoursCmd.onclick = Data.navigateToOpeningHours;

        var contactCmd = new WinJS.UI.AppBarCommand(null, {
            id: 'toContactButton',
            label: 'Kontakt',
            section: 'secondary',
            tooltip: 'Gå til kontaktinformasjon'
        });
        contactCmd.onclick = Data.navigateToContact;

        var pinCmd = new WinJS.UI.AppBarCommand(null, {
            id: 'cmdPin',
            section: 'secondary',
            label: 'Fest til start'
        });
        pinCmd.onclick = pinToStart;

        appBarCtrl.data = new WinJS.Binding.List([homeCmd, myPageCmd, searchCmd, loginCmd, eventsCmd, newsCmd, recommendationsCmd, openingHoursCmd, contactCmd, pinCmd]);

        appBar.addEventListener("beforeshow", setAppbarButton());
    }


    app.onactivated = function (args) {

        if (args.detail.kind === activation.ActivationKind.launch) {

            // TODO: applies only to desktop. Target for desktop.
            //var settingsPane = Windows.UI.ApplicationSettings.SettingsPane.getForCurrentView();
            //settingsPane.addEventListener("commandsrequested", onCommandsRequested);

            var applicationData = Windows.Storage.ApplicationData.current;
            applicationData.addEventListener("datachanged", roamingDataChangeHandler);

            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: This application has been newly launched.
                // Initialize your application here.
            } else {
                // TODO: This application has been reactivated from suspension.
                // Restore application state here.
            }

            if (args.detail.arguments !== "") {

                if (app.sessionState.history) {
                    nav.history = app.sessionState.history;
                } else {
                    nav.history.backStack.push({ location: "/pages/home/home.html" });
                }

                args.setPromise(WinJS.UI.processAll().done(function () {
                    nav.navigate(args.detail.arguments);
                }));

            } else {

                if (app.sessionState.history) {
                    nav.history = app.sessionState.history;
                }
                args.setPromise(WinJS.UI.processAll().done(function () {
                    if (nav.location) {
                        nav.history.current.initialPlaceholder = true;
                        nav.navigate(nav.location, nav.state);
                    } else {
                        nav.navigate(Application.navigator.home);
                    }
                }));
            }

            Notifications.setAreNotificationsSeen(false);

            createAppBar();

            LiveTile.liveTile();
        }
    };

    function roamingDataChangeHandler(eventArgs) {
        // TODO: Refresh your data
    }

    app.oncheckpoint = function (args) {
        // TODO: This application is about to be suspended. Save any state
        // that needs to persist across suspensions here. You might use the
        // WinJS.Application.sessionState object, which is automatically
        // saved and restored across suspension. If you need to complete an
        // asynchronous operation before your application is suspended, call
        // args.setPromise().
        app.sessionState.history = nav.history;
    };

    app.start();
})();

function doLogin() {

    // Get active user
    var user = LoginFlyout.getLoggedInBorrowerId();

    // If user was not logging out, user was logging in, so show login


    // TODO: ROAMING
    var loginDiv = document.getElementById("loginFragmentHolder");
    loginDiv.innerHTML = "";
    WinJS.UI.Fragments.renderCopy("/fragments/login/login.html", loginDiv).done(function () {

        var loginAnchor = document.querySelector("div");

        LoginFlyout.showLogin(loginAnchor);
    });

}

function pinByElementAsync(element, newTileID, newTileShortName, newTileDisplayName) {

    var uriLogo = new Windows.Foundation.Uri("ms-appx:///images/home/" + newTileID + ".png");
    var uriSmallLogo = new Windows.Foundation.Uri("ms-appx:///images/solvberget30.png");
    var currentTime = new Date();
    var TileActivationArguments = WinJS.Navigation.location;
    var tile = new Windows.UI.StartScreen.SecondaryTile(newTileID, newTileShortName, newTileDisplayName, TileActivationArguments, Windows.UI.StartScreen.TileOptions.showNameOnLogo, uriLogo);
    tile.foregroundText = Windows.UI.StartScreen.ForegroundText.light;
    tile.smallLogo = uriSmallLogo;

    var selectionRect = element.getBoundingClientRect();

    return new WinJS.Promise(function (complete, error, progress) {
        tile.requestCreateAsync({ x: selectionRect.left, y: selectionRect.top }).done(function (isCreated) {
            if (isCreated) {
                complete(true);
            } else {
                complete(false);
            }
        });
    });
}

function unpinByElementAsync(element, unwantedTileID) {

    var selectionRect = element.getBoundingClientRect();

    var tileToGetDeleted = new Windows.UI.StartScreen.SecondaryTile(unwantedTileID);

    return new WinJS.Promise(function (complete, error, progress) {
        tileToGetDeleted.requestDeleteForSelectionAsync({ height: selectionRect.height, width: selectionRect.width, x: selectionRect.left, y: selectionRect.top }).done(function (isDeleted) {
            if (isDeleted) {
                complete(true);
            } else {
                complete(false);
            }
        });
    });
}

function pinToStart() {
    document.getElementById("appBar").winControl.sticky = true;

    if (WinJS.UI.AppBarIcon.unpin === document.getElementById("cmdPin").winControl.icon) {
        unpinByElementAsync(document.getElementById("cmdPin"), Data.activePage).then(function (isDeleted) {
            if (isDeleted) {
                setAppbarButton();
            } else {
            }
        });

    } else {

        pinByElementAsync(document.getElementById("cmdPin"), Data.activePage, "Sølvberget", "Sølvberget - Stavanger Bibliotek").then(function (isCreated) {
            if (isCreated) {
                setAppbarButton();
            } else {
            }
        });
    }
}

function setAppbarButton() {

    LoginFlyout.updateAppBarButton();
    var exist = Windows.UI.StartScreen.SecondaryTile.exists(Data.activePage);

    if (exist) {
        document.getElementById("cmdPin").winControl.label = "Fjern fra start";
        document.getElementById("cmdPin").winControl.icon = "unpin";
        document.getElementById("cmdPin").winControl.tooltip = "Fjern fra start";
    } else {
        document.getElementById("cmdPin").winControl.label = "Pin til start";
        document.getElementById("cmdPin").winControl.icon = "pin";
        document.getElementById("cmdPin").winControl.tooltip = "Pin til start";
    }
}

function onPrivacyCommand(settingsCommand) {
    var uriToLaunch = "http://www.stavanger-kulturhus.no/SOELVBERGET/App-personvern";
    var uri = new Windows.Foundation.Uri(uriToLaunch);
    Windows.System.Launcher.launchUriAsync(uri).then(
       function (success) {
           if (success) {
               // URI launched
           } else {
               // URI launch failed
           }
       });
}

function onAttributionCommand(settingCommand) {
    WinJS.UI.SettingsFlyout.showSettings("attributionSettingsFlyout", "/pages/charms/attr/attribution.html");
}

function onCommandsRequested(eventArgs) {

    var privacyCommand = new Windows.UI.ApplicationSettings.SettingsCommand("personvern", "Personvernerklæring", onPrivacyCommand);
    eventArgs.request.applicationCommands.append(privacyCommand);

    var attributionCommand = new Windows.UI.ApplicationSettings.SettingsCommand("attribution", "Kredittering", onAttributionCommand);
    eventArgs.request.applicationCommands.append(attributionCommand);

}

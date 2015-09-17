(function () {
    "use strict";

    var ui = WinJS.UI;
    var event;

    ui.Pages.define("/pages/events/eventDetail/eventDetail.html", {

        ready: function (element, options) {
            var item = options.item;
            event = options.item;
            WinJS.Binding.processAll(element, item);

            if (!item.imageUrl) {
                $(".event-image-container").css("display", "none");
                $(".event-image-container").css("-ms-grid-row", "0");
                $(".event-content-holder").css("-ms-grid-row", "2");
            }

            element.querySelector(".titlearea .pagetitle").textContent = event.name;
            document.getElementById("cal-button").addEventListener("click", this.openIcalForEvent);
            document.getElementById("link-button").addEventListener("click", this.openLinkToEvent);
            element.querySelector(".content").focus();
        },

        openIcalForEvent: function () {
            // create custom iCalendar-link
            var pathArray = event.ticketUrl.split("/");
            var host = pathArray[0] + "//" + pathArray[2];
            var uriRaw = host + "/events/" + event.ticketCoId + ".ics";
            var uri = new Windows.Foundation.Uri(uriRaw);
            Windows.System.Launcher.launchUriAsync(uri).then(
            function (success) {
                if (success) {
                    //No-op
                } else {
                    //Error
                }
            });
        },

        openLinkToEvent: function () {
            var uriRaw = event.ticketUrl;
            var uri = new Windows.Foundation.Uri(uriRaw);
            Windows.System.Launcher.launchUriAsync(uri).then(
            function (success) {
                if (success) {
                    //No-op
                } else {
                    //Error
                }
            });
        }

    });

    WinJS.Namespace.define("EventItemConverters", {

        startConverter: WinJS.Binding.converter(function (start) {
            return (start == undefined || start === "") ? "" : "Starter kl. " + moment(start).format("HH:mm");
        }),

        stopConverter: WinJS.Binding.converter(function (stop) {
            return (stop == undefined || stop === "") ? "" : "Slutter kl. " + moment(stop).format("HH:mm");
        }),

        typeConverter: WinJS.Binding.converter(function (type) {
            return (type == undefined || type === "") ? "" : "Type arrangement: " + type;
        }),

        priceConverter: WinJS.Binding.converter(function (price) {
            return (price == undefined || price === "") ? "" : "Pris: " + price + " kr";
        }),
        dateConverter: WinJS.Binding.converter(function (date) {
            return (date == undefined || date === "") ? "" : moment(date).lang("nb").format("L");
        })



    });

})();

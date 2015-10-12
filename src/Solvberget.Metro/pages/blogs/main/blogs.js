(function () {
    "use strict";

    var ui = WinJS.UI;

    ui.Pages.define("/pages/blogs/main/blogs.html", {

        ready: function () {
            getBlogs();
            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());
        },
        unload: function () {
            Solvberget.Queue.CancelQueue('blogs');
        }
    });
})();

var ajaxGetBlogs = function () {
    var url = window.Data.serverBaseUrl + "/Blog/GetBlogs";
    Solvberget.Queue.QueueDownload("blogs", { url: url }, ajaxGetBlogsCallback, this, true);
};

var ajaxGetBlogsCallback = function (request) {
    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
    if (response != undefined && response !== "") {
        populateBlogs(response);
    }
    $("#blogsContent").fadeIn("slow");
    $("#blogsLoading").hide();
};

var populateBlogs = function (response) {

    var itemTemplate = document.getElementById("blog-template");
    var itemMobileTemplate = document.getElementById("blog-mobile-template");
    var listview = document.getElementById("blogs-listview").winControl;

    var bindingList = new WinJS.Binding.List();
    for (var i = 0; i < response.length; i++) {
        response[i].Index = i;
        bindingList.push(response[i]);
    }

    var screenWidth = screen.width;
    if (screenWidth <= 400) {
        listview.itemTemplate = itemMobileTemplate;
        listview.layout = new WinJS.UI.ListLayout();
    } else {
        listview.itemTemplate = itemTemplate;
    }

    listview.itemDataSource = bindingList.dataSource;
    listview.oniteminvoked = function (args) {
        args.detail.itemPromise.done(function (item) {
            WinJS.Navigation.navigate("/pages/blogs/entries/entries.html", { blogId: args.detail.itemIndex, blogModel: item });
        });
    }
};

var getBlogs = function () {
    $("#blogsContent").hide();
    $("#blogsLoading").fadeIn();
    ajaxGetBlogs();
};


WinJS.Namespace.define("BlogConverters", {
    cateogriesConverter: WinJS.Binding.converter(function (categories) {
        var outputStr = "Kategorier: ";
        for (x in categories) {
            outputStr += categories[x] + ", ";
        }
        outputStr = outputStr.substr(0, outputStr.length - 2);
        return outputStr;
    }),
    backgroundColorConverter: WinJS.Binding.converter(function (index) {
        return Data.getColorFromBlogsPool(index % 3, "1.0");
    }),
    backgroundColorEntryConverter: WinJS.Binding.converter(function (index) {
        return Data.getColorFromSubsetPool(index % 8, "1.0");
    }),
    entriesConverter: WinJS.Binding.converter(function (entries) {
        return "Antall innlegg: " + entries.length;
    }),
    subtitleConverter: WinJS.Binding.converter(function (title) {
        return "De siste innleggene fra " + title;
    }),
    publishedDate: WinJS.Binding.converter(function (published) {
        return "Publisert: " + published;
    }),
    updatedDate: WinJS.Binding.converter(function (updated) {
        return "Sist oppdatert: " + updated;
    }),
    descriptionWrapper: WinJS.Binding.converter(function (description) {
        return description;
    }),
    authorConverter: WinJS.Binding.converter(function (author) {
        return "Av: " + author;
    }),
    thumbnailConverter: WinJS.Binding.converter(function (thumbnailUrl) {
        return thumbnailUrl ? thumbnailUrl : "#";
    }),
    undefinedHider: WinJS.Binding.converter(function (attr) {
        return attr ? "block" : "none";
    })
});
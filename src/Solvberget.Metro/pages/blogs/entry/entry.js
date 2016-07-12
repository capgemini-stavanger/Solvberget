(function () {
    "use strict";

    var ui = WinJS.UI;

    ui.Pages.define("/pages/blogs/entry/entry.html", {
        ready: function (element, options) {
            getBlogEntry(options.blogId, options.entryId);
        }
    });
})();

var getBlogEntry = function (blogId, entryId) {
    var url = window.Data.serverBaseUrl + "/blogs/" + blogId.toString() + "/" + entryId.toString();
    Solvberget.Queue.QueueDownload("blogs", { url: url }, ajaxGetBlogEntryCallback, this, true);
}

var ajaxGetBlogEntryCallback = function (request) {
    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
    if (response != undefined && response !== "") {
        populateBlogEntry(response);
    }
};

var populateBlogEntry = function (response) {
    var blogEntryDiv = document.getElementById("mainBlogEntry");

    WinJS.Binding.processAll(blogEntryDiv, response).done(function () {
        $("a img").each(function () {
            if (!$(this).parent().parent().hasClass("separator")) // Bugfix with blogger.com
                $(this).parent().css("float", "left").css("padding", "10px");
        });

    });

    if (response && response.title) {
        if (response.title.length > 40) {
            $(".pagetitle").css("font-size", "15pt");
        }
    }

    var screenWidth = screen.width;
    if (screenWidth <= 400) {
        $(".blogentry.fragment").addClass("win-scrollview");
    }
};
(function () {
    "use strict";

    var ui = WinJS.UI;

    ui.Pages.define("/pages/blogs/entries/entries.html", {

        ready: function (element, options) {
            var blogId = options.blogId;
            var blogModel = options.blogModel.data;

            if (blogModel) {
                if (blogModel.title) {
                    if (blogModel.title.length > 40) {
                        blogModel.title = blogModel.title.substr(0, 37) + "...";
                    }    
                }
            }

            WinJS.Binding.processAll(element.querySelector(".fragment"), blogModel);
            getBlogWithEntries(blogId);
        },
        unload: function () {
            Solvberget.Queue.CancelQueue('blogs');
        }
    });

})();


var ajaxGetBlogWithEntries = function (blogId) {
    var url = window.Data.serverBaseUrl + "/blogs/" + blogId;
    Solvberget.Queue.QueueDownload("blogs", { url: url }, ajaxGetBlogWithEntriesCallback, this, true);
};
var ajaxGetBlogWithEntriesCallback = function (request, context) {
    var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
    if (response != undefined && response !== "")
        populateEntries(response);

    // Hide progress-ring, show content
    $("#entriesContent").fadeIn("slow");
    $("#entriesLoading").hide();
};

var populateEntries = function (response) {

    var itemTemplate = document.getElementById("blog-entry-template");
    var itemMobileTemplate = document.getElementById("blog-entry-mobile-template");
    var listview = document.getElementById("blog-entries-listview").winControl;

    var bindingList = new WinJS.Binding.List();
    for (var i = 0; i < response.posts.length; i++) {
        response.posts[i].Index = i;
        bindingList.push(response.posts[i]);
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
            WinJS.Navigation.navigate("/pages/blogs/entry/entry.html", { blogId: response.id, entryId: item.data.id });
        });
    }

};

var getBlogWithEntries = function (blogId) {
    // Show progress-ring, hide content
    $("#entriesContent").hide();
    $("#entriesLoading").fadeIn();

    // Get the user information from server
    ajaxGetBlogWithEntries(blogId);
};
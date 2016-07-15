(function () {

    "use strict";

    var ui = WinJS.UI;
    var utils = WinJS.Utilities;
    var nav = WinJS.Navigation;
    var app = WinJS.Application;

    var listRequestUrl = Data.serverBaseUrl + "/lists/";

    var lists = new Array();
    var listsBinding;

    var listSelectionIndex = -1;
    var continueToGetDocuments = false;
    var getListsHasReturnedCallback = false;

    ui.Pages.define("/pages/lists/libraryLists.html", {

        ready: function (element, options) {

            continueToGetDocuments = true;

            if (options && "selectedIndex" in options) {
                listSelectionIndex = options.selectedIndex;
            }
            else if (app.sessionState.listpageSelectionIndex && !this.isSingleColumn()) {
                if (app.sessionState.listpageSelectionIndex.index) {
                    listSelectionIndex = app.sessionState.listpageSelectionIndex.index;
                } else {
                    listSelectionIndex = -1;
                }
            }
            else {
                listSelectionIndex = -1;
            }

            //Init ListView
            var primaryListView = element.querySelector(".primary-listview").winControl;
            if (primaryListView) {
                primaryListView.layout = new ui.ListLayout();
                primaryListView.onselectionchanged = this.primaryListViewSelectionChanged.bind(this);
                primaryListView.itemTemplate = document.getElementById("listViewTemplateId");
            }

            //Hide either ListView (if we have selectionIndex != -1) or ListContent (selectionIndex == -1)
            this.updateVisibility();

            //Get the lists
            if (!getListsHasReturnedCallback)
                this.getLists(listRequestUrl, primaryListView);
            else {
                primaryListView.itemDataSource = listsBinding.dataSource;
                $("#listsLoading").hide();
                $("#listViewId").fadeIn();
                //this.processRemainingDocuments();
            }

            if (this.isSingleColumn()) {
                if (listSelectionIndex >= 0) {

                    var secondaryListView = document.getElementById("contents-listview").winControl;
                    var myTemplate = document.getElementById("documentTemplate");

                    var bindingList = new WinJS.Binding.List(lists[listSelectionIndex].Documents);

                    secondaryListView.itemDataSource = bindingList.dataSource;
                    secondaryListView.itemTemplate = myTemplate;
                    secondaryListView.layout = new WinJS.UI.ListLayout();
                    secondaryListView.oniteminvoked = function (args) {
                        args.detail.itemPromise.done(function (item) {
                            nav.navigate("/pages/documentDetail/documentDetail.html", { documentModel: item.data });
                        });
                    }

                    Solvberget.Queue.PrioritizeUrls('libraryList', lists[listSelectionIndex].urls);

                    if (this.doneLoadingDocuments(lists[listSelectionIndex].DocumentNumbers)) {
                        $(".headerProgress").hide();
                        $(".listTitle").text(lists[listSelectionIndex].Name);
                    }
                }
            } else {
                if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/lists/libraryLists.html") {
                    // Clean up the backstack to handle a user snapping, navigating
                    // away, unsnapping, and then returning to this page.
                    nav.history.backStack.pop();
                }
                // If this page has a selectionIndex, make that appear in the ListView.
                listSelectionIndex = Math.max(listSelectionIndex, 0);
                if (getListsHasReturnedCallback) primaryListView.selection.set(listSelectionIndex);
            }

            //AppBar
            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());

        },

        unload: function () {
            continueToGetDocuments = false;
            Solvberget.Queue.CancelQueue('libraryList');
        },

        primaryListViewSelectionChanged: function (args) {
            if (!continueToGetDocuments) return;

            $(".headerProgress").show();
            $(".listTitle").text("Laster..");

            var that = this;
            var primaryListView = this.element.querySelector(".primary-listview");
            var primaryListViewCtrl = primaryListView.winControl;
            if (primaryListViewCtrl) {
                primaryListViewCtrl.selection.getItems().done(function updateDetails(items) {
                    if (items.length > 0) {
                        listSelectionIndex = items[0].index;
                        if (that.isSingleColumn()) {
                            // If mobile, navigate to a new page containing the
                            // selected item's details.
                            nav.navigate("/pages/lists/libraryLists.html", { selectedIndex: listSelectionIndex });
                        }
                        else {
                            // If fullscreen or filled, update the details column with new data.
                            that.saveListSelectionIndex();

                            var secondaryListView = document.getElementById("contents-listview").winControl;
                            var myTemplate = document.getElementById("documentTemplate");
                            var requestUrl = listRequestUrl + items[0].data.id;

                            WinJS.xhr({ url: requestUrl }).then(
                                function (request) {
                                    if (!continueToGetDocuments)
                                        return;

                                    getListsHasReturnedCallback = true;
                                    var obj = JSON.parse(request.responseText);
                                    if (obj !== undefined) {
                                        var documents = obj.documents;
                                        var newList = [];
                                        for (var i = 0; i < documents.length; i++) {
                                            var stuff = documents[i];
                                            stuff.thumbnailUrl = "/images/placeholders/" + stuff.type + ".png";
                                            newList[i] = stuff;
                                        }

                                        var bindingList = new WinJS.Binding.List(newList);
                                        secondaryListView.itemDataSource = bindingList.dataSource;
                                        secondaryListView.itemTemplate = myTemplate;
                                        secondaryListView.layout = new WinJS.UI.ListLayout();
                                        secondaryListView.oniteminvoked = function (args) {
                                            args.detail.itemPromise.done(function (item) {
                                                nav.navigate("/pages/documentDetail/documentDetail.html", { documentModel: item.data });
                                            });
                                        }

                                        // Hide progress-ring, show content
                                        $(".headerProgress").hide();
                                        $(".listTitle").text(items[0].data.name);
                                    } else {
                                        //Error handling   
                                    }
                                },
                                function (request) {
                                    //Error handling
                                });
                        }
                    }
                });
            }
        },

        doneLoadingDocuments: function (documentNumbers) {
            if (documentNumbers !== undefined) {
                for (var docnumber in documentNumbers) {
                    if (!documentNumbers[docnumber]) {
                        return false;
                    }
                }
            }
            return true;
        },

        getLists: function (requestStr, listView) {

            WinJS.xhr({ url: requestStr }).then(
                function (request) {
                    if (!continueToGetDocuments) return;
                    getListsHasReturnedCallback = true;
                    var obj = JSON.parse(request.responseText);
                    if (obj !== undefined) {
                        lists = obj;
                        listsBinding = new WinJS.Binding.List(lists);
                        listView.itemDataSource = listsBinding.dataSource;
                        listView.selection.set(listSelectionIndex);
                        // Hide progress-ring, show content
                        $("#listsLoading").hide();
                        $("#listViewId").fadeIn();
                    } else {
                        //Error handling   
                    }
                },
                function (request) {
                    //Error handling
                });
        },

        saveListSelectionIndex: function () {
            var indexObj = { index: listSelectionIndex };
            app.sessionState.listpageSelectionIndex = indexObj;
        },

        updateLayout: function (element, viewState, lastViewState) {

            var listView = element.querySelector(".primary-listview").winControl;
            if (listView) {
                var firstVisible = listView.indexOfFirstVisible;
                this.updateVisibility();

                var handler = function (e) {
                    listView.removeEventListener("contentanimating", handler, false);
                    e.preventDefault();
                }

                if (this.isSingleColumn()) {
                    listView.selection.clear();
                    if (listSelectionIndex >= 0) {
                        // If the app has snapped into a single-column detail view,
                        // add the single-column list view to the backstack.
                        nav.history.current.state = {
                            selectedIndex: listSelectionIndex
                        };
                        nav.history.backStack.push({
                            location: "/pages/lists/libraryLists.html",
                            state: {}
                        });
                        element.querySelector(".listContentSection").focus();
                    } else {
                        listView.addEventListener("contentanimating", handler, false);
                        if (firstVisible >= 0 && listView.itemDataSource.list.length > 0) {
                            listView.indexOfFirstVisible = firstVisible;
                        }
                        listView.forceLayout();
                    }

                } else {
                    // If the app has unsnapped into the two-column view, remove any
                    // splitPage instances that got added to the backstack.
                    if (nav.canGoBack && nav.history.backStack[nav.history.backStack.length - 1].location === "/pages/lists/libraryLists.html") {
                        nav.history.backStack.pop();
                    }
                    if (viewState !== lastViewState) {
                        listView.addEventListener("contentanimating", handler, false);
                        if (firstVisible >= 0 && listView.itemDataSource.list.length > 0) {
                            listView.indexOfFirstVisible = firstVisible;
                        }
                        listView.forceLayout();
                    }

                    listView.selection.set(listSelectionIndex >= 0 ? listSelectionIndex : Math.max(firstVisible, 0));

                }

            }
        },

        isSingleColumn: function () {
            return (screen.width < 400);
        },

        updateVisibility: function () {
            var oldPrimary = document.querySelector(".primarycolumn");
            if (oldPrimary) {
                utils.removeClass(oldPrimary, "primarycolumn");
            }
            if (this.isSingleColumn()) {
                if (listSelectionIndex >= 0) {
                    utils.addClass(document.querySelector(".listContentSection"), "primarycolumn");
                    document.querySelector(".listContentSection").focus();
                } else {
                    utils.addClass(document.querySelector(".listViewSection"), "primarycolumn");
                    document.querySelector(".primary-listview").focus();
                }
            } else {
                document.querySelector(".primary-listview").focus();
            }
        }

    });
})();


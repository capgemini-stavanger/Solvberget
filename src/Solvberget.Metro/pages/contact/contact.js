(function () {
    "use strict";

    var ui = WinJS.UI;

    ui.Pages.define("/pages/contact/contact.html", {

        ready: function (element, options) {
            this.ajaxGetContactInformation();
            document.getElementById("appBar").addEventListener("beforeshow", setAppbarButton());
        },
        
        ajaxGetContactInformation: function () {
            var url = window.Data.serverBaseUrl + "/info/contact"; 
            Solvberget.Queue.QueueDownload("contact", { url: url }, this.ajaxGetContantInformationCallback, this, true);
        },

        ajaxGetContantInformationCallback: function (request, context) {
            var response = request.responseText == "" ? "" : JSON.parse(request.responseText);
            if (response != undefined && response !== "")
                context.populateContact(response);
            $(".contact-info .content").fadeIn("slow");
            $("#contact-info-loading").hide();
        },

        populateContact: function (response) {

            var contactTemplateDiv = document.getElementById("contact-info-template");
            var contactTemplateHolder = document.getElementById("contact-information-template-holder");

            var contactTemplate = undefined;
            if (contactTemplateDiv)
                contactTemplate = new WinJS.Binding.Template(contactTemplateDiv);

            var model;
            if (response) {
                for (var i = 0; i < response.length; i++) {
                    model = response[i];
                    if (contactTemplate && contactTemplateHolder && model)
                        contactTemplate.render(model, contactTemplateHolder).done($.proxy(function () {

                            if (this.contactPersons) {
                                var contactPersonsHtml = "";
                                var person;
                                for (var j = 0; j < this.contactPersons.length; j++) {
                                    person = this.contactPersons[j];
                                    contactPersonsHtml += "<div class=\"contact-person\">";

                                    if (person.position) contactPersonsHtml += "<b>" + person.position + ":</b> ";
                                    if (person.name) {
                                        if (person.email) {
                                            contactPersonsHtml += "<a href=\"mailto:" + person.email + "\">" + person.name + "</a><br />";
                                        }
                                        else {
                                            contactPersonsHtml += person.name + "<br />";
                                        }
                                    }
                                    if (person.phone) contactPersonsHtml += "Telefon: " + person.phone;
                                    contactPersonsHtml += "</div>";

                                }

                                $(".contact-persons-holder:last").html(contactPersonsHtml);
                            }

                            if (this.genericFields) {
                                var genericFieldsHtml = "";
                                for (var field in this.genericFields) {
                                    genericFieldsHtml += "<div class=\"contact-info-item\">" + this.genericFields[field] + "</div>";
                                }

                                $(".generic-fields-holder:last").html(genericFieldsHtml);

                            }

                            var colorIndex;
                            if (i % 2) colorIndex = 5;
                            else colorIndex = 3;
                            $(".contact-tile:last").css("background-color", Data.getColorFromPool(colorIndex, "1.0"));

                        }, model));
                    
                }
            }

        },        

        unload: function () {
            Solvberget.Queue.CancelQueue('contact');
        }
        
    });

    WinJS.Namespace.define("Solvberget.ContactConverters", {

        phoneConverter: WinJS.Binding.converter(function (phone) {
            return (phone == undefined || phone === "") ? "" : "<b>Telefon: </b>" + phone;
        }),
        
        faxConverter: WinJS.Binding.converter(function (fax) {
            return (fax == undefined || fax === "") ? "" : "<b>Fax: </b>" + fax;
        }),
        
        visitingAddressConverter: WinJS.Binding.converter(function (address) {
            return (address == undefined || address === "") ? "" : "<b>Besøksadresse: </b>" + address;
        }),
        
        postAddressConverter: WinJS.Binding.converter(function (address) {
            return (address == undefined || address === "") ? "" : "<b>Postsadresse: </b>" + address;
        })

    });

})();
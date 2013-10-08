﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Lucene.Net.Index;
using Solvberget.Domain.Implementation;
using Solvberget.Service.Infrastructure;

namespace Solvberget.Service
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "GetList", // Route name
                "List/GetLists/{limit}", // URL with parameters
                new { controller = "List", action = "GetLists", limit = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "GetUserInformation", // Route name
                "User/GetUserInformation/{userId}/{verification}", // URL with parameters
                new { controller = "User", action = "GetUserInformation", verification = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "SpellingDictionaryLookup", // Route name
                "Document/SpellingDictionaryLookup/{value}", // URL with parameters
                new { controller = "Document", action = "SpellingDictionaryLookup" } // Param defaults
            );

            routes.MapRoute(
                "GetDocumentThumbnailRoute", // Route name
                "Document/GetDocumentThumbnailRoute/{id}/{size}", // URL with parameters
                new { controller = "Document", action = "Index", id = UrlParameter.Optional, size = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "CancelReservation", // Route name
                "Document/CancelReservation/{documentItemNumber}/{documentItemSequence}/{cancellationSequence}", // URL with parameters
                new { controller = "Document", action = "CancelReservation" } // Parameter defaults
            );


            routes.MapRoute(
                "RequestReservation", // Route name
                "Document/RequestReservation/{documentid}/{userId}/{branch}", // URL with parameters
                new { controller = "Document", action = "RequestReservation" } // Parameter defaults
            );


            routes.MapRoute(
                "GetBlogWithEntries", // Route name
                "Blog/GetBlogWithEntries/{blogId}", // URL with parameters
                new { controller = "Blog", action = "GetBlogWithEntries" } // Parameter defaults
            );

            routes.MapRoute(
                "RequestPinCodeToSms", // Route name
                "User/RequestPinCodeToSms/{userId}", // URL with parameters
                new { controller = "User", action = "RequestPinCodeToSms", verification = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Document", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }
    
    }
}
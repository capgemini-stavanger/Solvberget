using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Nancy;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Events;
using Solvberget.Domain.Utils;
using Solvberget.Nancy.Mapping;

namespace Solvberget.Nancy.Modules
{
    public class EventModule : NancyModule
    {
        public EventModule(IEnvironmentPathProvider env) : base("/events")
        {
            var events = new List<EventDto>();

            var client = new WebClient();
            client.Encoding = Encoding.UTF8;

            var organizerId = ConfigurationManager.AppSettings["TicketCoOrganizerId"];
            var apiToken = ConfigurationManager.AppSettings["TicketCoApiToken"];

            try
            {
                var eventsJson = client.DownloadString(new Uri(
                String.Format("https://ticketco.no/api/public/v1/events?organizer_id={0}&token={1}", organizerId,
                    apiToken)));

                var serializer = new JsonSerializer();
                serializer.Culture = new CultureInfo("nb-no");

                var ticketCoEvents = serializer.Deserialize<TicketCoResult>(new JsonTextReader(new StringReader(eventsJson)));

                foreach (var element in ticketCoEvents.events)
                {
                    var ev = new EventDto();
                    events.Add(ev);

                    ev.Id = element.mobile_link.GetHashCode();
                    ev.Name = element.title;
                    ev.Description = element.description;
                    ev.ImageUrl = element.image.iphone2x.url;
                    ev.Location = element.location_name;
                    ev.Start = element.start_at;
                    ev.End = element.end_at;
                    ev.TicketPrice = element.ticket_price;
                    ev.TicketUrl = element.mobile_link;
                }
            }
            catch
            {
            }

            // todo: implement after new events integration in place
            Get["/"] = _ => events.OrderBy(ev => ev.Start).ToArray();

            Get["/{id}"] = args => events.FirstOrDefault(ev => ev.Id == args.id);
        }

        public class TicketCoResult
        {
            public TicketCoEventResult[] events { get; set; }
        }

        public class TicketCoEventResult
        {
            public double ticket_price { get; set; } // missing?
            public string title { get; set; }
            public string description { get; set; }
            public string location_name { get; set; }
            public string street_address { get; set; }
            public DateTime start_at { get; set; }
            public DateTime end_at { get; set; }
            public TicketCoEventImage image { get; set; }
            
            public string desktop_link { get; set; }
            public string mobile_link { get; set; }
        }

        public class TicketCoEventImage
        {
            public string url { get; set; }
            public TicketCoEventImage @default { get; set; }
            public TicketCoEventImage iphone2x { get; set; }
        }
    }
}
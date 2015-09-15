using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Nancy;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Utils;
using Ganss.XSS;

namespace Solvberget.Nancy.Modules
{
    public class EventModule : NancyModule
    {
        public EventModule(IEnvironmentPathProvider env) : base("/events")
        {
            var events = DownloadEvents();

            // todo: implement after new events integration in place
            Get["/"] = _ => events.OrderBy(ev => ev.Start).ToArray();

            Get["/{id}"] = args => events.FirstOrDefault(ev => ev.Id == args.id);

            Get["/ind-export/{org}/{id}"] = args =>
            {
                EventDto[] selection;

                List<EventDto> evs = DownloadEvents(args.Org);

                string name = "arrangementer-org-" + args.Org;

                if (args.id == "all")
                {
                    name += "-all.xml";
                    selection = evs.OrderBy(ev => ev.Start).ToArray();
                }
                else
                {
                    name = string.Format(name+"-id-{0}.xml", args.id);
                    selection = evs.Where(ev => ev.TicketCoId == args.id).ToArray();
                }

                string xml = new InDesignXmlBuilder().Build(selection);

                return Response.AsText(xml).AsAttachment(name, "text/xml; charset=utf-8");

            };
        }

        private static List<EventDto> DownloadEvents(string organizerId = null)
        {
            var events = new List<EventDto>();

            var client = new WebClient();
            client.Encoding = Encoding.UTF8;

            organizerId = organizerId ?? ConfigurationManager.AppSettings["TicketCoOrganizerId"];
            var apiToken = ConfigurationManager.AppSettings["TicketCoApiToken"];


            try
            {
                var eventsJson = client.DownloadString(new Uri(
                    String.Format("https://ticketco.no/api/public/v1/events?organizer_id={0}&token={1}", organizerId,
                        apiToken)));

                var serializer = new JsonSerializer();
                serializer.Culture = new CultureInfo("nb-no");

                var ticketCoEvents = serializer.Deserialize<TicketCoResult>(new JsonTextReader(new StringReader(eventsJson)));

                var sanitizer = new HtmlSanitizer();

                foreach (var element in ticketCoEvents.events)
                {
                    var ev = new EventDto();
                    events.Add(ev);

                    ev.TicketCoId = element.id;
                    ev.Id = element.mobile_link.GetHashCode();
                    ev.Name = element.title;
                    ev.Description = sanitizer.Sanitize(element.description);
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
            return events;
        }

        public class InDesignXmlBuilder
        {

            /*
             * InDesign crazy xml format:
             * 
             * <Root>
             * <arr_description>
             * 
             * <!-- event #1 -->
             * <nest_arr_place>$location</nest_arr_place>
             * <arr_tittel>$title</arr_tittel>
             * <arr_description>$description</arr_description>
             * 
             * <!-- event #2... -->
             * <!-- event #n... -->
             *  * 
             * </arr_description>
             * 
             * <!-- time info for event #1 -->
             * <arr_description>
             * <nest_dato_box_right>
             * <nest_dag>$day_name </nest_dag><nest_dato>$day_num</nest_dato>
             * $time
             * </nest_dato_box_right>
             * <arr_description>
             * <mnd_box>$month_name</mnd_box>
             * </arr_description>
             * 
             * <!-- time info event #2... ->
             * <!-- time info event #n... ->
             * 
             * </Root>

            */

            public string Build(EventDto[] events)
            {
                // xml format for In-Design is a bit.. weird, so lets just build it using a StringBuilder...
                var root = new StringBuilder();

                root.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                root.AppendLine("<Root>");
                root.AppendLine("<arr_description>");
                
                foreach (var ev in events)
                {
                    root.AppendFormat("<nest_arr_place>{0}</nest_arr_place>", ev.Location);
                    root.AppendLine();
                    root.AppendFormat("<arr_tittel>{0}</arr_tittel>", ev.Name);
                    root.AppendLine();
                    root.AppendFormat("<arr_description>{0}</arr_description>", ev.Description);
                    root.AppendLine();
                }

                root.AppendLine("</arr_description>");

                foreach (var ev in events)
                {
                    string dayName = ev.Start.ToString("dddd");
                    int dayNum = ev.Start.Day;
                    string time = ev.Start.ToString("HH.mm");

                    root.AppendLine("<arr_description>");
                    root.AppendLine("<nest_dato_box_right>");
                    root.AppendFormat("<nest_dag>{0} </nest_dag><nest_dato>{1}</nest_dato>", dayName, dayNum);
                    root.AppendLine();
                    root.AppendLine(time);
                    root.AppendLine("</nest_dato_box_right>");
                    root.AppendLine("</arr_description>");
                }

                root.AppendLine("</Root>");

                return root.ToString();
            }
        }

        public class TicketCoResult
        {
            public TicketCoEventResult[] events { get; set; }
        }

        public class TicketCoEventResult
        {
            public string id;
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

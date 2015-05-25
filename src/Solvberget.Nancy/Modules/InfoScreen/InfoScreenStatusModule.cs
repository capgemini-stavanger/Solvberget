using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.ModelBinding;
using Nancy.ViewEngines.SuperSimpleViewEngine;
using Newtonsoft.Json;
using Solvberget.Domain.Utils;
using HttpUtility = Nancy.Helpers.HttpUtility;

namespace Solvberget.Nancy.Modules
{
    public class InfoScreenStatusModule : NancyModule
    {
        private readonly IRootPathProvider path;
        private static Dictionary<string, ScreenStatus> _statuses;

        public Dictionary<string, ScreenStatus> Statuses
        {
            get
            {
                if (null == _statuses)
                {
                    string file = HttpContext.Current.Server.MapPath("~/screenlog.json");
                    _statuses = File.Exists(file) ? JsonConvert.DeserializeObject<Dictionary<string, ScreenStatus>>(File.ReadAllText(file)) : new Dictionary<string, ScreenStatus>();
                }

                return _statuses;
            }
            set
            {
                _statuses = value;
            }
        }

        private void SaveStatuses()
        {
            string file = HttpContext.Current.Server.MapPath("~/screenlog.json");
            File.WriteAllText(file, JsonConvert.SerializeObject(Statuses, Formatting.Indented));
        }

        public InfoScreenStatusModule(IEnvironmentPathProvider pathProvider)
            : base("/screenstatus")
        {
            Post["/{screenId}"] = args =>
            {
                // log screen status
                var update = this.Bind<ScreenStatusUpdate>();

                ScreenStatus status;

                if (!Statuses.TryGetValue(args.screenId, out status))
                {
                    status = new ScreenStatus {ScreenId = args.screenId};
                    Statuses[args.screenId] = status;
                }

                status.LastTemplate = update.Template ?? status.LastTemplate;
                status.Heartbeat = DateTimeOffset.UtcNow;

                if (!String.IsNullOrEmpty(update.Error))
                {
                    status.Log.Insert(0, DateTimeOffset.UtcNow + ": " + update.Error);
                    status.Log = status.Log.Take(25).ToList(); // only keep last 25 entries
                }

                SaveStatuses();
                
                return Response.AsJson(new{});
            };

            Get["/"] = args =>
            {
                var renderer = new SuperSimpleViewEngine();

                var template =

@"<html><head></head>
<style>
    body
    {
        font-family:Helvetica, Arial;
        font-size:15px;
    }

    table td
    {
        padding:8px;
        padding-right:16px;
        border-bottom: solid 1px #cecece;
        margin:0px;
    }

    thead td
    {
        font-weight:bold;
    }

</style>

<body>
<h1>Screen status report</h1>    
<table>
<tbody>
<thead>
    <tr>
        <td>Screen Name</td>
        <td>Screen ID</td>
        <td>Currently showing</td>
        <td>Last heartbeat</td>
        <td>Config</td>
    </tr>
</thead>

@Each.Statuses
<tr>
    <td>@Current.ScreenName</td>
    <td>@Current.ScreenId</td>
    <td>@Current.LastTemplate</td>
    <td>
        @If.MaybeDead
            <span style='background:#FFCC33'>@Current.TimeSinceLastHeartbeatString</span>
        @EndIf
        @IfNot.MaybeDead
            <span>@Current.TimeSinceLastHeartbeatString</span>
        @EndIf
    </td>
    <td>@Current.ConfigSummary</td>
</tr>

@EndEach

</tbody>
</table>  
<div style='margin-top:20px; font-size:80%; color:gray'>
    Screens will send a heartbeat every ~1-5 minutes, depending on the screen configuration. Screen that do send a heartbeat for ~10 minutes will be marked with yellow.
    <br/>
    This page auto-refreshes every ~10 seconds. Last refresh @Model.RenderTimeString.
</div>

<script type='text/javascript'>

setTimeout(function () {
        window.location.reload();
    }, 10000);

</script>

</body></html>";

                var cfg = SlideConfigModule.GetSlideConfigurationsFromFile(pathProvider);

                var model = new StatusModel();

                foreach (var s in cfg.OrderBy(c => c.Name))
                {
                    ScreenStatus status;

                    if (!Statuses.TryGetValue(s.Id, out status))
                    {
                        status = new ScreenStatus {ScreenId = s.Id, Heartbeat = DateTimeOffset.MinValue};
                    }

                    status.ScreenName = s.Name;
                    status.ConfigSummary = String.Join(", ", s.Config.Select(c => c.Template));
                    model.Statuses.Add(status);
                }

                var html = renderer.Render(template, model, new FakeViewEngineHost());
                return Response.AsText(html, "text/html");
            };
        }

        public class FakeViewEngineHost : IViewEngineHost
        {
            public Func<string, object, string> GetTemplateCallback { get; set; }
            public Func<string, string> ExpandPathCallBack { get; set; }

            public FakeViewEngineHost()
            {
                this.Context = new FakeContext { Name = "Frank" };
            }

            /// <summary>
            /// Html "safe" encode a string
            /// </summary>
            /// <param name="input">Input string</param>
            /// <returns>Encoded string</returns>
            public string HtmlEncode(string input)
            {
                return input.Replace("&", "&amp;").
                    Replace("<", "&lt;").
                    Replace(">", "&gt;").
                    Replace("\"", "&quot;");
            }

            /// <summary>
            /// Get the contenst of a template
            /// </summary>
            /// <param name="templateName">Name/location of the template</param>
            /// <param name="model">Model to use to locate the template via conventions</param>
            /// <returns>Contents of the template, or null if not found</returns>
            public string GetTemplate(string templateName, object model)
            {
                return this.GetTemplateCallback != null ? this.GetTemplateCallback.Invoke(templateName, model) : string.Empty;
            }

            /// <summary>
            /// Gets a uri string for a named route
            /// </summary>
            /// <param name="name">Named route name</param>
            /// <param name="parameters">Parameters to use to expand the uri string</param>
            /// <returns>Expanded uri string, or null if not found</returns>
            public string GetUriString(string name, params string[] parameters)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Expands a path to include any base paths
            /// </summary>
            /// <param name="path">Path to expand</param>
            /// <returns>Expanded path</returns>
            public string ExpandPath(string path)
            {
                return this.ExpandPathCallBack != null ? this.ExpandPathCallBack.Invoke(path) : path;
            }

            public string AntiForgeryToken()
            {
                return "CSRF";
            }

            private class FakeContext
            {
                public FakeContext()
                {
                    this.User = new FakeUser { Username = "Frank123" };
                }

                public string Name { get; set; }

                public FakeUser User { get; set; }

                public class FakeUser
                {
                    public string Username { get; set; }
                }
            }

            /// <summary>
            /// Context object of the host application.
            /// </summary>
            /// <value>An instance of the context object from the host.</value>
            public object Context { get; set; }
        }


    }
}
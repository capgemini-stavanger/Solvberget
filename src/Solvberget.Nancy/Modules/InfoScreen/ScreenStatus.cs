using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Solvberget.Nancy.Modules
{
    public class ScreenStatus
    {
        public ScreenStatus()
        {
            Log = new List<string>();
        }

        public string ScreenId { get; set; }
        public DateTimeOffset Heartbeat { get; set; }

        [JsonIgnore]
        public TimeSpan TimeSinceLastHeartbeat { get { return (DateTimeOffset.UtcNow - Heartbeat); }}

        [JsonIgnore]
        public string TimeSinceLastHeartbeatString
        {
            get
            {
                if (Heartbeat == DateTimeOffset.MinValue) return "never";
                var timeAgo = TimeSinceLastHeartbeat;

                int num = 0;
                var unit = "day";
                
                if (timeAgo.TotalMinutes < 1)
                {
                    num = (int) timeAgo.TotalSeconds;
                    unit = "second";
                }
                else if (timeAgo.TotalHours < 1)
                {
                    num = (int) timeAgo.TotalMinutes;
                    unit = "minute";

                }
                else if (timeAgo.TotalDays < 1)
                {
                    num = (int) timeAgo.TotalHours;
                    unit = "hour";
                }
                else
                {
                    num = (int) timeAgo.TotalDays;
                }

                return String.Format("{0} {1}{2} ago", num, unit, num > 1 ? "s" : "");
            }
        }

        [JsonIgnore]
        public bool MaybeDead { get { return TimeSinceLastHeartbeat > TimeSpan.FromMinutes(10); } }

        public string LastTemplate { get; set; }

        public List<string> Log { get; set; }

        public string ScreenName { get; set; }
        public string ConfigSummary { get; set; }
    }
}
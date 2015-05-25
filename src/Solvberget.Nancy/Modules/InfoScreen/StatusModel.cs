using System;
using System.Collections.Generic;

namespace Solvberget.Nancy.Modules
{
    public class StatusModel
    {
        public StatusModel()
        {
            Statuses = new List<ScreenStatus>();
        }

        public string RenderTimeString
        {
            get { return DateTime.Now.ToString("F"); }
        }

        public List<ScreenStatus> Statuses { get; set; }
    }
}
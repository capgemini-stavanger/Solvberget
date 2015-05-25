using System.Collections.Generic;

namespace Solvberget.Core.DTOs
{
    public class ScreenConfigDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public SlideConfigDto[] Config { get; set; }
    }

    public class SlideConfigDto
    {
        public string Template { get; set; }
        public int Duration { get; set; }

        public Dictionary<string, string> SlideOptions { get; set; }
    }
}
using System;

namespace Solvberget.Core.DTOs
{
    public class NewsStoryDto
    {
        public string Title { get; set; }
        public string Ingress { get; set; }
        public DateTime Published { get; set; }
        public string Link { get; set; }
    }
}

﻿namespace Solvberget.Core.DTO
{
    public class DocumentDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Type { get; set; }
        public DocumentAvailabilityDto Availability { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solvberget.Core.DTOs
{
    public class DocumentRatingDto
    {
        public string SourceUrl { get; set; }
        public double Score { get; set; }
        public double MaxScore { get; set; }
        public string Source { get; set; }
    }
}

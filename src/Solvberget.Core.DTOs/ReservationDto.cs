using System;
using System.Collections.Generic;

namespace Solvberget.Core.DTOs
{
    public class ReservationDto : RequestReplyDto
    {
        public DocumentDto Document { get; set; }

        public DateTime Reserved { get; set; }

        public bool ReadyForPickup { get; set; }

        public DateTime? PickupDeadline { get; set; }

        // v1.2
        public string PickupDeadlineString { get; set; }
        public string ReservedString { get; set; }

        public string PickupLocation { get; set; }
    }
}

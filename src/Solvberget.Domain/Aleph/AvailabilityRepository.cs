using Solvberget.Domain.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Solvberget.Domain.Aleph
{
    public class AvailabilityRepository
    {
        public static List<AvailabilityInformation> GenerateLocationAndAvailabilityInfo(IEnumerable<DocumentItem> docItems, Document doc)
        {
            var items = docItems.ToList();
            if (!items.Any()) return null;
            var availabilityinfo = new List<AvailabilityInformation>();

            foreach (var branchToHandle in AvailabilityInformation.BranchesToHandle)
            {
                var availabilitesForBranch = GenerateInfoFor(doc, branchToHandle, items);
                if (availabilitesForBranch == null) continue;
                availabilityinfo.AddRange(availabilitesForBranch);
            }

            return availabilityinfo;
        }

        private static List<AvailabilityInformation> GenerateInfoFor(Document doc, string branch, IEnumerable<DocumentItem> docItems)
        {
            var availabilityInformation = FillProperties(doc, branch, docItems);

            if (!availabilityInformation.Any()) return null;
            return availabilityInformation.FirstOrDefault()?.Branch != null ? availabilityInformation : null;
        }

        private static List<AvailabilityInformation> FillProperties(Document doc, string branch, IEnumerable<DocumentItem> docItems)
        {
            var aiList = new List<AvailabilityInformation>();

            var items = docItems.Select(x => x).Where(x => x.Branch.Equals(branch) && x.IsReservable).ToList();
            var itemsInDepartment = items.GroupBy(x => x.Department).ToList();

            foreach (var dep in itemsInDepartment)
            {
                var locations = dep.GroupBy(x => x.PlacementCode).ToList();
                foreach (var location in locations)
                {
                    var docItem = location.FirstOrDefault();
                    if (docItem != null)
                    {
                        var availability = new AvailabilityInformation
                        {
                            Branch = branch,
                            Department = docItem.Department,
                            PlacementCode = docItem.PlacementCode,
                            TotalCount = location.Count(),
                            AvailableCount = location.Count(x => x.LoanStatus == null && !x.OnHold && !InUnavailableState(x))
                        };

                        // Awful last-minute hack..
                        if (branch == "Madla" && availability.Department == "2. etasje Barn og ungdom")
                        {
                            availability.Department = "Barneavdelingen";
                        }


                        if (availability.AvailableCount == 0)
                        {
                            var dueDates = items.Where(x => x.LoanDueDate != null).Select(x => x.LoanDueDate.Value);
                            if (dueDates.Any())
                            {
                                var earliestDueDate = dueDates.OrderBy(x => x).FirstOrDefault();

                                if (!items.Any(x => x.NoRequests > 0))
                                {
                                    if (earliestDueDate.CompareTo(DateTime.Now) < 0)
                                    {
                                        // The due date has passed, but the document is not handed in yet. Set to next day.
                                        availability.EarliestAvailableDateFormatted = DateTime.Now.AddDays(1).ToShortDateString();
                                    }
                                    else
                                    {
                                        availability.EarliestAvailableDateFormatted = earliestDueDate.ToShortDateString();
                                    }
                                }
                                else
                                {
                                    var totalNumberOfReservations = items.Sum(x => x.NoRequests);
                                    var calculation1 = (totalNumberOfReservations * (doc.StandardLoanTime + AvailabilityInformation.AveragePickupTimeInDays));
                                    // Below for added days: if it is required to round up the result of dividing m by n 
                                    // (where m and n are integers), one should compute (m+n-1)/n
                                    // Source: Number Conversion, Roland Backhouse, 2001
                                    var calculation2 = (calculation1 + availability.TotalCount - 1) / availability.TotalCount;

                                    if (availability.TotalCount == 1)
                                        availability.EarliestAvailableDateFormatted = earliestDueDate.AddDays(calculation2).ToShortDateString();
                                    else
                                        availability.EarliestAvailableDateFormatted = earliestDueDate.AddDays(calculation2 + doc.StandardLoanTime).ToShortDateString();

                                }
                            }
                            else
                            {
                                availability.EarliestAvailableDateFormatted = "Ukjent";
                            }
                        }
                        else
                        {
                            availability.EarliestAvailableDateFormatted = "";
                        }

                        aiList.Add(availability);
                    }
                }
            }

            return aiList;
        }

        private static bool InUnavailableState(DocumentItem documentItem)
        {
            var unavailableStates = new[] { "Ikke mottatt", "Mottatt" };
            return unavailableStates.Contains(documentItem.ItemProcessStatusText);
        }

        public static string GenerateEstimatedAvailableDate(IEnumerable<DocumentItem> docItems, Document document)
        {
            var items = docItems.Select(x => x).Where(x => x.IsReservable).ToList();
            var availableCount = items.Count(x => x.LoanStatus == null && !x.OnHold && !InUnavailableState(x));
            if (availableCount > 0) return "";

            var dueDates = items.Where(doc => doc.LoanDueDate != null).Select(doc => doc.LoanDueDate.Value).ToList();
            if (!dueDates.Any()) return "Ukjent";
            {
                var earliestDueDate = dueDates.OrderBy(date => date).FirstOrDefault();

                if (!items.Any(doc => doc.NoRequests > 0))
                {
                    return earliestDueDate.CompareTo(DateTime.Now) < 0
                        ? DateTime.Now.AddDays(1).ToShortDateString()
                        : earliestDueDate.ToShortDateString();
                }

                var totalNumberOfReservations = items.Sum(x => x.NoRequests);
                var calculation1 = totalNumberOfReservations * (document.StandardLoanTime + AvailabilityInformation.AveragePickupTimeInDays);
                var calculation2 = (calculation1 + items.Count - 1) / items.Count;

                return items.Count == 1
                    ? earliestDueDate.AddDays(calculation2).ToShortDateString()
                    : earliestDueDate.AddDays(calculation2 + document.StandardLoanTime).ToShortDateString();
            }
        }
    }
}

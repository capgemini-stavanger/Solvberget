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
                var distinctDepartments = new List<string>();
                foreach (var availabilityInformation in availabilitesForBranch)
                {
                    if (distinctDepartments.Contains(availabilityInformation.Department)) continue;

                    distinctDepartments.Add(availabilityInformation.Department);
                    availabilityinfo.Add(availabilityInformation);
                }
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

            foreach (var documentItem in items)
            {
                var availability = new AvailabilityInformation
                {
                    Branch = branch,
                    Department = documentItem.Department,
                    PlacementCode = items.FirstOrDefault().PlacementCode,
                    TotalCount = items.Count,
                    AvailableCount = items.Count(x => x.LoanStatus == null && !x.OnHold && !InUnavailableState(x))
                };


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

            return aiList;
        }

        private static bool InUnavailableState(DocumentItem documentItem)
        {
            var unavailableStates = new[] { "Ikke mottatt", "Mottatt" };
            return unavailableStates.Contains(documentItem.ItemProcessStatusText);
        }
    }
}

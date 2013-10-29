using System.Collections.Generic;

namespace Solvberget.Domain.Static
{
    public interface IInformationRepository
    {
        List<OpeningHoursInformation> GetOpeningHoursInformation();
        List<ContactInformation> GetContactInformation();
    }
}
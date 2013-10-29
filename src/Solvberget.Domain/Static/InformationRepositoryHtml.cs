using System.Collections.Generic;

namespace Solvberget.Domain.Static
{
    public class InformationRepositoryHtml : IInformationRepository
    {

        public List<OpeningHoursInformation> GetOpeningHoursInformation()
        {
            var webpage = new OpeningHoursWebPage();
            webpage.FillProperties();
            return webpage.OpeningHoursInformationList;
        }

        public List<ContactInformation> GetContactInformation()
        {
            var webpage = new ContactWebPage();
            webpage.FillProperties();
            return webpage.ContactInformationList;
        }
    }
}

using Nancy;
using Solvberget.Domain.Static;

namespace Solvberget.Nancy.Modules
{
    public class InformationModule : NancyModule
    {
        public InformationModule(IInformationRepository informationRepository) : base("/info")
        {
            Get["/contact"] = _ => informationRepository.GetContactInformation();

            Get["/opening-hours"] = _ => informationRepository.GetOpeningHoursInformation();
        }
    }
}
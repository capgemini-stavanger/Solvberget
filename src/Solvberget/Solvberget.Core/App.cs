using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Solvberget.Core.Properties;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels;
using System.Reflection;

namespace Solvberget.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes(typeof(SearchService).GetTypeInfo().Assembly) // Solvberget.Core.Services assembly
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.LazyConstructAndRegisterSingleton<IStringDownloader, HttpBodyDownloader>();
            Mvx.LazyConstructAndRegisterSingleton<DtoDownloader, DtoDownloader>();
            Mvx.LazyConstructAndRegisterSingleton<IServiceUrls, ServiceUrlsFromResources>();
            RegisterAppStart<HomeViewModel>();
        }
    }

    public class ServiceUrlsFromResources : IServiceUrls
    {
        public string ServiceUrl { get { return Resources.ServiceUrl; } }
        public string ServiceUrl_Search { get { return Resources.ServiceUrl_Search; } }
        public string ServiceUrl_Document { get { return Resources.ServiceUrl_Document; } }
        public string ServiceUrl_Rating { get { return Resources.ServiceUrl_Rating; } }
        public string ServiceUrl_Review { get { return Resources.ServiceUrl_Review; } }
        public string ServiceUrl_Events { get { return Resources.ServiceUrl_Events; } }
        public string ServiceUrl_Event { get { return Resources.ServiceUrl_Event; } }
        public string ServiceUrl_MediaImage { get { return Resources.ServiceUrl_MediaImage; } }
    }
}



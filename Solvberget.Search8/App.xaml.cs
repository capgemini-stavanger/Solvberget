using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Search8.Pages;

namespace Solvberget.Search8
{
    sealed partial class App : IServiceUrls
    {
        private WinRTContainer _container;

        public App()
        {
            LogManager.GetLog = type => new DebugLog(type);

            InitializeComponent();
        }

        protected override void Configure()
        {
            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

            _container.PerRequest<SearchViewModel>();
            _container.PerRequest<ResultsViewModel>();

            _container.Instance<IServiceUrls>(this);

            _container.PerRequest<DtoDownloader>();
            _container.PerRequest<IUserAuthenticationDataService, NoAuthentication>();
            _container.PerRequest<IStringDownloader, HttpBodyDownloader>();
            _container.PerRequest<ISearchService, SearchService>();
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new Exception("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootView<SearchView>();
        }

        public string ServiceUrl { get { return "http://solvbergetapp.cloudapp.net/api"; } }
        public string ServiceUrl_Search { get { return "/documents/search?query={0}"; } }
        public string ServiceUrl_Document { get { return "/documents/{0}"; } }
        public string ServiceUrl_Rating { get { return "/documents/{0}/rating"; } }
        public string ServiceUrl_Review { get { return "/documents/{0}/review"; } }
        public string ServiceUrl_Events { get { return null; } }
        public string ServiceUrl_Event { get { return null; } }
        public string ServiceUrl_MediaImage { get { return "/documents/{0}/thumbnail"; } }
    }

    internal class NoAuthentication : IUserAuthenticationDataService
    {
        public bool UserInfoRegistered()
        {
            return false;
        }

        public string GetUserId()
        {
            return null;
        }

        public string GetUserPassword()
        {
            return null;
        }

        public void SetUser(string userId)
        {

        }

        public void SetPassword(string password)
        {
        }

        public void RemoveUser()
        {
        }

        public void RemovePassword()
        {
        }
    }
}

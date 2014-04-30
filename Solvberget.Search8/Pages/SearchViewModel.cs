using System;
using Caliburn.Micro;

namespace Solvberget.Search8.Pages
{
    public class SearchViewModel : Screen
    {
        private readonly INavigationService _navigation;

        private string _query;

        public SearchViewModel(INavigationService navigation)
        {
            _navigation = navigation;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        public string Query
        {
            get { return _query; }
            set
            {
                _query = value;
                NotifyOfPropertyChange("Query");
            }
        }
        
        public async void Search()
        {
            if (String.IsNullOrEmpty(Query))
            {
                return;
            }

            _navigation
                .UriFor<ResultsViewModel>()
                .WithParam(r => r.Query, Query)
                .Navigate();
        }
    }
}
﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class SearchViewModel : BaseViewModel 
    {
        private readonly ISearchService _searchService;

        public SearchViewModel(ISearchService searchService)
        {
            _searchService = searchService;
            Title = "Søk";
            SearchAndLoad();
        }

        private string _query;
        public string Query 
        {
            get { return _query; }
            set { _query = value; RaisePropertyChanged(() => Query);}
        }

        private IEnumerable<SearchResultViewModel> _results;
        public IEnumerable<SearchResultViewModel> Results 
        {
            get { return _results; }
            set { _results = value; RaisePropertyChanged(() => Results);}
        }

        private MvxCommand<SearchResultViewModel> _showDetailsCommand;
        public ICommand ShowDetailsCommand
        {
            get
            {
                return _showDetailsCommand ?? (_showDetailsCommand = new MvxCommand<SearchResultViewModel>(ExecuteShowDetailsCommand));
            }
        }

        private void ExecuteShowDetailsCommand(SearchResultViewModel searchResultViewModel)
        {
            ShowViewModel<SearchResultViewModel>(searchResultViewModel);
        }

        // Loads a a set of Documents retrieved from the service into the results list.
        public void SearchAndLoad()
        {
            var results = _searchService.Search(Query);
            Results = from document in results
                select new SearchResultViewModel()
                {
                    Name = document.Title
                };
        }
    }
    
    public class SearchResultViewModel : BaseViewModel
    {
        private string _name;
        public string Name 
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name);}
        }
    }
}

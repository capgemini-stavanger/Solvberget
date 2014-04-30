using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Search8.Pages
{
    public class FilterOptionVm
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public Func<DocumentDto, bool> Predicate { get; set; } 

        public bool Filter(DocumentDto document)
        {
            return null == Predicate || Predicate(document);
        }
    }

    public class ResultsViewModel : Screen
    {
        private readonly ISearchService _search;
        private readonly INavigationService _navigation;

        private bool _isSearching;
        private int _resultCount;
        private string _query;
        private FilterOptionVm _selectedFilter;

        public ResultsViewModel(ISearchService search, INavigationService navigation)
        {
            _search = search;
            _navigation = navigation;
            Results = new BindableCollection<DocumentDto>();
            FilterOptions = new BindableCollection<FilterOptionVm>();
        }
        
        private async void DoSearch()
        {
            Results.Clear();
            NotifyOfPropertyChange("FilteredResults");
            
            IsSearching = true;
            var results = (await _search.Search(Query)).ToList();
            ResultCount = results.Count;

            IsSearching = false;

            Results.Clear();
            Results.AddRange(results);
            
            FilterOptions.Clear();
            FilterOptions.Add(new FilterOptionVm { Name = "Alle typer medier", Count = ResultCount });
            FilterOptions.Add(new FilterOptionVm { Name = "Kun bøker", Count = Results.Count(r => r.Type == "Book"), Predicate = doc => doc.Type == "Book" });
            FilterOptions.Add(new FilterOptionVm { Name = "Kun CDer", Count = Results.Count(r => r.Type == "Cd"), Predicate = doc => doc.Type == "Cd" });
            FilterOptions.Add(new FilterOptionVm { Name = "Kun filmer", Count = Results.Count(r => r.Type == "Film"), Predicate = doc => doc.Type == "Film" });
            FilterOptions.Add(new FilterOptionVm { Name = "Kun lydbøker", Count = Results.Count(r => r.Type == "AudioBook"), Predicate = doc => doc.Type == "AudioBook" });
            FilterOptions.Add(new FilterOptionVm { Name = "Kun noter", Count = Results.Count(r => r.Type == "SheetMusic"), Predicate = doc => doc.Type == "SheetMusic" });
            FilterOptions.Add(new FilterOptionVm { Name = "Kun spill", Count = Results.Count(r => r.Type == "Game"), Predicate = doc => doc.Type == "Game" });
            FilterOptions.Add(new FilterOptionVm { Name = "Kun tidskrifter", Count = Results.Count(r => r.Type == "Journal"), Predicate = doc => doc.Type == "Journal" });

            SelectedFilter = FilterOptions.First();
        }

        public BindableCollection<DocumentDto> Results { get; set; }

        public IEnumerable<DocumentDto> FilteredResults
        {
            get
            {
                if (null == SelectedFilter) return Results;
                return Results.Where(SelectedFilter.Filter);
            }
        }

        public BindableCollection<FilterOptionVm> FilterOptions { get; set; }

        public FilterOptionVm SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                NotifyOfPropertyChange("SelectedFilter");
                NotifyOfPropertyChange("FilteredResults");
            }
        }

        public void ShowDetails(DocumentDto document)
        {
            _navigation.UriFor<ResultDetailsViewModel>().WithParam(d => d.DocumentId, document.Id).WithParam(d => d.DocumentTitle, document.Title).Navigate();
        }

        public bool IsSearching
        {
            get { return _isSearching; }
            set
            {
                if (value.Equals(_isSearching)) return;
                _isSearching = value;
                NotifyOfPropertyChange("IsSearching");
            }
        }

        public int ResultCount
        {
            get { return _resultCount; }
            set
            {
                if (value == _resultCount) return;
                _resultCount = value;
                NotifyOfPropertyChange("ResultCount");
            }
        }

        public string Query
        {
            get { return _query; }
            set
            {
                if (value == _query) return;
                _query = value;

                DoSearch();
                NotifyOfPropertyChange("Query");
            }
        }


        public void GoBack()
        {
            _navigation.GoBack();
        }
    }
}
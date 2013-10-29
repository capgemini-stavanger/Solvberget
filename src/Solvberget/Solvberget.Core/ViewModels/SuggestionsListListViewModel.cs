﻿
//This View Model is for the list of LibraryLists, which is the suggestions lists

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class SuggestionsListListViewModel : BaseViewModel
    {
        private readonly ISuggestionsService _suggestionsService;

        public SuggestionsListListViewModel(ISuggestionsService suggestionsService)
        {
            _suggestionsService = suggestionsService;
            Title = "Anbefalinger";
        }

        public void Init()
        {
            Load();
        }

        private List<SuggestionsListViewModel> _lists;
        public List<SuggestionsListViewModel> Lists
        {
            get { return _lists; }
            set { _lists = value; RaisePropertyChanged(() => Lists); }
        }

        private MvxCommand<SuggestionListSummaryViewModel> _showListCommand;
        public ICommand ShowListCommand
        {
            get
            {
                return _showListCommand ?? (_showListCommand = new MvxCommand<SuggestionListSummaryViewModel>(ExecuteShowListCommand));
            }
        }

        private void ExecuteShowListCommand(SuggestionListSummaryViewModel searchResultViewModel)
        {
            ShowViewModel<SuggestionsListViewModel>(searchResultViewModel.Id);
        }


        // Loads a a set of Documents retrieved from the service into the results list.
        public async void Load()
        {
            IsLoading = true;

            Lists = (from n in await _suggestionsService.GetSuggestionsList()
                        select new SuggestionsListViewModel
                           {
                               Name = n.Name,
                               Documents = n.Documents
                           }).ToList();

            IsLoading = false;
        }
    }
}

using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsStore.Views;
using Solvberget.Core.Services.Interfaces;
using CoreViewModel = Solvberget.Core.ViewModels;
using Solvberget.KioskApp.Common;
using Solvberget.KioskApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Solvberget.KioskApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchView : Page, INotifyPropertyChanged
    {

        public SearchViewModel ViewModel { get; set; }


        public SearchView()
        {
            this.InitializeComponent();
            ViewModel = new SearchViewModel();
            
            DataContext = ViewModel;
        }

        private async void StartSearch(string query)
        {
            var service = Mvx.Resolve<ISearchService>();
            var documents = await service.Search(query);

            var results = (from document in documents
                                        select new CoreViewModel.SearchResultViewModel
                                        {
                                            Name = document.Title,
                                            Type = document.Type,
                                            Image = "",
                                            Year = (document.Year != 0) ? document.Year.ToString("####") : "Ukjent år",
                                            DocNumber = document.Id,
                                        }).ToList();

            ViewModel.GroupedResults = results.GroupBy(result => result.Name[0]).OrderBy(k => k.Key);
            groupedItemsViewSource.Source = ViewModel.GroupedResults;
            (semanticZoom.ZoomedOutView as ListViewBase).ItemsSource = groupedItemsViewSource.View.CollectionGroups;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var query = e.Parameter as string;

            if (!string.IsNullOrEmpty(query))
            {
                ViewModel.Query = query;
                StartSearch(query);
            }
            base.OnNavigatedTo(e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    class GroupedResult
    {
        public string Key { get; set; }
        public List<SearchViewModel> Items { get; set; }
    }

}

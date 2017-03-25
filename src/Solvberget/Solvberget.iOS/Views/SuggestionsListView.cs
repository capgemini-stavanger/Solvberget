using MvvmCross.Binding.BindingContext;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
    public partial class SuggestionsListView : NamedTableViewController
    {
        public new SuggestionsListViewModel ViewModel
        {
            get
            {
                return base.ViewModel as SuggestionsListViewModel;
            }
        }

        protected override void ViewModelReady()
        {
            base.ViewModelReady();

            LoadingOverlay.LoadingText = "Henter anbefalinger...";

            var source = new SimpleTableViewSource<SearchResultViewModel>(TableView, CellBindings.SearchResults);
            TableView.Source = source;

            var set = this.CreateBindingSet<SuggestionsListView, SuggestionsListViewModel>();
            set.Bind(source).To(vm => vm.Docs);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);

            set.Apply();

            TableView.ReloadData();
        }
    }
}


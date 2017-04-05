using MvvmCross.Binding.BindingContext;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
    public partial class SuggestionsListListView : NamedTableViewController
    {
        public new SuggestionsListListViewModel ViewModel => base.ViewModel as SuggestionsListListViewModel;

        protected override void ViewModelReady()
        {
            base.ViewModelReady();

            LoadingOverlay.LoadingText = "Henter anbefalinger...";

            var source = new SimpleTableViewSource<SuggestionListSummaryViewModel>(TableView, CellBindings.SuggestionLists);
            TableView.Source = source;

            var set = this.CreateBindingSet<SuggestionsListListView, SuggestionsListListViewModel>();
            set.Bind(source).To(vm => vm.Lists);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowListCommand);

            set.Apply();

            TableView.ReloadData();
        }
    }
}


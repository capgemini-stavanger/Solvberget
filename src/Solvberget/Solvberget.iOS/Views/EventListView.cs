using MvvmCross.Binding.BindingContext;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
    public partial class EventListView : NamedTableViewController
    {
        public new EventListViewModel ViewModel
        {
            get
            {
                return base.ViewModel as EventListViewModel;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            LoadingOverlay.LoadingText = "Henter arrangementer...";
        }

        protected override void ViewModelReady()
        {
            base.ViewModelReady();

            var source = new SimpleTableViewSource<EventViewModel>(TableView, CellBindings.Events);

            TableView.Source = source;

            var set = this.CreateBindingSet<EventListView, EventListViewModel>();
            set.Bind(source).To(vm => vm.Events);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);

            set.Apply();

            TableView.ReloadData();
        }
    }
}


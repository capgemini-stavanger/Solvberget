using MvvmCross.Binding.BindingContext;
using Solvberget.Core.ViewModels;
using UIKit;

namespace Solvberget.iOS
{
    public partial class OpeningHoursView : NamedTableViewController
    {
        public new OpeningHoursViewModel ViewModel
        {
            get
            {
                return base.ViewModel as OpeningHoursViewModel;
            }
        }

        protected override void ViewModelReady()
        {
            base.ViewModelReady();

            LoadingOverlay.LoadingText = "Henter åpningstider...";

            var source = new StandardTableViewSource(TableView, UITableViewCellStyle.Default,
                "OpeningHoursLocations", "TitleText Title");

            TableView.Source = source;

            var set = this.CreateBindingSet<OpeningHoursView, OpeningHoursViewModel>();
            set.Bind(source).To(vm => vm.Locations);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);

            set.Apply();

            TableView.ReloadData();
        }
    }
}


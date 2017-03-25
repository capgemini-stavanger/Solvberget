using MvvmCross.Binding.BindingContext;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
    public partial class OpeningHoursLocationView : NamedTableViewController
    {
        public new OpeningHoursLocationViewModel ViewModel { get { return base.ViewModel as OpeningHoursLocationViewModel; } }

        protected override void ViewModelReady()
        {
            base.ViewModelReady();

            LoadingOverlay.LoadingText = "Henter åpningstider...";

            var source = new StandardTableViewSource(TableView,
                "TitleText Key + If(Value.Length>0, ': ','') + Value");


            TableView.Source = source;
            TableView.AllowsSelection = false;

            var set = this.CreateBindingSet<OpeningHoursLocationView, OpeningHoursLocationViewModel>();
            set.Bind(source).To(vm => vm.Hours);

            set.Apply();

            TableView.ReloadData();

        }
    }
}


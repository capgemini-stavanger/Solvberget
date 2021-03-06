using MvvmCross.Binding.BindingContext;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
    public partial class BlogOverviewView : NamedTableViewController
    {
        public new BlogOverviewViewModel ViewModel
        {
            get
            {
                return base.ViewModel as BlogOverviewViewModel;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            LoadingOverlay.LoadingText = "Henter blogger...";
        }

        protected override void ViewModelReady()
        {
            base.ViewModelReady();

            var source = new SimpleTableViewSource<BlogItemViewModel>(TableView, CellBindings.Blogs);

            TableView.Source = source;

            var set = this.CreateBindingSet<BlogOverviewView, BlogOverviewViewModel>();
            set.Bind(source).To(vm => vm.Blogs);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailsCommand);

            set.Apply();

        }
    }
}


using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V4;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    [Register("solvberget.droid.views.fragments.SearchResultCategoryView")]
    public class SearchResultCategoryView : MvxFragment
    {
        public SearchResultCategoryView()
        {
            RetainInstance = true;
        }

        public string BindableProperty { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_searchresults, null);

            var searchResultsTitle = view.FindViewById<TextView>(Resource.Id.searchResultsTitle);
            var resultsList = view.FindViewById<MvxListView>(Resource.Id.searchResultsList);
            var set = this.CreateBindingSet<SearchResultCategoryView, SearchViewModel>();
            set.Bind(resultsList).For(rl => rl.ItemsSource).To(BindableProperty);
            set.Bind(resultsList).For(rl => rl.ItemClick).To(vm => vm.ShowDetailsCommand);
            set.Bind(searchResultsTitle).For(l => l.Visibility).To(vm => vm.LastQuery).WithConversion("Visibility");
            set.Bind(searchResultsTitle).For(l => l.Text).To(vm => vm.LastQuery);
            set.Apply();

            return view;
        }
    }
}
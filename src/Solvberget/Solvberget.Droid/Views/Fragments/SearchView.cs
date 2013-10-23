using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    public class SearchView : MvxFragment
    {
        public SearchView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            SetHasOptionsMenu(true);
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_search, null);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.search_menu, menu);

            IMenuItem item = menu.FindItem(Resource.Id.search);
            var sView = (Android.Widget.SearchView)item.ActionView;

            sView.Iconified = false;
            sView.QueryTextSubmit += sView_QueryTextSubmit;
            sView.QueryTextChange += sView_QueryTextChange;



            base.OnCreateOptionsMenu(menu, inflater);
        }

        void sView_QueryTextChange(object sender, Android.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            var vm = (SearchViewModel) ViewModel;
            vm.Query = e.NewText;
        }

        void sView_QueryTextSubmit(object sender, Android.Widget.SearchView.QueryTextSubmitEventArgs e)
        {
            var vm = (SearchViewModel)ViewModel;
            vm.SearchAndLoad();
        }
    }
}
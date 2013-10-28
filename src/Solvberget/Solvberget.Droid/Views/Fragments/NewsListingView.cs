using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;

namespace Solvberget.Droid.Views.Fragments
{
    public class NewsListingView : MvxSherlockFragment
    {
        public NewsListingView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            SetHasOptionsMenu(true);
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_newslisting, null);
        }
    }
}
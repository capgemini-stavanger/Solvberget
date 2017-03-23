using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    [Register("solvberget.droid.views.fragments.MyPageFavoritesView")]
    public class MyPageFavoritesView : MvxFragment
    {
        private MyPageFavoritesViewModel _viewModel;
        public new MyPageFavoritesViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as MyPageFavoritesViewModel); }
        }

        public MyPageFavoritesView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            ViewModel.PropertyChanged += MyPageFavoritesView_PropertyChanged;

            return this.BindingInflate(Resource.Layout.fragment_profile_favorites, null);
        }

        void MyPageFavoritesView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FavoriteIsRemoved")
            {
                var context = Application.Context;
                const string text = "Favoritt fjernet";
                const ToastLength duration = ToastLength.Long;

                var toast = Toast.MakeText(context, text, duration);
                toast.Show();
            }
        }

        public override void OnResume()
        {
            ViewModel.OnViewReady();
            base.OnResume();
        }
    }
}
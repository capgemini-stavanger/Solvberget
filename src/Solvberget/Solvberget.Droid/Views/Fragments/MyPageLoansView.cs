using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using Solvberget.Core.ViewModels;
using System.ComponentModel;

namespace Solvberget.Droid.Views.Fragments
{
    [Register("solvberget.droid.views.fragments.MyPageLoansView")]
    public class MyPageLoansView : MvxFragment
    {
        private MyPageLoansViewModel _viewModel;
        public new MyPageLoansViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as MyPageLoansViewModel); }
        }

        public MyPageLoansView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            ViewModel.PropertyChanged += MyPageLoansView_PropertyChanged;

            return this.BindingInflate(Resource.Layout.fragment_profile_loans, null);
        }

        private void MyPageLoansView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var isChanged = e.PropertyName;

            if (isChanged == "RenewalStatus")
                Toast.MakeText(Application.Context, ViewModel.RenewalStatus, ToastLength.Long).Show();
        }
        public override void OnResume()
        {
            ViewModel.OnViewReady();
            base.OnResume();
        }
    }
}
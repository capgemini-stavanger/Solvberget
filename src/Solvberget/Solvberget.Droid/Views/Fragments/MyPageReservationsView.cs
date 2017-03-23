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
    [Register("solvberget.droid.views.fragments.MyPageReservationsView")]
    public class MyPageReservationsView : MvxFragment
    {
        private MyPageReservationsViewModel _viewModel;
        public new MyPageReservationsViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as MyPageReservationsViewModel); }
        }

        public MyPageReservationsView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            ViewModel.PropertyChanged += MyPageReservationsView_PropertyChanged;

            return this.BindingInflate(Resource.Layout.fragment_profile_reservations, null);
        }

        void MyPageReservationsView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var isChanged = e.PropertyName;

            if (isChanged == "ReservationRemoved")
                Toast.MakeText(Application.Context, "Reservasjon fjernet", ToastLength.Long).Show();
        }

        public override void OnResume()
        {
            ViewModel.OnViewReady();
            base.OnResume();
        }
    }
}
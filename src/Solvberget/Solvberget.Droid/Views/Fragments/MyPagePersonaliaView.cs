using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace Solvberget.Droid.Views.Fragments
{
    [Register("solvberget.droid.views.fragments.MyPagePersonaliaView")]
    public class MyPagePersonaliaView : MvxFragment
    {
        public MyPagePersonaliaView()
        {
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_profile_personalia, null);
        }
    }
}
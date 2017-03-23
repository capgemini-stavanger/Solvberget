using Android.Content;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace Solvberget.Droid.ActionBar
{
    public class MvxActionBarActivity : MvxEventSourceActionBarActivity, IMvxAndroidView
    {
        protected MvxActionBarActivity()
        {
            BindingContext = new MvxAndroidBindingContext((Context)DataContext, this);
            this.AddEventListeners();
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set
            {
                DataContext = value;
                OnViewModelSet();
            }
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            base.StartActivityForResult(intent, requestCode);
        }

        protected virtual void OnViewModelSet()
        {
        }

        public IMvxBindingContext BindingContext { get; set; }


        //public override void SetContentView(int layoutResId)
        //{
        //    var view = this.BindingInflate(layoutResId, null);
        //    SetContentView(view);
        //}

        public LayoutInflater LayoutInflater { get; }
    }
}
using Android.App;
using Android.Support.V4.App;
using Android.Views;
using Android.Webkit;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    [Activity(Label = "Webside", Theme = "@style/MyTheme", Icon = "@android:color/transparent", ParentActivity = typeof(HomeView))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "solvberget.droid.views.HomeView")]
    public class GenericWebViewView : MvxSherlockActivity
    {
        private WebView _webView;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            
            var set = this.CreateBindingSet<GenericWebViewView, GenericWebViewViewModel>();
            set.Bind(SupportActionBar).For(v => v.Title).To(vm => vm.Title).Mode(MvxBindingMode.OneWay);
            set.Apply();


            _webView = new WebView(this);

            _webView.Settings.JavaScriptEnabled = true;
            _webView.Settings.SetSupportZoom(true);


            _webView.LoadUrl(((GenericWebViewViewModel)(ViewModel)).Uri);
            var webViewClient = new WebViewClient();
            _webView.SetWebViewClient(webViewClient);

            SetContentView(_webView);
        }
    }
}
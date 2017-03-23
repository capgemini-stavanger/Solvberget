using Android.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform;
using Solvberget.Core.ViewModels;
using Solvberget.Droid.ActionBar;
using Solvberget.Droid.Helpers;
using Solvberget.Droid.Views.WebClients;

namespace Solvberget.Droid.Views.Fragments
{
    [Activity(Label = "Webside", Theme = "@style/MyTheme", Icon = "@drawable/ic_launcher", ParentActivity = typeof(HomeView))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "solvberget.droid.views.HomeView")]
    public class LocalHtmlWebView : MvxActionBarActivity
    {
        private WebView _webView;

        private LocalHtmlWebViewModel _viewModel;
        public new LocalHtmlWebViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = base.ViewModel as LocalHtmlWebViewModel); }
        }

        protected override void OnViewModelSet()
        {
            Window.RequestFeature(WindowFeatures.Progress);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetBackgroundDrawable(Resources.GetDrawable(Resource.Color.s_main_green));
            SupportActionBar.SetLogo(Resource.Drawable.logo_white);

            base.OnViewModelSet();

            SetContentView(Resource.Layout.page_localwebview);


            var set = this.CreateBindingSet<LocalHtmlWebView, LocalHtmlWebViewModel>();
            set.Bind(SupportActionBar).For(v => v.Title).To(vm => vm.Title).Mode(MvxBindingMode.OneWay);
            set.Apply();


            _webView = FindViewById<WebView>(Resource.Id.webView);
            _webView.Settings.JavaScriptEnabled = true;
            _webView.Settings.SetSupportZoom(true);


            var progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            var webChromeClient = new ProgressUpdatingWebChromeClient(progressBar);
            var webViewClient = new ProgressHandlingWebViewClient(progressBar);
            _webView.SetWebViewClient(webViewClient);
            _webView.SetWebChromeClient(webChromeClient);

            if (!string.IsNullOrEmpty(ViewModel.Url))
            {
                _webView.LoadUrl(ViewModel.Url);
            }
            else if (!string.IsNullOrEmpty(ViewModel.Html))
            {
                _webView.LoadDataWithBaseURL(null, ViewModel.Html, "text/html", "UTF-8", null);
            }
        }

        protected override void OnResume()
        {
            ViewModel.OnViewReady();
            base.OnResume();
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (e.Action == KeyEventActions.Down)
            {
                switch (keyCode)
                {
                    case Keycode.Back:
                        if (_webView.CanGoBack())
                            _webView.GoBack();
                        else
                            Finish();
                        break;
                }
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }

        protected override void OnStart()
        {
            base.OnStart();
            var analytics = Mvx.Resolve<IAndroidAnalytics>();
            if (analytics != null)
            {
                analytics.StartSession(this);
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            var analytics = Mvx.Resolve<IAndroidAnalytics>();
            if (analytics != null)
            {
                analytics.EndSession(this);
            }
        }
    }
}
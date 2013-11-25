using Android.App;
using Android.Drm;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using Solvberget.Core.ViewModels;
using Solvberget.Droid.ActionBar;
using ActionProvider = Android.Support.V4.View.ActionProvider;
using ShareActionProvider = Android.Support.V7.Widget.ShareActionProvider;

namespace Solvberget.Droid.Views.Fragments
{
    [Activity(Label = "Mediadetaljer", Theme = "@style/MyTheme", Icon = "@android:color/transparent", ParentActivity = typeof(HomeView))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "solvberget.droid.views.HomeView")]
    public class MediaDetailView : MvxActionBarActivity
    {
        private LoadingIndicator _loadingIndicator;
        private ShareActionProvider _shareActionProvider;
        private IMenu _menu;
        private bool _starIsClicked;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.fragment_mediadetail);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            ((MediaDetailViewModel)ViewModel).PropertyChanged += MediaDetailView_PropertyChanged;

            BindLoadingIndicator();
        }

        void MediaDetailView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsFavorite":
                    if (_starIsClicked)
                    {
                        LoadMenu();
                        if (((MediaDetailViewModel)ViewModel).IsFavorite)
                            Toast.MakeText(Application.Context, "Lagt til som favoritt", ToastLength.Long).Show();
                    }
                    break;
                case "IsReservable":
                    if (((MediaDetailViewModel)ViewModel).IsReservedByUser)
                        Toast.MakeText(Application.Context, "Dokumentet er reservert", ToastLength.Long).Show();
                    break;
            }
        }

        private void BindLoadingIndicator()
        {
            _loadingIndicator = new LoadingIndicator(this);

            var set = this.CreateBindingSet<MediaDetailView, SearchResultViewModel>();
            set.Bind(SupportActionBar).For(v => v.Title).To(vm => vm.Title).Mode(MvxBindingMode.OneWay);
            set.Bind(_loadingIndicator).For(pi => pi.Visible).To(vm => vm.IsLoading);
            set.Apply();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    NavUtils.NavigateUpFromSameTask(this);
                    break;
                case Resource.Id.menu_is_not_favorite:
                    if (((MediaDetailViewModel)ViewModel).LoggedIn)
                    {
                        _starIsClicked = true;
                        ((MediaDetailViewModel)ViewModel).AddFavorite();
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "Logg inn for � legge til favoritter", ToastLength.Long).Show();
                    }
                    
                    break;
                case Resource.Id.menu_is_favorite:
                    _starIsClicked = true;
                    ((MediaDetailViewModel)ViewModel).RemoveFavorite();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            _menu = menu;

            return LoadMenu();
        }

        private void CreateShareMenu()
        {
            if (_shareActionProvider != null)
            {
                var playStoreLink = "https://play.google.com/store/apps/details?id=" + PackageName;
                var shareTextBody = string.Format(
                    "Jeg s�kte og fant {0} hos S�lvberget. Last ned appen p� Google Play for muligheten til � l�ne den du ogs�. {1}",
                    ((MediaDetailViewModel)ViewModel).Title,
                    playStoreLink);

                var shareIntent = ShareCompat.IntentBuilder.From(this)
                    .SetType("text/plain")
                    .SetText(shareTextBody)
                    .SetSubject("S�lvberget")
                    .Intent;
                _shareActionProvider.SetShareIntent(shareIntent);
            }
        }

        public bool LoadMenu()
        {
            _menu.Clear();

            MenuInflater.Inflate(
                ((MediaDetailViewModel)ViewModel).IsFavorite
                    ? Resource.Menu.star_is_favorite
                    : Resource.Menu.star_is_not_favorite, _menu);

            MenuInflater.Inflate(Resource.Menu.share, _menu);
            // Locate MenuItem with ShareActionProvider
            var inflatedShareView = _menu.FindItem(Resource.Id.menu_share);
            var actionShareView = new Android.Support.V7.Widget.ShareActionProvider(this);
            MenuItemCompat.SetActionProvider(inflatedShareView, actionShareView);

            _shareActionProvider = actionShareView;

            CreateShareMenu();

            _starIsClicked = false;

            return base.OnCreateOptionsMenu(_menu);
        }
    }
}
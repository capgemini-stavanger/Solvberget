using Android.App;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Views;
using Solvberget.Core.ViewModels;

namespace Solvberget.Droid.Views.Fragments
{
    [Activity(Label = "Mediadetaljer", Theme = "@style/MyTheme", Icon = "@android:color/transparent", ParentActivity = typeof(HomeView))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "solvberget.droid.views.HomeView")]
    public class MediaDetailView : MvxActivity
    {
        private LoadingIndicator _loadingIndicator;
        private ShareActionProvider _shareActionProvider;
        private IMenu _menu;
        private bool _starIsClicked;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.fragment_mediadetail);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            ((MediaDetailViewModel)ViewModel).PropertyChanged += MediaDetailView_PropertyChanged;

            BindLoadingIndicator();
        }

        void MediaDetailView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsFavorite" && _starIsClicked)
            {
                LoadMenu();
            }
        }

        private void BindLoadingIndicator()
        {
            _loadingIndicator = new LoadingIndicator(this);

            var set = this.CreateBindingSet<MediaDetailView, SearchResultViewModel>();
            set.Bind(ActionBar).For(v => v.Title).To(vm => vm.Title).Mode(MvxBindingMode.OneWay);
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
                    _starIsClicked = true;
                    ((MediaDetailViewModel)ViewModel).AddFavorite();
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
            IMenuItem shareMenuItem = _menu.FindItem(Resource.Id.menu_share);
            _shareActionProvider = (ShareActionProvider)shareMenuItem.ActionProvider;

            CreateShareMenu();

            _starIsClicked = false;

            return base.OnCreateOptionsMenu(_menu);
        }
    }
}
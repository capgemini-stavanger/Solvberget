using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Droid.Helpers;

namespace Solvberget.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IList<Assembly> AndroidViewAssemblies
        {
            get
            {
                var assemblies = base.AndroidViewAssemblies;
                assemblies.Add(typeof(Android.Support.V4.Widget.DrawerLayout).Assembly);
                return assemblies;
            }
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var customPresenter = new CustomPresenter();
            Mvx.RegisterSingleton<ICustomPresenter>(customPresenter);
            return customPresenter;
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            Mvx.LazyConstructAndRegisterSingleton<DtoDownloader, AndroidDtoDownloader>();
            Mvx.LazyConstructAndRegisterSingleton<IAnalyticsService, FlurryAnalyticsService>();
            Mvx.LazyConstructAndRegisterSingleton<IAndroidAnalytics, FlurryAnalyticsService>();
        }
    }
}
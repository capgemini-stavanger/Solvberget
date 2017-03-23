using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Solvberget.Core;
using Solvberget.Core.Services;
using Solvberget.Core.Services.Interfaces;
using Solvberget.Droid.Helpers;
using System.Collections.Generic;
using System.Reflection;

namespace Solvberget.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IEnumerable<Assembly> AndroidViewAssemblies
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
using Facebook.CoreKit;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
    using Foundation;
    using UIKit;

    /// <summary>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the 
    /// User Interface of the application, as well as listening (and optionally responding) to 
    /// application events from iOS.
    /// </summary>
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        const string FacebookAppId = "178416682352295";
        const string DisplayName = "Sølvberget for iOS";

        /// <summary>
        /// The window.
        /// </summary>
        public override UIWindow Window
        {
            get;
            set;
        }

        /// <summary>
        /// Finished the launching.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="options">The options.</param>
        /// <returns>True or false.</returns>
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var presenter = new MvxSlidingPanelsTouchViewPresenter(this, Window);
            var setup = new Setup(this, presenter);
            setup.Initialize();

            var appStart = new MvxAppStart<HomeScreenViewModel>();
            Mvx.RegisterSingleton<IMvxAppStart>(appStart);

            Mvx.LazyConstructAndRegisterSingleton<DtoDownloader, IosDtoDownloader>();

            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            Settings.AppID = FacebookAppId;
            Settings.DisplayName = DisplayName;
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;

            Window.MakeKeyAndVisible();

            return ApplicationDelegate.SharedInstance.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            // We need to handle URLs by passing them to FBSession in order for SSO authentication
            // to work.
            return ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
            //return FBSession.ActiveSession.HandleOpenURL(url);
        }

        public override void OnActivated(UIApplication application)
        {
            // We need to properly handle activation of the application with regards to SSO
            // (e.g., returning from iOS 6.0 authorization dialog or from fast app switching).
            //FBSession.ActiveSession.HandleDidBecomeActive();
        }
    }
}
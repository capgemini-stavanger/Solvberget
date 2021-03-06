using CoreGraphics;
using MvvmCross.iOS.Views;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;
using Solvberget.Core.ViewModels;
using System.Linq;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Solvberget.iOS
{
    public partial class HomeView : MvxViewController
    {
        public new HomeViewModel ViewModel
        {
            get
            {
                return base.ViewModel as HomeViewModel;
            }
        }

        static bool UserInterfaceIdiomIsPhone
        {
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
            }
        }

        private UIBarButtonItem CreateSliderButton(string imageName, PanelType panelType)
        {
            UIButton button = new UIButton(new CGRect(0, 0, 40f, 40f));
            button.SetBackgroundImage(UIImage.FromBundle(imageName), UIControlState.Normal);
            button.TouchUpInside += delegate
            {
                SlidingPanelsNavigationViewController navController = NavigationController as SlidingPanelsNavigationViewController;
                navController.TogglePanel(panelType);
            };

            return new UIBarButtonItem(button);
        }

        public HomeView() : base("HomeView", null)
        {
            NavigationItem.LeftBarButtonItem = CreateSliderButton("SlideRight40.png", PanelType.LeftPanel);
        }

        MenuTableViewSource source;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            source = new MenuTableViewSource(MenuTableView);

            source.BackgroundColor = Application.ThemeColors.Main2;
            source.TextColor = Application.ThemeColors.MainInverse;
            source.TintColor = Application.ThemeColors.Main;

            View.BackgroundColor = UIColor.Clear;
            MenuTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            MenuTableView.Source = source;
            MenuTableView.BackgroundColor = UIColor.Clear;
            MenuTableView.BackgroundView = null;

            var set = this.CreateBindingSet<HomeView, HomeViewModel>();
            set.Bind(source).To(vm => vm.MenuItems);
            set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.SelectMenuItemCommand);
            set.Apply();

            MenuTableView.ReloadData();

            View.Add(MenuTableView);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var currentView = MvxSlidingPanelsTouchViewPresenter.CurrentView;
            var row = null == currentView || null == currentView.ViewModel
                      ? null
                      : ViewModel.MenuItems.FirstOrDefault(mi => mi.ViewModelType == currentView.ViewModel.GetType());

            source.SelectRow(row);
        }
    }
}


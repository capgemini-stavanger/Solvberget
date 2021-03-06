using CoreGraphics;
using Foundation;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;
using Solvberget.Core.ViewModels;
using System;
using System.Linq;
using UIKit;

namespace Solvberget.iOS
{


    public partial class HomeScreenView : NamedViewController
    {
        public HomeScreenView() : base("HomeScreenView", null)
        {
            NavigationItem.LeftBarButtonItem = CreateSliderButton("logo.white.png", PanelType.LeftPanel);
        }

        public new HomeScreenViewModel ViewModel { get { return base.ViewModel as HomeScreenViewModel; } }

        private UIBarButtonItem CreateSliderButton(string imageName, PanelType panelType)
        {
            var button = new UIBarButtonItem(UIImage.FromBundle(imageName).Scale(new CGSize(20, 20)), UIBarButtonItemStyle.Plain,

                (s, e) =>
                {
                    SlidingPanelsNavigationViewController navController = NavigationController as SlidingPanelsNavigationViewController;
                    navController.TogglePanel(panelType);
                });

            return button;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (UIHelpers.MinVersion7) return;

            NavigationItem.Title = "Hjem";
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            ViewModel.WaitForReady(() => InvokeOnMainThread(CreateMenu));
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            foreach (var v in ScrollView.Subviews) v.RemoveFromSuperview();
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            foreach (var v in ScrollView.Subviews) v.RemoveFromSuperview();
            CreateMenu();
        }

        private void CreateMenu()
        {
            UIView myPageBox;

            if (View.Bounds.Width > 700)
            {
                var centeredBox = new UIView(new CGRect(0, 0, 640, 320));
                ScrollView.Add(centeredBox);
                ScrollView.ContentSize = ScrollView.Frame.Size;

                centeredBox.Center = new CGPoint(ScrollView.Bounds.Width / 2, ScrollView.Bounds.Height / 2);
                centeredBox.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;

                // tablet layout

                centeredBox.Add(myPageBox = CreateBox(0f, 0f, 200f, 200f, "m"));
                centeredBox.Add(CreateBox(220f, 0f, 200f, 200f, "s"));
                centeredBox.Add(CreateBox(440f, 0f, 200f, 200f, "h"));

                centeredBox.Add(CreateBox(0f, 220f, 200f, 100f, "a"));

                centeredBox.Add(CreateBox(220f, 220f, 90f, 100f, "q"));
                centeredBox.Add(CreateBox(330f, 220f, 90f, 100f, "n"));

                centeredBox.Add(CreateBox(440f, 220f, 90f, 100f, "å"));
                centeredBox.Add(CreateBox(550f, 220f, 90f, 100f, "c"));


            }
            else if (View.Bounds.Width > 320) // phone landscape
            {
                var extraPadding = UIScreen.MainScreen.Bounds.Height > 480 ? 15f : 0f;

                // phone layout
                var centeredBox = new UIView(new CGRect(0, 0, 390 + extraPadding * 3, 190 + extraPadding));
                ScrollView.Add(centeredBox);

                centeredBox.Center = new CGPoint(ScrollView.Bounds.Width / 2, ScrollView.Bounds.Height / 2);
                centeredBox.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;

                centeredBox.Add(myPageBox = CreateBox(0f, 0f, 90f, 90f, "m"));
                centeredBox.Add(CreateBox(100f + extraPadding, 0f, 90f, 90f, "s"));
                centeredBox.Add(CreateBox(200f + extraPadding * 2, 0f, 90f, 90f, "h"));
                centeredBox.Add(CreateBox(300f + extraPadding * 3, 0f, 90f, 90f, "a"));

                centeredBox.Add(CreateBox(0f, 100f + extraPadding, 90f, 90f, "q"));
                centeredBox.Add(CreateBox(100f + extraPadding, 100f + extraPadding, 90f, 90f, "n"));
                centeredBox.Add(CreateBox(200f + extraPadding * 2, 100f + extraPadding, 90f, 90f, "å"));
                centeredBox.Add(CreateBox(300f + extraPadding * 3, 100f + extraPadding, 90f, 90f, "c"));
            }
            else // phone portrait
            {
                var extraPadding = UIScreen.MainScreen.Bounds.Height > 480 ? 15f : 0f;

                // phone layout
                var centeredBox = new UIView(new CGRect(0, 0, 190 + extraPadding, 390 + extraPadding * 3));
                ScrollView.Add(centeredBox);

                centeredBox.Center = new CGPoint(ScrollView.Bounds.Width / 2, ScrollView.Bounds.Height / 2);
                centeredBox.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;

                centeredBox.Add(myPageBox = CreateBox(0f, 0f, 90f, 90f, "m"));
                centeredBox.Add(CreateBox(100f + extraPadding, 0f, 90f, 90f, "s"));

                centeredBox.Add(CreateBox(0f, 100f + extraPadding, 90f, 90f, "h"));
                centeredBox.Add(CreateBox(100f + extraPadding, 100f + extraPadding, 90f, 90f, "a"));

                centeredBox.Add(CreateBox(0f, 200f + extraPadding * 2, 90f, 90f, "q"));
                centeredBox.Add(CreateBox(100f + extraPadding, 200f + extraPadding * 2, 90f, 90f, "n"));

                centeredBox.Add(CreateBox(0f, 300f + extraPadding * 3, 90f, 90f, "å"));
                centeredBox.Add(CreateBox(100f + extraPadding, 300f + extraPadding * 3, 90f, 90f, "c"));
            }

            ScrollView.ContentSize = new CGSize(
                ScrollView.Subviews.Max(s => s.Frame.Right),
                ScrollView.Subviews.Max(s => s.Frame.Bottom + 20f));



            foreach (var v in ScrollView.Subviews)
            {
                v.Alpha = 0.0f;
                v.Transform = CGAffineTransform.MakeScale(1.1f, 1.1f);
            }

            foreach (UIView view in ScrollView.Subviews)
            {
                UIView.Animate(0.3f, 0f, UIViewAnimationOptions.CurveEaseOut,
                    () =>
                    {
                        view.Alpha = 1.0f;
                        view.Transform = CGAffineTransform.MakeScale(1f, 1f);

                    }, null);
            }

            ViewModel.WaitForReady(() =>
            {
                if (String.IsNullOrEmpty(ViewModel.MyPageBadgeText)) return;

                InvokeOnMainThread(() =>
                {
                    AddBadge(myPageBox, ViewModel.MyPageBadgeText);
                });
            });
        }

        UIView CreateBox(float x, float y, float width, float height, string itemChar)
        {
            var item = ViewModel.ListElements.First(l => l.IconChar == itemChar);
            UIView view = new UIView(new CGRect(0, 0, width, height));
            view.Center = new CGPoint(x + width / 2, y + height / 2);

            view.AddGestureRecognizer(new BoxGestureRecognizer(view));
            if (itemChar == "h") // Anbefalinger 
            {
                view.AddGestureRecognizer(new UITapGestureRecognizer(
                    () =>
                        UIApplication.SharedApplication.OpenUrl(
                            new NSUrl("https://www.stavanger-kulturhus.no/Anbefalinger"))));
            }
            else
            {
                view.AddGestureRecognizer(new UITapGestureRecognizer(() => item.GoToCommand.Execute(null)));
            }

            view.BackgroundColor = ColorFor(item);

            UILabel title = new UILabel();
            title.TextColor = UIColor.White;
            title.BackgroundColor = UIColor.Clear;

            if (height < 120) title.Font = Application.ThemeColors.DefaultFont.WithSize(12f);
            else if (height < 200) title.Font = Application.ThemeColors.DefaultFont;
            else title.Font = Application.ThemeColors.TitleFont.WithSize(20f);

            title.Text = item.Title;

            var size = title.SizeThatFits(new CGSize(width, 0));
            var titleX = (width - size.Width) / 2;
            var titleY = height - size.Height - 5f;

            title.Frame = new CGRect(new CGPoint(titleX, titleY), size);

            view.Add(title);

            var icon = new UILabel();
            icon.BackgroundColor = UIColor.Clear;
            icon.Font = Application.ThemeColors.IconFont.WithSize(height / 3);

            var heightOffset = height < 200 ? -3 : 0;

            icon.TextColor = UIColor.White;
            icon.TextAlignment = UITextAlignment.Center;
            icon.LineBreakMode = UILineBreakMode.CharacterWrap;
            icon.Frame = new CGRect(0, 0, height - 40, height - 40);
            icon.Center = new CGPoint(width / 2, (height / 2) + heightOffset);
            icon.Text = item.IconChar;

            view.Add(icon);

            return view;

        }

        UILabel AddBadge(UIView box, string text)
        {
            var badgeSize = box.Frame.Width > 100 ? 25f : 18f;

            UILabel badge = new UILabel();
            badge.Font = Application.ThemeColors.DefaultFontBold.WithSize(badgeSize - 6f);
            badge.TextColor = Application.ThemeColors.MainInverse;

            badge.BackgroundColor = Application.ThemeColors.Main2.ColorWithAlpha(0.35f);
            badge.Text = text;
            badge.TextAlignment = UITextAlignment.Center;
            badge.Frame = new CGRect(new CGPoint(
                (box.Frame.Width - badgeSize), 0f), new CGSize(badgeSize, badgeSize));

            box.Add(badge);

            return badge;
        }

        public class BoxGestureRecognizer : UIGestureRecognizer
        {
            UIView _box;

            public BoxGestureRecognizer(UIView box)
            {
                _box = box;
            }

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                UIView.BeginAnimations(null);
                UIView.SetAnimationDuration(0.2f);
                _box.Transform = CGAffineTransform.MakeScale(.9f, .9f);
                UIView.CommitAnimations();

            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);
                EndAnimate();
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);
                EndAnimate();
            }

            private void EndAnimate()
            {
                UIView.BeginAnimations(null);
                UIView.SetAnimationDuration(0.2f);
                _box.Transform = CGAffineTransform.MakeScale(1f, 1f);
                UIView.CommitAnimations();
            }

        }

        UIColor ColorFor(HomeScreenElementViewModel item)
        {
            switch (item.IconChar)
            {
                case "m": // min side
                    return UIColor.FromRGB(53, 180, 70);
                case "a": // arrangementer
                    return UIColor.FromRGB(22, 143, 89);
                case "s": // søk
                    return UIColor.FromRGB(0, 115, 19);
                case "h": // anbefalinger
                    return UIColor.FromRGB(102, 207, 40);
                default:
                    return UIColor.FromRGB(53, 180, 70);

            }
        }
    }
}


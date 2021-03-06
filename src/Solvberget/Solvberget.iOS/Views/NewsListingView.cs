using CoreGraphics;
using Foundation;
using Solvberget.Core.ViewModels;
using System;
using System.Globalization;
using UIKit;

namespace Solvberget.iOS
{
    public class NewsListingView : NamedViewController
    {
        public new NewsListingViewModel ViewModel
        {
            get
            {
                return base.ViewModel as NewsListingViewModel;
            }
        }

        public NewsListingView() : base()
        { }

        UIScrollView container;

        public override void ViewDidLoad()
        {
            View.BackgroundColor = UIColor.White;
            base.ViewDidLoad();
        }

        protected override void ViewModelReady()
        {
            base.ViewModelReady();

            LoadingOverlay.LoadingText = "Henter nyheter...";

            if (null != container)
                container.RemoveFromSuperview();

            //View.Frame = new RectangleF(View.Frame);
            View.AutoresizingMask = UIViewAutoresizing.All;
            container = new UIScrollView(new CGRect(CGPoint.Empty, View.Frame.Size));

            View.Add(container);

            StyleView();
            RenderView();
        }

        void StyleView()
        {
            container.BackgroundColor = UIColor.White;
        }

        private void RenderView()
        {

            nfloat padding = 10.0f;

            nfloat y = padding;

            foreach (var item in ViewModel.Stories)
            {
                var itemCtrl = new TitleAndSummaryItem();
                itemCtrl.View.Frame = new CGRect(padding, y, container.Frame.Width - (2 * padding), 50.0f);

                itemCtrl.Clicked += (sender, e) => UIApplication.SharedApplication.OpenUrl(new NSUrl(item.Uri.OriginalString));

                itemCtrl.TitleLabelText = item.NewsTitle;

                itemCtrl.SummaryLabelText = item.Published.ToString("dddd d. MMMM", new CultureInfo("nb-no")).ToUpperInvariant();

                if (!String.IsNullOrEmpty(item.Ingress)) itemCtrl.SummaryLabelText += Environment.NewLine + Environment.NewLine + item.Ingress.Replace("&nbsp;", " ");

                container.Add(itemCtrl.View);

                y += itemCtrl.Frame.Height + padding;
            }

            container.ContentSize = new CGSize(320, y);
            container.ScrollEnabled = true;
        }
    }
}


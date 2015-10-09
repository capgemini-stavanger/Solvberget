using System;
using System.Drawing;
using Foundation;
using UIKit;
using CoreGraphics;

namespace Solvberget.iOS
{
    public partial class TitleAndSummaryItem : UIViewController
    {
		public TitleAndSummaryItem() : base("TitleAndSummaryItem", null)
        {
        }

       	public string TitleLabelText { get; set;
		}

		public string SummaryLabelText
		{
			get;
			set;
		}

		public CGRect Frame
		{
			get { return View.Frame; }
			set { View.Frame = value; }
		}

		void StyleView()
		{
			TitleLabel.Font = Application.ThemeColors.TitleFont;
			TitleLabel.TextColor = Application.ThemeColors.Main;
			SummaryLabel.Font = Application.ThemeColors.DefaultFont;
			SummaryLabel.TextColor = Application.ThemeColors.Main2;
			View.BackgroundColor = Application.ThemeColors.VerySubtle;
		}

		private void UpdateLayout()
		{
			TitleLabel.Text = TitleLabelText;
			SummaryLabel.Text = SummaryLabelText;
			TitleLabel.Frame = new CGRect(TitleLabel.Frame.Location, TitleLabel.SizeThatFits(new CGSize(TitleLabel.Frame.Width, 0)));
			var padding = 10.0f;
			var titlePadding = 5.0f;
			SummaryLabel.Frame = new CGRect(new CGPoint(SummaryLabel.Frame.X, TitleLabel.Frame.Bottom + titlePadding), 
				SummaryLabel.SizeThatFits(new CGSize(SummaryLabel.Frame.Width, 0)));

			var totalHeight = SummaryLabel.Frame.Bottom + padding;

			View.Frame = new CGRect(View.Frame.Location, new CGSize(View.Frame.Width, totalHeight));

			#if DEBUG
			//TitleLabel.BackgroundColor = UIColor.Red.ColorWithAlpha(0.5f);
			//SummaryLabel.BackgroundColor = UIColor.Blue.ColorWithAlpha(0.5f);
			#endif

		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		
			StyleView();
			UpdateLayout();

			View.AddGestureRecognizer(new UITapGestureRecognizer(OnTap));
        }

		public event EventHandler Clicked = (s,e) => {};

		private void OnTap()
		{
			Clicked(this, EventArgs.Empty);
		}
    }
}


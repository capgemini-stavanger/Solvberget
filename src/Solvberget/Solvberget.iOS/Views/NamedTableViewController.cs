using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using SlidingPanels.Lib;
using SlidingPanels.Lib.PanelContainers;
using Solvberget.Core.ViewModels.Base;

namespace Solvberget.iOS
{
	public abstract class NamedTableViewController : MvxTableViewController
	{
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (null == ViewModel) return;
			NavigationItem.Title = (ViewModel as BaseViewModel).Title.ToUpperInvariant();
		}

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}

		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			base.WillDisplay(tableView, cell, indexPath);
		}

		public override void ViewWillAppear(bool animated)
		{
			if (null != ViewModel) NavigationItem.Title = (ViewModel as BaseViewModel).Title.ToUpperInvariant();

			base.ViewWillAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			if (UIHelpers.MinVersion7)
			{
				NavigationItem.Title = String.Empty;
			}

			base.ViewWillDisappear(animated);

		}
	}
	
}

// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Solvberget.iOS
{
	[Register ("BlogView")]
	partial class BlogView
	{
		[Outlet]
		UIKit.UIView DescriptionContainer { get; set; }

		[Outlet]
		UIKit.UILabel DescriptionLabel { get; set; }

		[Outlet]
		UIKit.UIView ItemsContainer { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollContainer { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DescriptionContainer != null) {
				DescriptionContainer.Dispose ();
				DescriptionContainer = null;
			}

			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}

			if (ItemsContainer != null) {
				ItemsContainer.Dispose ();
				ItemsContainer = null;
			}

			if (ScrollContainer != null) {
				ScrollContainer.Dispose ();
				ScrollContainer = null;
			}
		}
	}
}

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
	[Register ("LoginView")]
	partial class LoginView
	{
		[Outlet]
		UIKit.UILabel ErrorMessage { get; set; }

		[Outlet]
		UIKit.UIButton LoginButton { get; set; }

		[Outlet]
		UIKit.UIButton LostCredentialsButton { get; set; }

		[Outlet]
		UIKit.UITextField Password { get; set; }

		[Outlet]
		UIKit.UILabel PinLabel { get; set; }

		[Outlet]
		UIKit.UITextField Username { get; set; }

		[Outlet]
		UIKit.UILabel UsernameLAbel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (LoginButton != null) {
				LoginButton.Dispose ();
				LoginButton = null;
			}

			if (LostCredentialsButton != null) {
				LostCredentialsButton.Dispose ();
				LostCredentialsButton = null;
			}

			if (Password != null) {
				Password.Dispose ();
				Password = null;
			}

			if (PinLabel != null) {
				PinLabel.Dispose ();
				PinLabel = null;
			}

			if (Username != null) {
				Username.Dispose ();
				Username = null;
			}

			if (UsernameLAbel != null) {
				UsernameLAbel.Dispose ();
				UsernameLAbel = null;
			}

			if (ErrorMessage != null) {
				ErrorMessage.Dispose ();
				ErrorMessage = null;
			}
		}
	}
}

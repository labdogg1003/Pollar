// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Pollzit
{
	[Register ("CustomCellView")]
	partial class CustomCellView
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblQuestion { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblRoomCode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView taskcell { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (lblQuestion != null) {
				lblQuestion.Dispose ();
				lblQuestion = null;
			}
			if (lblRoomCode != null) {
				lblRoomCode.Dispose ();
				lblRoomCode = null;
			}
			if (taskcell != null) {
				taskcell.Dispose ();
				taskcell = null;
			}
		}
	}
}

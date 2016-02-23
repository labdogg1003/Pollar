using System;
using System.CodeDom.Compiler;
using UIKit;
using CoreGraphics;

namespace Pollzit
{
	partial class CustomCellView : UITableViewCell
	{
		public CustomCellView (IntPtr handle) : base (handle)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Gray;
			//ContentView.BackgroundColor = UIColor.FromRGB (218, 255, 127);
			lblRoomCode = new UILabel();
			lblQuestion = new UILabel ();
			ContentView.AddSubviews(new UIView[] {lblRoomCode, lblQuestion});
			lblQuestion.Font = UIFont.ItalicSystemFontOfSize (12);

		}
		public void UpdateCell (string code, string question)
		{
			lblRoomCode.Text = code;
			lblQuestion.Text = question;
		}
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			//lblRoomCode.Frame = new CGRect (10, 10, 40, 40);
			//lblQuestion.Frame = new CGRect (60, 12, 100, 40);
		}
	}
}

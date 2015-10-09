using System;
using UIKit;
using System.Drawing;
using System.Linq;
using Foundation;
using CoreGraphics;

namespace Solvberget.iOS
{
	public class LabelAndValue
    {
		UILabel _label = new UILabel();
		UILabel _value = new UILabel();

		public LabelAndValue(UIView container, string label, string value, nint colspan = 1, Action onTap = null)
			: this(container, label, value, false, colspan: colspan, onTap : onTap)
		{}


		private nint LabelTag = 2;
		private nint ValueTag = 3;

		public LabelAndValue(UIView container, string label, string value, bool bold, nint colspan = 1, Action onTap = null)
        {
			// style 
			_label.Tag = LabelTag;
			_label.Font = Application.ThemeColors.LabelFont;
			_label.TextColor = Application.ThemeColors.Main;
			_label.BackgroundColor = UIColor.Clear;

			_value.Font = Application.ThemeColors.DefaultFont;
			_value.TextColor = Application.ThemeColors.Main2;
			_value.Lines = 0;
			_value.Tag = ValueTag;
			_value.BackgroundColor = UIColor.Clear;
			_value.LineBreakMode = UILineBreakMode.WordWrap;

			#if DEBUG
			// uncomment to visualize label and value frames
			//_label.BackgroundColor = UIColor.Red.ColorWithAlpha(0.5f);
			//_value.BackgroundColor = UIColor.Blue.ColorWithAlpha(0.5f);
			#endif

			if (bold) _value.Font = Application.ThemeColors.DefaultFontBold;

			nfloat padding = 10f;
			nint columnWidth = 300; //single column on phone portrait;
			var maxColspan = 1;

			// show 2 columns on phone landscape
			if (container.Frame.Width > 300)
			{
				columnWidth = (nint)Math.Floor((container.Frame.Width - 3*padding) / 2f);
				maxColspan = 2;
			}
			// show 3 columns on ipad portrait+landscape
			if (container.Frame.Width > 700)
			{
				columnWidth = (nint)Math.Floor((container.Frame.Width - 4*padding) / 3f);
				maxColspan = 3;
			}

			columnWidth = (nint)(columnWidth * Math.Min(maxColspan, colspan) + (nint)padding * (colspan-1));

			UIView lastLabel = container.Subviews.LastOrDefault(v => v.Tag == LabelTag);

			var lastX = null == lastLabel ? 0.0f : lastLabel.Frame.Right;
			var lastY = null == lastLabel ? 0.0f : lastLabel.Frame.Top;

			// new row, first column
			var x = padding;
			var y = container.Subviews.Length == 0 ? padding : container.Subviews.Max(s => s.Frame.Bottom) + padding*2;

			if (null != lastLabel && lastX + columnWidth <= container.Frame.Width + 1) // +1 to ensure rounding is not an issue
			{
				// new column, not first column
				x = lastX + padding;
				y = lastY;
			}

			// add label and optional value to container
			if (String.IsNullOrEmpty(label) && bold)
			{
				_value.Font = Application.ThemeColors.HeaderFont;
			}
			else
			{
				container.Add(_label);
				_label.Text = null == label ? String.Empty : label.ToUpperInvariant();
			}

			if (!String.IsNullOrEmpty(value))
			{
				container.Add(_value);
				_value.Text = value;
			}

			// determine minimum size
			var labelSize = _label.SizeThatFits(new CGSize(columnWidth, 0));
			var valueSize = _value.SizeThatFits(new CGSize(columnWidth, 0));

			// expand size to fill column width
			labelSize = new CGSize(columnWidth, labelSize.Height);
			valueSize = new CGSize(columnWidth, valueSize.Height);

			_label.Frame = new CGRect(new CGPoint(x,y), labelSize);

			y = _label.Frame.Bottom;
			_value.Frame = new CGRect(new CGPoint(x,y), valueSize);

			// resize container to fit new label/value pair
			var newHeight = container.Subviews.Max(s => s.Frame.Bottom) + padding;

			container.Frame = new CGRect(container.Frame.Location, 
				new CGSize(container.Frame.Width, newHeight));

			if (null != onTap)
			{
				_value.TextColor = Application.ThemeColors.Main;
				var onTapAction = new NSAction(onTap);

				_label.AddGestureRecognizer(new UITapGestureRecognizer(onTapAction));
				_value.AddGestureRecognizer(new UITapGestureRecognizer(onTapAction));

				_label.UserInteractionEnabled = _value.UserInteractionEnabled = true;
			}
		}
    }
}


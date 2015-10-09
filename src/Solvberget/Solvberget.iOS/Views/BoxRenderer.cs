using System;
using CoreGraphics;
using UIKit;
using System.Linq;

namespace Solvberget.iOS
{
	public class BoxRenderer
	{
		UIView _container;

		public BoxRenderer(UIView container)
		{
			_container = container;
			_boxWidthMinusPadding = container.Bounds.Width - (2*_padding);
		}

		nfloat _padding = 10.0f;
		nfloat _boxWidthMinusPadding;

		public UIView StartBox()
		{
			var box = new UIView(new CGRect(_padding, _padding, _boxWidthMinusPadding,_padding));
			box.BackgroundColor = Application.ThemeColors.VerySubtle;

			var prev = _container.Subviews.LastOrDefault();

			if (null != prev)
			{
				box.Frame = new CGRect(_padding, prev.Frame.Bottom + _padding, _boxWidthMinusPadding,_padding);
			}
		
			_container.Add(box);

			return box;
		}

		public void AddSectionHeader(string name)
		{
			UILabel label = new UILabel();
			label.Text = name.ToUpperInvariant();
			label.Font = Application.ThemeColors.HeaderFont;
			label.TextColor = Application.ThemeColors.Main;

			var y = _container.Subviews.Length == 0 ? _padding : _container.Subviews.Last().Frame.Bottom + _padding;
			label.Frame = new CGRect(_padding, y, _boxWidthMinusPadding, label.SizeThatFits(new CGSize(_boxWidthMinusPadding, 0)).Height);

			_container.Add(label);
		}
	}

}

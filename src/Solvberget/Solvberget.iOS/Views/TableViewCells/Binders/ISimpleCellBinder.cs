using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public interface ISimpleCellBinder<T> where T : class
	{
		void Bind(SimpleCell cell, T model);
	}
}

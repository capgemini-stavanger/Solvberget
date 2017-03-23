using Android.Text;
using MvvmCross.Platform.Converters;
using System;
using System.Globalization;

namespace Solvberget.Droid.Converters
{
    public class StringToHtmlConverter : MvxValueConverter<string, ISpanned>
    {
        protected override ISpanned Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return Html.FromHtml(value);
        }
    }
}
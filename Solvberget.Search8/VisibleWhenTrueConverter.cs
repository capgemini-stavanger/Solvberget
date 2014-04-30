using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Solvberget.Search8
{
    public class VisibleWhenTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return System.Convert.ToBoolean(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
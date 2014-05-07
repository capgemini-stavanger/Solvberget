using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Solvberget.Search8
{
    public class VisibleWhenTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Boolean || value is Int32)
            {
                return System.Convert.ToBoolean(value) ? Visibility.Visible : Visibility.Collapsed;
            }
            
            if (value is String)
            {
                return !String.IsNullOrEmpty(value as String) ? Visibility.Visible : Visibility.Collapsed;
            }
            
            return null == value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Solvberget.Search8
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (String.IsNullOrEmpty((string)value) || (string)value == "Ukjent")
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

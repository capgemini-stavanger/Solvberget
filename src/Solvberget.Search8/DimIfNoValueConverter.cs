using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Solvberget.Search8
{
    public class DimIfNoValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!String.IsNullOrEmpty(value as String)) return value;
            return "Ukjent";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.Web;

namespace Solvberget.Search8
{
    public class CollapseWhenTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return System.Convert.ToBoolean(value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
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
            var vis = (Visibility)(new VisibleWhenTrueConverter().Convert(value, targetType, parameter, language));

            return vis == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
using Solvberget.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Solvberget.Search8
{
    class DocumentImageHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // for results view
            if (value is string)
            {
                if((string)value == "Cd" || (string)value == "AudioBook")
                {
                    return Stretch.Uniform;
                } else
                {
                    return Stretch.Fill;
                }
            }
            
            // for detail result view
            if (value is CdDto || value is AudioBookDto)
            {
                return 300;
            }
            return 463;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

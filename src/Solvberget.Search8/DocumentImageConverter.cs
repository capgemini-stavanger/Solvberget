using System;
using Windows.UI.Xaml.Data;
using Caliburn.Micro;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services;

namespace Solvberget.Search8
{
    public class DocumentImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var doc = value as DocumentDto;
            if (null == doc) return null;
            
            var urls = IoC.Get<IServiceUrls>();

            return urls.ServiceUrl + String.Format(urls.ServiceUrl_MediaImage, doc.Id);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
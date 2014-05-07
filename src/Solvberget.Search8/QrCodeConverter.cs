using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using ZXing;

namespace Solvberget.Search8
{
    public class QrCodeConverter : IValueConverter
    {
        private const int QrCodePixelSize = 120;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (null == value) return null;

            var writer = new BarcodeWriter {Format = BarcodeFormat.QR_CODE};
            writer.Options.Height = QrCodePixelSize;
            writer.Options.Width = QrCodePixelSize;

            var bitmap = writer.Write(value.ToString()).ToBitmap() as WriteableBitmap;

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
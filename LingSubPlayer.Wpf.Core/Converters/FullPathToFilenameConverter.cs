using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace LingSubPlayer.Wpf.Core.Converters
{
    public class FullPathToFilenameConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;

            if (s == null)
            {
                return null;
            }

            return Path.GetFileName(s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
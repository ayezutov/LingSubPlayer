using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace LingSubPlayer.Wpf.Core.Converters
{
    public class ItemsCountToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof (Visibility))
            {
                throw new NotSupportedException("Can only convert to Visibility");
            }
            
            var enumerable = value as IEnumerable;

            if (enumerable != null)
            {
                return enumerable.Cast<object>().Any() ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;

namespace LingSubPlayer.Wpf.Core.Converters
{
    public class PercentageToAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof (double))
            {
                throw new NotSupportedException();
            }

            if (value == null)
            {
                return 0.0;
            }

            double initial = System.Convert.ToDouble(value);

            return initial/100*360;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
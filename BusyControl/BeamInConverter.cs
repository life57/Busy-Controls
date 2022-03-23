using System;
using System.Globalization;
using System.Windows.Data;

namespace BusyControl
{
    public class BeamInConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int r = (int)value;
            return (int)(-0.5 * r) + (int)(r *0.2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

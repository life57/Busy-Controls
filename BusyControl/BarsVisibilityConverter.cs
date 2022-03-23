using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static BusyControl.BusyUserControl;

namespace BusyControl
{
    public class BarsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var t = (BusyType)value;
            return t == BusyType.Bars ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

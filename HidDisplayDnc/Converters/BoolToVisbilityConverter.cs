using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace HidDisplayDnc.Converters
{
    [ValueConversion(typeof(bool), typeof(System.Windows.Visibility))]
    public class BoolToVisbilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(System.Windows.Visibility))
                throw new InvalidOperationException("The target must be a System.Windows.Visibility");

            return (bool)value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}

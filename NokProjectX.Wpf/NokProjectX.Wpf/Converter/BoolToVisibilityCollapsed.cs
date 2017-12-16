namespace NokProjectX.Wpf.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Defines the <see cref="BoolToVisibilityCollapsed" />
    /// </summary>
    public class BoolToVisibilityCollapsed : IValueConverter
    {
        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="value">The <see cref="object"/></param>
        /// <param name="targetType">The <see cref="Type"/></param>
        /// <param name="parameter">The <see cref="object"/></param>
        /// <param name="culture">The <see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = false;
            if (value is bool b)
            {
                flag = b;
            }
            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The <see cref="object"/></param>
        /// <param name="targetType">The <see cref="Type"/></param>
        /// <param name="parameter">The <see cref="object"/></param>
        /// <param name="culture">The <see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace NokProjectX.Wpf.Converter
{
    public class InventoryHoverConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool b)) return null;
            if (b)
            {
                return "/NokProjectX.Wpf;component/Resources/inventoryBlue.png";
            }
            return "/NokProjectX.Wpf;component/Resources/inventoryBlack.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
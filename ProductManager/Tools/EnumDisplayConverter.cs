using System.Globalization;
using ProductManager.Common;

namespace ProductManager.Tools
{
    // converts enum values into readable text in xaml bindings
    public class EnumDisplayConverter : IValueConverter
    {

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // return empty text if binding value is missing
            if (value is null)
            {
                return string.Empty;
            }

            // if value is not an enum, just return its string representation
            if (value is not Enum enumValue)
            {
                return value.ToString() ?? string.Empty;
            }
            // use Display attribute text for enums
            return enumValue.GetDisplayName();
        }

        // convert back is not needed because data is only displayed
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
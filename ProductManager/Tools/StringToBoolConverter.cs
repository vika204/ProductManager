using System.Globalization;

namespace ProductManager.Tools
{
    public class StringToBoolConverter : IValueConverter
    {
        // returns true if validation message is not empty.
        // used to show or hide error labels in forms.
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return !string.IsNullOrWhiteSpace(value?.ToString());
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
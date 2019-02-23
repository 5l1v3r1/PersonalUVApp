using System;
using System.Globalization;
using Xamarin.Forms;

namespace PersonalUVApp.Converters
{
    public class NameToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return "B" == value.ToString() ? Color.Red : Color.Blue;
            }
            else
                return Color.Brown;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

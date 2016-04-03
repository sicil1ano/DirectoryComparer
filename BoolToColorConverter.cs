using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace CompareDirectories
{
    /// <summary>
    /// It converts the boolean result of the equality check of the contents of directories in a color.
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        #region IValueConverterMembers

        /// <summary>
        /// Converts the boolean value to a color (Green/Red);
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = (bool)value;

            return result ? new SolidColorBrush(Colors.DarkGreen) : new SolidColorBrush(Colors.DarkRed);
        }

        /// <summary>
        /// Converts back the color value to a boolean one. (NOT USED)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}

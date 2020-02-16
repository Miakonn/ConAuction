using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ConAuction3.Converters {

    public class EmptyListVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            else
            {
                ICollection list = value as ICollection;
                if (list != null)
                {
                    if (list.Count == 0)
                        return Visibility.Collapsed;
                    else
                        return Visibility.Visible;
                }
                else
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class BoolToValueConverter<T> : IValueConverter {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null) return FalseValue;
            return (bool) value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value != null && value.Equals(TrueValue);
        }
    }

    public class BoolToIntConverter : BoolToValueConverter<int> { }

    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility> { }

    public class BoolToBoolConverter : BoolToValueConverter<bool> { }


    [ValueConversion(typeof(bool), typeof(SolidColorBrush))]
    public class BoolToColorBrushConverter : IValueConverter {
        #region Implementation of IValueConverter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Boolean value controlling color change</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">A CSV string on the format [ColorNameIfTrue;ColorNameIfFalse;OpacityNumber] may be provided for customization, default is [LimeGreen;Transperent;1.0].</param>
        /// <param name="culture"></param>
        /// <returns>A SolidColorBrush in the supplied or default colors depending on the state of value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            SolidColorBrush color;
            // Setting default values
            var colorIfTrue = Colors.LimeGreen;
            var colorIfFalse = Colors.Transparent;
            double opacity = 1;
            // Parsing converter parameter
            if (parameter != null) {
                // Parameter format: [ColorNameIfTrue;ColorNameIfFalse;OpacityNumber]
                var parameterString = parameter.ToString();
                if (!string.IsNullOrEmpty(parameterString)) {
                    var parameters = parameterString.Split(';');
                    var count = parameters.Length;
                    if (count > 0 && !string.IsNullOrEmpty(parameters[0])) {
                        colorIfTrue = ColorFromName(parameters[0]);
                    }

                    if (count > 1 && !string.IsNullOrEmpty(parameters[1])) {
                        colorIfFalse = ColorFromName(parameters[1]);
                    }

                    if (count > 2 && !string.IsNullOrEmpty(parameters[2])) {
                        if (double.TryParse(parameters[2], NumberStyles.AllowDecimalPoint,
                            CultureInfo.InvariantCulture.NumberFormat, out var dblTemp))
                            opacity = dblTemp;
                    }
                }
            }

            // Creating Color Brush
            if (value != null && (bool) value) {
                color = new SolidColorBrush(colorIfTrue) {Opacity = opacity};
            }
            else {
                color = new SolidColorBrush(colorIfFalse) {Opacity = opacity};
            }

            return color;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion

        public static Color ColorFromName(string colorName) {
            var systemColor = System.Drawing.Color.FromName(colorName);
            return Color.FromArgb(systemColor.A, systemColor.R, systemColor.G, systemColor.B);
        }
    }
}
using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

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
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToValueConverter<T> : IValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null) return FalseValue;
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(TrueValue);
        }
    }

    public class BoolToIntConverter : BoolToValueConverter<int> { }
    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility> { }
    public class BoolToBoolConverter : BoolToValueConverter<bool> { }

  

}
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ConAuction3.Converters {
    public class StringNullOrEmptyToVisibilityConverter : MarkupExtension, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return string.IsNullOrEmpty(value as string)
                       ? Visibility.Collapsed
                       : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
    }
}
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UserManager.Core.ViewManagement
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter
        where T : class, IValueConverter, new()
    {
        private static T converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (converter == null)
                converter = new T();

            return converter;
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}

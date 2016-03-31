using System.Windows;

namespace UserManager.Core.ViewManagement
{
    public static class ConverterHelper
    {
        public static TValueType GetValue<TValueType>(object value, TValueType defaultValue) 
            where TValueType : struct
        {
            if (value == DependencyProperty.UnsetValue)
                return defaultValue;

            TValueType ret = defaultValue;

            if (value is TValueType)
            {
                ret = (TValueType)value;
            }
            else if (value is TValueType?)
            {
                var nullable = (TValueType?)value;
                ret = nullable ?? defaultValue;
            }

            return ret;
        }

        public static TValueType GetValue<TValueType>(object value) where TValueType : struct
        {
            return GetValue<TValueType>(value, default(TValueType));
        }
    }
}

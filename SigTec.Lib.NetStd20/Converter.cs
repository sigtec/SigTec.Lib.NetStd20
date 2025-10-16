namespace SigTec.Lib.NetStd20
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Globalization;
  using System.Text;
  using System.Text.RegularExpressions;

  public static class Converter
  {

    public static T ConvertTo<T>(object? value, CultureInfo? cultureInfo = null) => (T)ConvertTo(value, typeof(T), cultureInfo)!;
    public static object? ConvertTo(object? value, Type targetType, CultureInfo? cultureInfo = null)
    {
      if (value == null || value is DBNull)
      {
        if (targetType.IsNullable())
        {
          return null;
        }
        return Activator.CreateInstance(targetType);
      }

      var sourceType = value.GetType();

      // get underlying type for Nullables
      targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;
      sourceType = Nullable.GetUnderlyingType(sourceType) ?? sourceType;

      // culture defaults to InvariantCulture
      cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;

      // direct assignment possible?
      if (targetType.IsAssignableFrom(sourceType))
      {
        return value;
      }

      // try type converter on target type
      var targetConverter = TypeDescriptor.GetConverter(targetType);
      if (targetConverter != null && targetConverter.CanConvertFrom(value.GetType()))
      {
        return targetConverter.ConvertFrom(null, cultureInfo, value);
      }

      // try type converter on source type
      var sourceConverter = TypeDescriptor.GetConverter(sourceType);
      if (sourceConverter != null && sourceConverter.CanConvertTo(targetType))
      {
        return sourceConverter.ConvertTo(null, cultureInfo, value, targetType);
      }

      // String to Enum
      if(targetType.IsEnum && value is string s)
      {
        return Enum.Parse(targetType, s, true);
      }

      // try if both source and target are IConvertible
      if (value is IConvertible && typeof(IConvertible).IsAssignableFrom(targetType))
      {
        return Convert.ChangeType(value, targetType, cultureInfo);
      }

      throw new InvalidCastException($"Cannot convert {value.GetType()} to {targetType}.");
    }
  }
}

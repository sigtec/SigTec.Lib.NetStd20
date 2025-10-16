namespace SigTec.Lib.NetStd20.Collections.Generic
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Runtime.CompilerServices;

  public static class ReadOnlyDictionaryExtensions
  {
    /// <summary>
    /// Converts a format string with named placeholders into a FormattableString,
    /// replacing placeholders with dictionary values. Supports alignment and format specifiers.
    /// </summary>
    public static FormattableString ToFormattableString<T>(
        this IReadOnlyDictionary<string, T> dict,
        string format)
    {
      var parsedFormatString = ToFormattableStringInternal.Parse(format);
      var args = parsedFormatString.Keys.Select(s => (object?)dict[s]).ToArray();
      return FormattableStringFactory.Create(parsedFormatString.Format, args);
    }
  }
}

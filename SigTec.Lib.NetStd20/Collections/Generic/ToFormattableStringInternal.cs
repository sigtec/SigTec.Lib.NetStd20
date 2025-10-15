namespace SigTec.Lib.NetStd20.Collections.Generic
{
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Text;
  using System.Text.RegularExpressions;

  internal static class ToFormattableStringInternal
  {
    // Regex now supports optional alignment and format specifiers.
    // Matches {identifier[,alignment][:format]} but skips escaped {{ and }}.
    private static readonly Regex _placeholderRegex = new(
        @"(?<!\{)\{(?<name>[A-Za-z_][A-Za-z0-9_]*)(?<align>,[0-9]+)?(?<format>:[^}]*)?\}(?!\})",
        RegexOptions.Compiled);

    internal class ParsedFormatString
    {
      public ParsedFormatString(string format, IEnumerable<string> keys)
      {
        Format = format;
        Keys = keys;
      }

      public string Format { get; }
      public IEnumerable<string> Keys { get; }
    }


    // Optional cache: compiled positional format strings per original template
    // for performance (especially if used repeatedly with the same format).
    private static readonly ConcurrentDictionary<string, ParsedFormatString> _formatCache = new();

    internal static ParsedFormatString Parse(string format)
    {
      if (_formatCache.TryGetValue(format, out var parsedFormatString))
      {
        return parsedFormatString;
      }

      var keys = new List<string>();
      var sb = new StringBuilder();
      int lastIndex = 0;

      foreach (Match m in _placeholderRegex.Matches(format))
      {
        // Copy literal section before match
        sb.Append(format, lastIndex, m.Index - lastIndex);

        string name = m.Groups["name"].Value;
        string align = m.Groups["align"].Value;   // e.g. ",-10"
        string fmt = m.Groups["format"].Value;    // e.g. ":s"

        int argIndex = keys.Count;
        keys.Add(name);

        // Replace with positional placeholder
        sb.Append('{')
                 .Append(argIndex)
                 .Append(align)
                 .Append(fmt)
                 .Append('}');

        lastIndex = m.Index + m.Length;
      }

      // Append the trailing literal section
      sb.Append(format, lastIndex, format.Length - lastIndex);

      var result = new ParsedFormatString(sb.ToString(), keys);

      // Cache format string for reuse
      _formatCache.TryAdd(format, result);

      return result;
    }
  }
}

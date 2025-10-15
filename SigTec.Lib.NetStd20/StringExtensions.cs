using System;

namespace SigTec.Lib.NetStd20
{
  public static class StringExtensions
  {
    public static bool Contains(this string s, string v, StringComparison comparison) => s.IndexOf(v, comparison) >= 0;
  }
}

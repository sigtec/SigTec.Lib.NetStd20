namespace SigTec.Lib.NetStd20
{
  using System;

  public static class TypeExtensions
  {
    public static bool IsNullable(this Type type) => !type.IsValueType || (Nullable.GetUnderlyingType(type) != null);
  }
}

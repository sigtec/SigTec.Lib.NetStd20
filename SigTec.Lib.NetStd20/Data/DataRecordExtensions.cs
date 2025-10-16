namespace SigTec.Lib.NetStd20.Data
{
  using System.Collections.Generic;
  using System.Data;

  public static class DataRecordExtensions
  {
    public static IReadOnlyDictionary<string, object?> ToDictionary(this IDataRecord record) => new DataRecordWrapper(record);

    public static object[] GetValues(this IDataRecord record)
    {
      var result = new object[record.FieldCount];
      record.GetValues(result);
      return result;
    }

    public static T GetValue<T>(this IDataRecord record)
    {
      var values = record.GetValues();
      return Converter.ConvertTo<T>(values[0]);
    }

    public static (T0, T1) GetValues<T0, T1>(this IDataRecord record)
    {
      var values = record.GetValues();
      return (Converter.ConvertTo<T0>(values[0]), Converter.ConvertTo<T1>(values[1]));
    }

    public static (T0, T1, T2) GetValues<T0, T1, T2>(this IDataRecord record)
    {
      var values = record.GetValues();
      return (Converter.ConvertTo<T0>(values[0]), Converter.ConvertTo<T1>(values[1]), Converter.ConvertTo<T2>(values[2]));
    }

    public static (T0, T1, T2, T3) GetValues<T0, T1, T2, T3>(this IDataRecord record)
    {
      var values = record.GetValues();
      return (Converter.ConvertTo<T0>(values[0]), Converter.ConvertTo<T1>(values[1]), Converter.ConvertTo<T2>(values[2]), Converter.ConvertTo<T3>(values[3]));
    }

    public static (T0, T1, T2, T3, T4) GetValues<T0, T1, T2, T3, T4>(this IDataRecord record)
    {
      var values = record.GetValues();
      return (Converter.ConvertTo<T0>(values[0]), Converter.ConvertTo<T1>(values[1]), Converter.ConvertTo<T2>(values[2]), Converter.ConvertTo<T3>(values[3]), Converter.ConvertTo<T4>(values[4]));
    }


  }
}

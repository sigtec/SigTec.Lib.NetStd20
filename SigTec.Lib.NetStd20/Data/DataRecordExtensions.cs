using System.Collections.Generic;
using System.Data;

namespace SigTec.Lib.NetStd20.Data
{
  public static class DataRecordExtensions
  {
    public static IReadOnlyDictionary<string, object?> ToDictionary(this IDataRecord record) => new DataRecordWrapper(record);
  }
}

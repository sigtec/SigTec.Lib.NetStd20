namespace SigTec.Lib.NetStd20.Data
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;

  internal class DataRecordWrapper : IReadOnlyDictionary<string, object?>
  {
    readonly IDataRecord _record;

    private static object? GetValueOrNull(object? value) => value is DBNull ? null : value;

    public DataRecordWrapper(IDataRecord record)
    {
      _record = record;
    }

    public object? this[string key] => GetValueOrNull(_record[key]);

    public IEnumerable<string> Keys => Enumerable.Range(0, _record.FieldCount).Select(i => _record.GetName(i));

    public IEnumerable<object?> Values
    {
      get
      {
        var values = new object?[_record.FieldCount];
        _record.GetValues(values);
        return values;
      }
    }

    public int Count => _record.FieldCount;

    public bool ContainsKey(string key) => this.Keys.Contains(key);

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
      var values = new object[_record.FieldCount];
      _record.GetValues(values);
      for (int i = 0; i < values.Length; i++)
      {
        yield return new KeyValuePair<string, object?>(_record.GetName(i), values[i]); 
      }
    }

    public bool TryGetValue(string key, out object? value)
    {
      if(!this.ContainsKey(key))
      {
        value = null;
        return false;
      }
      value = this[key];
      return true;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}

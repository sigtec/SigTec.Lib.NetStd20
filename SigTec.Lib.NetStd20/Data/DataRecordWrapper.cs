namespace SigTec.Lib.NetStd20.Data
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;

  internal class DataRecordWrapper : IReadOnlyDictionary<string, object?>
  {
    private readonly IReadOnlyDictionary<string, object?> _values;

    public DataRecordWrapper(IDataRecord record)
    {
      var data = record.GetValues();
      _values = Enumerable.Range(0, data.Length).ToDictionary(
        i => record.GetName(i),
        i => record.IsDBNull(i) ? null : data[i]
      );
    }

    public object? this[string key] => _values[key];

    public IEnumerable<string> Keys => _values.Keys;

    public IEnumerable<object?> Values => _values.Values;

    public int Count => _values.Count;

    public bool ContainsKey(string key)
    {
      return _values.ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
      return _values.GetEnumerator();
    }

    public bool TryGetValue(string key, out object? value)
    {
      return _values.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable)_values).GetEnumerator();
    }
  }
}

namespace SigTec.Lib.NetStd20.Collections.Generic
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;

  public class OverlayDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
  {
    private readonly IReadOnlyDictionary<TKey, TValue>? _base = null;
    private readonly Dictionary<TKey, TValue> _overlay = new Dictionary<TKey, TValue>();

    /// <summary>
    /// Parameterless Constructor for JSON serialization support
    /// </summary>
    public OverlayDictionary()
    {

    }

    public OverlayDictionary(IReadOnlyDictionary<TKey, TValue> @base) : this()
    {
      _base = @base;
    }

    /// <summary>
    /// Add-Method for JSON serialization support 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(TKey key, TValue value) => _overlay.Add(key, value);

    public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> values)
    {
      foreach (var kvp in values)
      {
        _overlay.Add(kvp.Key, kvp.Value);
      }
    }

    /// <summary>
    /// clears the overlay, keeps the base entries.
    /// </summary>
    public void Reset() => _overlay.Clear();

    public TValue this[TKey key]
    {
      get => this.TryGetValue(key, out var value) ? value : throw new KeyNotFoundException();
      set => _overlay[key] = value;
    }

    public IEnumerable<TKey> Keys => (_base == null) ? _overlay.Keys : _base.Keys.Concat(_overlay.Keys).Distinct();

    public IEnumerable<TValue> Values
    {
      get
      {
        foreach (var key in this.Keys)
        {
          yield return this[key];
        }
      }
    }

    public int Count => this.Keys.Count();

    public bool ContainsKey(TKey key) => _overlay.ContainsKey(key) | _base?.ContainsKey(key) ?? false;

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      foreach (var key in this.Keys)
      {
        yield return new KeyValuePair<TKey, TValue>(key, this[key]);
      }
    }

    public bool TryGetValue(TKey key, out TValue value) => _overlay.TryGetValue(key, out value) | _base?.TryGetValue(key, out value) ?? false;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}

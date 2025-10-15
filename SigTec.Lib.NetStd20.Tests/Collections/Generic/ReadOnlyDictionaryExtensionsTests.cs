namespace SigTec.Lib.NetStd20.Tests.Collections.Generic
{
  using SigTec.Lib.NetStd20.Collections.Generic;
  using System.Globalization;

  [TestClass]
  public class ReadOnlyDictionaryExtensionsTests
  {
    [TestMethod]
    public void ReplacesSimpleKeysCorrectly()
    {
      var dict = new Dictionary<string, object?>
      {
        ["name"] = "Alice",
        ["age"] = 30
      };

      var fs = dict.ToFormattableString("Name: {name}, Age: {age}");

      string result = fs.ToString();
      Assert.AreEqual("Name: Alice, Age: 30", result);
    }

    [TestMethod]
    public void SupportsFormatSpecifiers()
    {
      var dict = new Dictionary<string, object?>
      {
        ["date"] = new DateTime(2024, 12, 31, 23, 59, 0)
      };

      var fs = dict.ToFormattableString("Date: {date:yyyy-MM-dd HH:mm}");
      string result = fs.ToString();

      Assert.AreEqual("Date: 2024-12-31 23:59", result);
    }

    [TestMethod]
    public void SupportsAlignmentSpecifiers()
    {
      var dict = new Dictionary<string, object?>
      {
        ["value"] = 42
      };

      var fs = dict.ToFormattableString("Value:{value,5}End");
      string result = fs.ToString();

      // "Value:" + "   42" + "End"
      Assert.AreEqual("Value:   42End", result);
    }

    [TestMethod]
    public void EscapedBracesArePreserved()
    {
      var dict = new Dictionary<string, object?>
      {
        ["name"] = "Bob"
      };

      var fs = dict.ToFormattableString("{{ User: {name} }}");
      string result = fs.ToString();

      // Escaped braces become single literal braces
      Assert.AreEqual("{ User: Bob }", result);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void ThrowsOnMissingKey()
    {
      var dict = new Dictionary<string, object?>
      {
        ["existing"] = 123
      };

      _ = dict.ToFormattableString("Missing: {missingKey}");
    }

    [TestMethod]
    public void EmptyFormatReturnsSameString()
    {
      var dict = new Dictionary<string, object?>();
      var fs = dict.ToFormattableString("No placeholders here.");
      Assert.AreEqual("No placeholders here.", fs.ToString());
    }

    [TestMethod]
    public void SupportsMultipleDifferentFormats_CacheIndependence()
    {
      var dict = new Dictionary<string, object?>
      {
        ["x"] = 1,
        ["y"] = 2
      };

      var fs1 = dict.ToFormattableString("{x}+{y}={x}");
      var fs2 = dict.ToFormattableString("{y},{x}");

      Assert.AreEqual("1+2=1", fs1.ToString());
      Assert.AreEqual("2,1", fs2.ToString());
    }

    [TestMethod]
    public void CachedFormatIsReusedAndProducesCorrectOutput()
    {
      // Same format, different values → should reuse cache internally
      const string template = "{a}-{b}";
      var dict1 = new Dictionary<string, object> { ["a"] = 1, ["b"] = 2 };
      var dict2 = new Dictionary<string, object> { ["a"] = "X", ["b"] = "Y" };

      var fs1 = dict1.ToFormattableString(template);
      var fs2 = dict2.ToFormattableString(template);

      Assert.AreEqual("1-2", fs1.ToString());
      Assert.AreEqual("X-Y", fs2.ToString());
    }

    [TestMethod]
    public void WorksWithReadonlyDictionaryInterface()
    {
      IReadOnlyDictionary<string, object> dict = new Dictionary<string, object>
      {
        ["user"] = "Charlie"
      };

      var fs = dict.ToFormattableString("Hello {user}!");
      Assert.AreEqual("Hello Charlie!", fs.ToString());
    }

    [TestMethod]
    public void SupportsNumericFormatting()
    {
      var dict = new Dictionary<string, object>
      {
        ["value"] = 1234.567
      };

      var fs = dict.ToFormattableString("{value:N2}");
      Assert.AreEqual("1,234.57", fs.ToString(CultureInfo.InvariantCulture));
    }
  }
}

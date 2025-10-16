namespace SigTec.Lib.NetStd20.Tests
{

  [TestClass]
  public class ConverterTests
  {
    [TestMethod]
    public void ConvertIntToStringTest()
    {
      var input = 42;
      var expected = input.ToString();
      var actual = Converter.ConvertTo<string>(input);
      Assert.AreEqual(expected, actual);
    }

    [Flags]
    enum TestEnum
    {
      None = 0,
      One = 1,
      Two = 2,
      Four = 4,
      All = -1
    }

    [TestMethod]
    public void ConvertEnumFlagsToStringTest()
    {
      var input = TestEnum.Two | TestEnum.Four;
      var expected = input.ToString();
      var actual = Converter.ConvertTo<string>(input);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ConvertStringToEnumFlagsTest()
    {
      var input = "Two, Four";
      var expected = TestEnum.Two | TestEnum.Four;
      var actual = Converter.ConvertTo<TestEnum>(input);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ConvertTimeSpanFromString()
    {
      var input = "00:42:00";
      var expected = TimeSpan.FromMinutes(42);
      var actual = Converter.ConvertTo<TimeSpan>(input);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ConvertBoolFromString()
    {
      var input = "true";
      var expected = true;
      var actual = Converter.ConvertTo<bool>(input);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ConvertBoolFromIntTrue()
    {
      var input = 1;
      var expected = true;
      var actual = Converter.ConvertTo<bool>(input);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ConvertBoolFromIntFalse()
    {
      var input = 0;
      var expected = false;
      var actual = Converter.ConvertTo<bool>(input);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ConvertNullToNullableInt()
    {
      var input = (object?)null;
      var actual = Converter.ConvertTo<int?>(input);
      Assert.IsFalse(actual.HasValue);
    }
  }
}
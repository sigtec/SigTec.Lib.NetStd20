using SigTec.Lib.NetStd20.Reflection;

namespace SigTec.Lib.NetStd20.Tests.Reflection
{
    [TestClass]
    public class ReflectionTests
    {
        [TestMethod]
        public void SimpleReflectToDictionaryTest()
        {
            var today = DateTime.Today;
            var instance = new
            {
                Foo = 1,
                Bar = "bar",
                today
            };
            var actual = instance.Reflect();
            Assert.AreEqual(1, actual["Foo"]);
            Assert.AreEqual("bar", actual["Bar"]);
            Assert.AreEqual(today, actual["today"]);
        }
    }
}

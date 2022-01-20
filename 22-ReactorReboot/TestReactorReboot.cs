using Microsoft.VisualStudio.TestTools.UnitTesting;
using static _22_ReactorReboot.InputReader;

namespace _22_ReactorReboot
{
  [TestClass]
  public class TestReactorReboot
  {
    [TestMethod]
    [DataRow("on x=10..12", true)]
    [DataRow("off x=10..12", false)]
    public void ReadOnOffWorks(string line, bool on)
    {
      var input = InterpretLine(line);
      Assert.AreEqual(input.On, on);
    }

    [TestMethod]
    [DataRow("on x=10..12", 10)]
    [DataRow("on x=5..12", 5)]
    public void ReadX_Works(string line, int expectedX)
    {
      var input = InterpretLine(line);
      Assert.AreEqual(input.Start.X, expectedX);
    }
  }
}
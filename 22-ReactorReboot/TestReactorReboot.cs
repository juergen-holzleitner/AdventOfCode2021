using Microsoft.VisualStudio.TestTools.UnitTesting;
using static _22_ReactorReboot.InputReader;

namespace _22_ReactorReboot
{
  [TestClass]
  public class TestReactorReboot
  {
    [TestMethod]
    [DataRow("on x=10..12,y=3..7,z=2..9", true, 10, 3, 2)]
    [DataRow("off x=10..12,y=3..7,z=2..9", false, 10, 3, 2)]
    [DataRow("on x=5..12,y=3..7,z=2..9", true, 5, 3, 2)]
    public void Read_Start_Works(string line, bool on, int expectedX, int expectedY, int expectedZ)
    {
      var input = InterpretLine(line);
      Assert.AreEqual(input.On, on);
      Assert.AreEqual(input.Start.X, expectedX);
      Assert.AreEqual(input.Start.Y, expectedY);
      Assert.AreEqual(input.Start.Z, expectedZ);
    }
  }
}
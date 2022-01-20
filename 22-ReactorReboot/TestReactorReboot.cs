using Microsoft.VisualStudio.TestTools.UnitTesting;
using static _22_ReactorReboot.InputReader;

namespace _22_ReactorReboot
{
  [TestClass]
  public class TestReactorReboot
  {
    [TestMethod]
    [DataRow("on x=10..12,y=3..7,z=2..9", true, 10, 3, 2, 12, 7, 9)]
    [DataRow("off x=10..12,y=3..7,z=2..9", false, 10, 3, 2, 12, 7, 9)]
    [DataRow("on x=5..12,y=3..7,z=2..9", true, 5, 3, 2, 12, 7, 9)]
    public void Read_Start_Works(string line, bool on, int expectedStartX, int expectedStartY, int expectedStartZ, int expectedEndX, int expectedEndY, int expectedEndZ)
    {
      var input = InterpretLine(line);
      Assert.AreEqual(input.On, on);
      Assert.AreEqual(input.Start.X, expectedStartX);
      Assert.AreEqual(input.Start.Y, expectedStartY);
      Assert.AreEqual(input.Start.Z, expectedStartZ);

      Assert.AreEqual(input.End.X, expectedEndX);
      Assert.AreEqual(input.End.Y, expectedEndY);
      Assert.AreEqual(input.End.Z, expectedEndZ);
    }
  }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _22_ReactorReboot
{
  [TestClass]
  public class TestReactorReboot
  {
    [TestMethod]
    public void ReadOnWorks()
    {
      var input = InputReader.InterpretLine("on");
      Assert.AreEqual(input.On, true);
    }

    [TestMethod]
    public void ReadOffWorks()
    {
      var input = InputReader.InterpretLine("off");
      Assert.AreEqual(input.On, false);
    }

    [TestMethod]
    public void ReadX_Works()
    {
      var input = InputReader.InterpretLine("off on x=10..12");
      Assert.AreEqual(input.Start.X, 10);
    }
  }
}
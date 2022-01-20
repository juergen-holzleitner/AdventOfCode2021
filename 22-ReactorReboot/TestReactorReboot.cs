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
      var intpur = InputReader.InterpretLine("off");
      Assert.AreEqual(intpur.On, false);
    }
  }
}
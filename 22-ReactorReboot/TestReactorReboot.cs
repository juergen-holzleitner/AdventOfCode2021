using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _22_ReactorReboot
{
  [TestClass]
  public class TestReactorReboot
  {
    [TestMethod]
    public void ReadOnOffWorks()
    {
      var input = InputReader.InterpretLine("on");
      Assert.AreEqual(input.On, true);
    }
  }
}
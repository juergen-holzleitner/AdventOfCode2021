using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _23_Amphipod
{
  [TestClass]
  public class TestAmphipod
  {
    [TestMethod]
    public void HallwayHasElevenPositions()
    {
      var hallway = new Hallway();
      Assert.AreEqual(10, hallway.MaxPosition);
      Assert.AreEqual(0, hallway.MinPosition);
    }
  }
}
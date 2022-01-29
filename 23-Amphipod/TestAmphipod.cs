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
      Assert.AreEqual(10, Hallway.MaxPosition);
      Assert.AreEqual(0, Hallway.MinPosition);
    }

    [TestMethod]
    public void SideRoomHasTwoPositions()
    {
      var sideRoom = new SideRoom(0, 'A', new char[] { });
      Assert.AreEqual(2, SideRoom.NumPositions);
    }

    [TestMethod]
    [DataRow(2, 'A')]
    [DataRow(4, 'B')]
    [DataRow(6, 'C')]
    [DataRow(8, 'D')]
    public void SideRoomHasCorrectProperties(int position, char targetAmphipod)
    {
      var sideRoom = new SideRoom(position, targetAmphipod, new char[] { });
      Assert.AreEqual(position, sideRoom.HallwayPosition);
      Assert.AreEqual(targetAmphipod, sideRoom.TargetAmphipod);
    }

    [TestMethod]
    public void SideRoomHasCorrectStartingAmphipods()
    {
      var sideRoom = new SideRoom(0, 'A', new char[]{ 'B', 'A'});
      Assert.AreEqual('B', sideRoom.GetAmphipodAt(0));
      Assert.AreEqual('A', sideRoom.GetAmphipodAt(1));
    }
  }
}
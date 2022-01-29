using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace _23_Amphipod
{
  [TestClass]
  public class TestAmphipod
  {
    [TestMethod]
    [DataRow(11)]
    [DataRow(3)]
    public void HallwayHasPositions(int numPositions)
    {
      var hallway = new Hallway(numPositions);
      Assert.AreEqual(numPositions, hallway.NumPositions);
    }

    [TestMethod]
    [DataRow(2, 'A')]
    [DataRow(4, 'B')]
    [DataRow(6, 'C')]
    [DataRow(8, 'D')]
    public void SideRoomHasCorrectProperties(int position, char targetAmphipod)
    {
      var sideRoom = new SideRoom(position, targetAmphipod, Array.Empty<char?>());
      Assert.AreEqual(position, sideRoom.HallwayPosition);
      Assert.AreEqual(targetAmphipod, sideRoom.TargetAmphipod);
    }

    [TestMethod]
    public void SideRoomHasCorrectStartingAmphipods()
    {
      var sideRoom = new SideRoom(0, 'A', new char?[]{ 'B', 'A'});
      Assert.AreEqual('B', sideRoom.GetAmphipodAt(0));
      Assert.AreEqual('A', sideRoom.GetAmphipodAt(1));
    }

    [TestMethod]
    public void SideRoomIsFinal()
    {
      var sideRoom = new SideRoom(0, 'A', new char?[] { 'A' });
      Assert.IsTrue(sideRoom.IsFinal());
    }

    [TestMethod]
    public void SideRoomIsNotFinal()
    {
      var sideRoom = new SideRoom(0, 'A', new char?[] { 'B' });
      Assert.IsFalse(sideRoom.IsFinal());
    }

    [TestMethod]
    public void CanCreateBurrowThatIsFinal()
    {
      var hallway = new Hallway(3);
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'A' });
      var burrow = new Burrow(hallway, sideRoom);
      Assert.IsTrue(burrow.IsFinal());
    }

    [TestMethod]
    public void CanCreateBurrowThatIsNotFinal()
    {
      var hallway = new Hallway(3);
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'B' });
      var burrow = new Burrow(hallway, sideRoom);
      Assert.IsFalse(burrow.IsFinal());
    }

    [TestMethod]
    public void SideRoomCanMoveOut()
    {
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'B' });
      var moveOut = sideRoom.MoveOut();
      Assert.IsNotNull(moveOut);
      Assert.IsNull(sideRoom.GetAmphipodAt(0));
    }

    [TestMethod]
    public void SideRoomHasCanMoveOut()
    {
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'B' });
      Assert.IsTrue(sideRoom.CanMoveOut());
    }

    [TestMethod]
    public void SideRoomCanNotMoveOutIfFinal()
    {
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'A' });
      Assert.IsFalse(sideRoom.CanMoveOut());
    }

    [TestMethod]
    public void SideRoomThrowsIfItCanNotMoveOut()
    {
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'A' });
      Assert.ThrowsException<InvalidOperationException>( () => sideRoom.MoveOut());
    }

    [TestMethod]
    public void SideRoomCanNotMoveOutIfEmpty()
    {
      var sideRoom = new SideRoom(1, 'A', new char?[] { null });
      Assert.IsFalse(sideRoom.CanMoveOut());
    }

    [TestMethod]
    public void CanNotMoveOutTwice()
    {
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'B' });
      _ = sideRoom.MoveOut();
      Assert.IsFalse(sideRoom.CanMoveOut());
    }

    [TestMethod]
    public void SideRoomCanBeCloned()
    {
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'B' });
      var otherRoom = sideRoom.Clone();
      sideRoom.MoveOut();
      Assert.AreEqual('B', otherRoom.GetAmphipodAt(0));
    }

    [TestMethod]
    public void CanGetPossibleNextBurrowConfigurations()
    {
      var hallway = new Hallway(3);
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'B' });
      var burrow = new Burrow(hallway, sideRoom);
      var nextBurrowConfigs = burrow.GetAllFollowingConfigs();
      foreach (var n in nextBurrowConfigs)
        Assert.IsNull(n.SideRoom.GetAmphipodAt(0));
    }
  }
}
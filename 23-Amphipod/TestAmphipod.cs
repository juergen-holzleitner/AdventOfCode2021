using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
      var burrow = new Burrow(hallway, new List<SideRoom>() { sideRoom });
      Assert.IsTrue(burrow.IsFinal());
    }

    [TestMethod]
    public void CanCreateBurrowThatIsNotFinal()
    {
      var hallway = new Hallway(3);
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'B' });
      var burrow = new Burrow(hallway, new List<SideRoom>() { sideRoom });
      Assert.IsFalse(burrow.IsFinal());
    }

    [TestMethod]
    public void SideRoomCanMoveOut()
    {
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'B' });
      sideRoom.MoveOut();
      Assert.IsNull(sideRoom.GetAmphipodAt(0));
    }

    [TestMethod]
    public void SideRoomCanGetMoveOutAmphipod()
    {
      var sideRoom = new SideRoom(0, 'A', new char?[] { 'B' });
      var amphipod = sideRoom.GetMoveOutAmphipod();
      Assert.AreEqual('B', amphipod);
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
      sideRoom.MoveOut();
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
    public void HallwayIsInitiallyEmpty()
    {
      var hallway = new Hallway(1);
      Assert.IsNull(hallway.GetAmphipodAt(0));
    }

    [TestMethod]
    public void HallwayCanMoveIn()
    {
      var hallway = new Hallway(1);
      hallway.MoveIn(0, 'A');
      Assert.AreEqual('A', hallway.GetAmphipodAt(0));
    }

    [TestMethod]
    public void HallwayThrowsIfPositionIsOccupied()
    {
      var hallway = new Hallway(1);
      hallway.MoveIn(0, 'A');
      Assert.ThrowsException<InvalidOperationException>(() => hallway.MoveIn(0, 'A'));
    }

    [TestMethod]
    public void HallwayCanBeCloned()
    {
      var hallway = new Hallway(1);
      hallway.MoveIn(0, 'A');
      var hallway2 = hallway.Clone();
      Assert.AreEqual('A', hallway2.GetAmphipodAt(0));
    }

    [TestMethod]
    public void HallwayCanMoveInIfEmpty()
    {
      var hallway = new Hallway(1);
      Assert.IsTrue(hallway.CanMoveIn(0));
    }

    [TestMethod]
    public void HallwayCanNotMoveInIfOccupied()
    {
      var hallway = new Hallway(1);
      hallway.MoveIn(0, 'A');
      Assert.IsFalse(hallway.CanMoveIn(0));
    }

    [TestMethod]
    public void HallwayCanNotMoveInIfWayIsOccupied()
    {
      var hallway = new Hallway(3);
      hallway.MoveIn(1, 'A');
      Assert.IsFalse(hallway.CanMoveInFrom(0, 2));
    }

    [TestMethod]
    public void CanGetPossibleNextBurrowConfigurations()
    {
      var hallway = new Hallway(2);
      var sideRoom = new SideRoom(1, 'A', new char?[] { 'B' });
      var burrow = new Burrow(hallway, new List<SideRoom>() { sideRoom });
      var nextBurrowConfigs = burrow.GetAllFollowingConfigs();
      Assert.AreEqual(1, nextBurrowConfigs.Count());
      var (nextConfig, _) = nextBurrowConfigs.First();
      Assert.IsNull(nextConfig.SideRooms[0].GetAmphipodAt(0));
      Assert.AreEqual('B', nextConfig.Hallway.GetAmphipodAt(0));
      Assert.IsNull(nextConfig.Hallway.GetAmphipodAt(1));
    }

    [TestMethod]
    public void CanNotMoveOverOtherAmphipods()
    {
      var hallway = new Hallway(3);
      hallway.MoveIn(1, 'A');
      var sideRoom = new SideRoom(0, 'A', new char?[] { 'B' });
      var burrow = new Burrow(hallway, new List<SideRoom>() { sideRoom });
      var nextBurrowConfigs = burrow.GetAllFollowingConfigs();
      Assert.AreEqual(0, nextBurrowConfigs.Count());
    }

    [TestMethod]
    public void SidewayCanNotMoveInIfInvalidTarget()
    {
      var sideRoom = new SideRoom(0, 'A', new char?[] { null });
      Assert.IsFalse(sideRoom.CanMoveIn('B'));
    }

    [TestMethod]
    public void SidewayCanMoveInIfFree()
    {
      var sideRoom = new SideRoom(0, 'A', new char?[] { null });
      Assert.IsTrue(sideRoom.CanMoveIn('A'));
    }

    [TestMethod]
    public void SidewayCanNotMoveInIfOthersAreIn()
    {
      var sideRoom = new SideRoom(0, 'A', new char?[] { null, 'B' });
      Assert.IsFalse(sideRoom.CanMoveIn('A'));
    }

    [TestMethod]
    public void SidewayMoveInOccupiesPlace()
    {
      var sideRoom = new SideRoom(0, 'A', new char?[] { null });
      sideRoom.MoveIn('A');
      Assert.AreEqual('A', sideRoom.GetAmphipodAt(0));
    }

    [TestMethod]
    public void SidewayMoveInThrowsIfNotPossible()
    {
      var sideRoom = new SideRoom(0, 'A', new char?[] { null });
      Assert.ThrowsException<InvalidOperationException>(() => sideRoom.MoveIn('B'));
    }

    [TestMethod]
    public void SidewayMoveInOccupiesTheLastPlace()
    {
      var sideRoom = new SideRoom(0, 'A', new char?[] { null, null, 'A' });
      sideRoom.MoveIn('A');
      Assert.AreEqual('A', sideRoom.GetAmphipodAt(1));
    }

    [TestMethod]
    public void CanMoveIntoFreeSideRoom()
    {
      var hallway = new Hallway(3);
      hallway.MoveIn(1, 'A');
      var sideRoom = new SideRoom(0, 'A', new char?[] { null });
      var burrow = new Burrow(hallway, new List<SideRoom>() { sideRoom });
      var nextBurrowConfigs = burrow.GetAllFollowingConfigs();
      var (nextConfig, _) = nextBurrowConfigs.Single();
      Assert.IsNull(nextConfig.Hallway.GetAmphipodAt(1));
      Assert.AreEqual('A', nextConfig.SideRooms[0].GetAmphipodAt(0));
    }

    [TestMethod]
    public void CanNotMoveIntoFreeSideRoomIfWayIsBlocked()
    {
      var hallway = new Hallway(3);
      hallway.MoveIn(2, 'A');
      hallway.MoveIn(1, 'B');
      var sideRoom = new SideRoom(0, 'A', new char?[] { null });
      var burrow = new Burrow(hallway, new List<SideRoom>() { sideRoom });
      var nextBurrowConfigs = burrow.GetAllFollowingConfigs();
      Assert.AreEqual(0, nextBurrowConfigs.Count());
    }

    [TestMethod]
    public void HallwayCanMoveOutIsFalseIfNone()
    {
      var hallway = new Hallway(1);
      Assert.IsFalse(hallway.CanMoveOut(0));
    }

    [TestMethod]
    public void HallwayCanMoveOutIsTrueIfAny()
    {
      var hallway = new Hallway(1);
      hallway.MoveIn(0, 'A');
      Assert.IsTrue(hallway.CanMoveOut(0));
    }

    [TestMethod]
    public void HallwayThrowsIfCanNotMoveOut()
    {
      var hallway = new Hallway(1);
      Assert.ThrowsException<InvalidOperationException>(() => hallway.MoveOut(0));
    }

    [TestMethod]
    public void HallwayPositionIsNullAfterMoveOut()
    {
      var hallway = new Hallway(1);
      hallway.MoveIn(0, 'A');
      hallway.MoveOut(0);
      Assert.IsNull(hallway.GetAmphipodAt(0));
    }

    [TestMethod]
    public void CanMoveDirectlyIntoTargetSideRoom()
    {
      var hallway = new Hallway(2);
      var sideRoomA = new SideRoom(0, 'A', new char?[] { 'B' });
      var sideRoomB = new SideRoom(1, 'B', new char?[] { null });
      var burrow = new Burrow(hallway, new List<SideRoom>() { sideRoomA, sideRoomB });
      var nextBurrowConfigs = burrow.GetAllFollowingConfigs();
      Assert.AreEqual(1, nextBurrowConfigs.Count());
    }

    [TestMethod]
    public void CostOfMoveOutIsCorrect()
    {
      var hallway = new Hallway(2);
      var sideRoom = new SideRoom(0, 'A', new char?[] { 'B' });
      var burrow = new Burrow(hallway, new List<SideRoom> { sideRoom });
      var (_, cost) = burrow.GetAllFollowingConfigs().Single();
      Assert.AreEqual(20, cost);
    }

    [TestMethod]
    public void FirstPositionAfterInitialConfigIsFound()
    {
      var startBurrow = BurrowProcessor.GetSampleStartPosition();
      var next = startBurrow.GetAllFollowingConfigs();
      var (sampleStep, cost) = (from b in next
                       where b.Item1.Hallway.GetAmphipodAt(3) == 'B' 
                       && b.Item1.SideRooms[2].GetAmphipodAt(0) is null
                       select b).Single();
      Assert.AreEqual(40, cost);

      next = sampleStep.GetAllFollowingConfigs();
      (sampleStep, cost) = (from b in next
                        where b.Item1.Hallway.GetAmphipodAt(3) == 'B' 
                        && b.Item1.SideRooms[2].GetAmphipodAt(0)=='C'
                        && b.Item1.SideRooms[1].GetAmphipodAt(0) is null
                        select b).Single();
      Assert.AreEqual(400, cost);
    }
  }
}
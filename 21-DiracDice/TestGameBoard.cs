using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _21_DiracDice
{
  [TestClass]
  public class TestGameBoard
  {
    [TestMethod]
    [DataRow(7, 5, 2)]
    [DataRow(8, 1, 9)]
    [DataRow(10, 1, 1)]
    public void Increament_OnGameBoard_ReturnsCorrectValue(int start, int inc, int end)
    {
      var newPos = GameBoard.IncrementPosBy(start, inc);
      Assert.AreEqual(end, newPos);
    }

    [TestMethod]
    public void PawnPosition_CanBeRequested()
    {
      var pawn = new Pawn(3);
      Assert.AreEqual(3, pawn.Position);
    }

    [TestMethod]
    public void Pawn_CanMove()
    {
      var pawn = new Pawn(3);
      pawn.MoveTo(5);
      Assert.AreEqual(5, pawn.Position);
    }

    [TestMethod]
    public void PawnHasInitialScoreOfZero()
    {
      var pawn = new Pawn(3);
      Assert.AreEqual(0, pawn.Score);
    }

    [TestMethod]
    public void PawnScoreIncreasesByTargetPosition()
    {
      var pawn = new Pawn(3);
      pawn.MoveTo(5);
      Assert.AreEqual(5, pawn.Score);
      pawn.MoveTo(7);
      Assert.AreEqual(12, pawn.Score);
    }
  }
}
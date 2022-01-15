using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _21_DiracDice
{
  [TestClass]
  public class TestDiracDice
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

    [TestMethod]
    public void DeterministicDiceStartsWithOne()
    {
      var dice = new DeterministicDice();
      var val = dice.GetNextValue();
      Assert.AreEqual(1, val);
    }

    [TestMethod]
    public void DeterministicDice_Returns_PlusOne_WithEachRoll()
    {
      var dice = new DeterministicDice();
      var val = dice.GetNextValue();
      val = dice.GetNextValue();
      Assert.AreEqual(2, val);
    }

    [TestMethod]
    public void DeterministicDice_StartsOverAfter1000()
    {
      var dice = new DeterministicDice(999);
      var val = dice.GetNextValue();
      Assert.AreEqual(1000, val);

      val = dice.GetNextValue();
      Assert.AreEqual(1, val);
    }

    [TestMethod]
    [DataRow(1000, true)]
    [DataRow(999, false)]
    public void ScoresAreAWinningScores(int score, bool isWinning)
    {
      bool isWinningScore = GameBoard.IsWinningScore(score);
      Assert.AreEqual(isWinning, isWinningScore);
    }
  }
}
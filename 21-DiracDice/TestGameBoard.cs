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
  }
}
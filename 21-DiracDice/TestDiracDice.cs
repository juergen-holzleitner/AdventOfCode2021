using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

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
      _ = dice.GetNextValue();
      int val = dice.GetNextValue();
      Assert.AreEqual(2, val);
    }

    [TestMethod]
    public void DeterministicDice_StartsOverAfter1000()
    {
      var dice = new DeterministicDice(99);
      var val = dice.GetNextValue();
      Assert.AreEqual(100, val);

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

    [TestMethod]
    public void MultipleDiceRolls_ReturnCorrectMovement()
    {
      var dice = new DeterministicDice();
      var movement = dice.GetValueAfterRolls(3);
      Assert.AreEqual(6, movement);
    }

    [TestMethod]
    public void PawnIsAtCorrectPositionAfterFirstMove()
    {
      var pawn = new Pawn(4);
      var dice = new DeterministicDice();
      GameBoard.ProcessMove(pawn, dice);

      Assert.AreEqual(10, pawn.Position);
      Assert.AreEqual(10, pawn.Score);
    }

    [TestMethod]
    public void PawnsHaveCorrectPositionAfterAFewMoves()
    {
      var pawns = new List<Pawn>() {
        new Pawn(4),
        new Pawn(8)
      };

      var dice = new DeterministicDice();

      for (int n = 0; n < 8; ++n)
      {
        GameBoard.ProcessMove(pawns[n % 2], dice);
      }

      Assert.AreEqual(6, pawns[0].Position);
      Assert.AreEqual(26, pawns[0].Score);
      Assert.AreEqual(6, pawns[1].Position);
      Assert.AreEqual(22, pawns[1].Score);
    }

    [TestMethod]
    public void PlayUntilWin()
    {
      var pawns = new List<Pawn>() {
        new Pawn(4),
        new Pawn(8)
      };

      var dice = new DeterministicDice();

      for (int n = 0; ; ++n)
      {
        GameBoard.ProcessMove(pawns[n % 2], dice);
        if (GameBoard.IsWinningScore(pawns[n % 2].Score))
          break;
      }

      Assert.AreEqual(10, pawns[0].Position);
      Assert.AreEqual(1000, pawns[0].Score);
      Assert.AreEqual(3, pawns[1].Position);
      Assert.AreEqual(745, pawns[1].Score);
    }

    [TestMethod]
    public void TestingSampleIsCorrect()
    {
      var pawns = new List<Pawn>() {
        new Pawn(4),
        new Pawn(8)
      };

      var dice = new DeterministicDice();

      int n = 0;
      for (; ; ++n)
      {
        GameBoard.ProcessMove(pawns[n % 2], dice);
        if (GameBoard.IsWinningScore(pawns[n % 2].Score))
          break;
      }

      var otherScore = pawns[(n + 1) % 2].Score;
      int rolls = (n + 1) * 3;
      int result = rolls * otherScore;

      Assert.AreEqual(745, otherScore);
      Assert.AreEqual(993, rolls);
      Assert.AreEqual(739785, result);
    }

    [TestMethod]
    public void Universes_CanCompare_State()
    {
      var p11 = new Pawn(1);
      var p12 = new Pawn(2);
      var universe1 = new Universe(p11, p12);

      var p21 = new Pawn(1);
      var p22 = new Pawn(2);
      var universe2 = new Universe(p21, p22);

      bool equals = universe1.Equals(universe2);
      Assert.IsTrue(equals);
    }

    [TestMethod]
    public void QuantumDie_GeneratesAllNextUniverses()
    {
      var universes = QuantumDie.GenerateUniverses();
      Assert.AreEqual(27, universes.Sum(u => u.Value));
      Assert.AreEqual(3, universes.Min(u => u.Key));
      Assert.AreEqual(9, universes.Max(u => u.Key));
    }

    [TestMethod]
    public void ProcessOneUniverseStep()
    {
      Pawn p1 = new Pawn(4);
      Pawn p2 = new Pawn(8);
      var universe = new Universe(p1, p2);

      var activeUniverses = new Dictionary<Universe, int>()
      {
        {universe, 1 }
      };

      var newUniverses = new Dictionary<Universe, int>();

      foreach (var u in activeUniverses)
      {
        foreach (var d in QuantumDie.GenerateUniverses())
        {
          int n = u.Key.Steps % 2;

          int newPos = GameBoard.IncrementPosBy(u.Key.Pawns[n].Position, d.Key);
          var newList = new List<Pawn>(u.Key.Pawns);
          newList[n] = new Pawn(newList[n].Position);
          newList[n].MoveTo(newPos);

          var newU = new Universe(newList, u.Key.Steps + 1);
          if (newUniverses.ContainsKey(newU))
          {
            newUniverses[newU] += d.Value;
          }
          else
          {
            newUniverses.Add(newU, d.Value);
          }
        }
      }

      Assert.AreEqual(7, newUniverses.Count);
    }

    [TestMethod]
    public void ExampleWithTestValues_ReturnsCorrectValues()
    {
      Pawn p1 = new Pawn(4);
      Pawn p2 = new Pawn(8);
      var universe = new Universe(p1, p2);

      var activeUniverses = new Dictionary<Universe, long>()
    {
      {universe, 1 }
    };

      var winCounts = new long[2] { 0, 0 };

      while (activeUniverses.Any())
      {
        var newUniverses = new Dictionary<Universe, long>();

        foreach (var u in activeUniverses)
        {
          int step = u.Key.Steps % 2;

          foreach (var d in QuantumDie.GenerateUniverses())
          {
            int newPos = GameBoard.IncrementPosBy(u.Key.Pawns[step].Position, d.Key);
            var newPawn = new Pawn(u.Key.Pawns[step]);
            newPawn.MoveTo(newPos);

            long numUniverses = u.Value * d.Value;

            if (newPawn.Score >= 21)
            {
              winCounts[step] += numUniverses;
              continue;
            }

            var newList = new List<Pawn>(u.Key.Pawns);
            newList[step] = newPawn;

            var newU = new Universe(newList, u.Key.Steps + 1);
            if (newUniverses.ContainsKey(newU))
            {
              newUniverses[newU] += numUniverses;
            }
            else
            {
              newUniverses.Add(newU, numUniverses);
            }
          }
        }

        activeUniverses = newUniverses;
      }

      Assert.AreEqual(444356092776315, winCounts[0]);
      Assert.AreEqual(341960390180808, winCounts[1]);
    }
  }
}
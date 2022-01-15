using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21_DiracDice
{
  internal static class GameBoard
  {
    public static int IncrementPosBy(int pos, int increment)
    {
      int zeroBased = pos - 1;
      zeroBased += increment;
      zeroBased %= 10;
      return zeroBased + 1;
    }

    internal static bool IsWinningScore(int score)
    {
      if (score <= 999)
        return false;
      return true;
    }

    internal static void ProcessMove(Pawn pawn, DeterministicDice dice)
    {
      var distance = dice.GetValueAfterRolls(3);
      var newPos = IncrementPosBy(pawn.Position, distance);
      pawn.MoveTo(newPos);
    }
  }
}

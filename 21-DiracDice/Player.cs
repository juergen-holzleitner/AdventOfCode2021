using System;

namespace _21_DiracDice
{
  internal class Pawn
  {
    public Pawn(int startPosition)
    {
      Position = startPosition;
    }

    public int Position { get; internal set; }

    public int Score { get; internal set; } = 0;

    internal void MoveTo(int finalPos)
    {
      Position = finalPos;
      Score += Position;
    }
  }
}
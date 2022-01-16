using System;

namespace _21_DiracDice
{
  internal class Pawn : IEquatable<Pawn>
  {
    public Pawn(int startPosition)
    {
      Position = startPosition;
    }

    public Pawn(Pawn other)
    {
      Position = other.Position;
      Score = other.Score;
    }

    public int Position { get; internal set; }

    public int Score { get; internal set; } = 0;

    public bool Equals(Pawn? other)
    {
      if (other is null) return false;

      if (ReferenceEquals(this, other)) return true;

      if (GetType() != other.GetType()) return false;

      return Position == other.Position && Score == other.Score;
    }

    public override bool Equals(object? obj) => Equals(obj as Pawn);

    internal void MoveTo(int finalPos)
    {
      Position = finalPos;
      Score += Position;
    }

    public override int GetHashCode() => (Position, Score).GetHashCode();

    public static bool operator ==(Pawn lhs, Pawn rhs)
    {
      if (lhs is null)
      {
        if (rhs is null) return true;

        return false;
      }
      return lhs.Equals(rhs);
    }

    public static bool operator !=(Pawn lhs, Pawn rhs) => !(lhs == rhs);
  }
}
using System;

namespace _21_DiracDice
{
  internal class Universe : IEquatable<Universe>
  {
    private readonly Pawn p1;
    private readonly Pawn p2;

    public Universe(Pawn p1, Pawn p2)
    {
      this.p1 = p1;
      this.p2 = p2;
    }

    public override bool Equals(object? obj) => Equals(obj as Universe);

    public bool Equals(Universe? other)
    {
      if (other is null) return false;

      if (ReferenceEquals(this, other)) return true;

      if (GetType() != other.GetType()) return false;

      return p1.Equals(other.p1) && p2.Equals(other.p2);
    }

    public override int GetHashCode() => (p1, p2).GetHashCode();

    public static bool operator ==(Universe lhs, Universe rhs)
    {
      if (lhs is null)
      {
        if (rhs is null) return true;

        return false;
      }

      return lhs.Equals(rhs);
    }

    public static bool operator !=(Universe lhs, Universe rhs) => !(lhs == rhs);

  }
}
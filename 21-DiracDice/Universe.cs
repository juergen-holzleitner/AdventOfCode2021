using System;
using System.Collections.Generic;

namespace _21_DiracDice
{
  internal class Universe : IEquatable<Universe>
  {
    private readonly List<Pawn> pawns;

    public Universe(Pawn p1, Pawn p2)
    {
      pawns = new List<Pawn>() 
      {
        p1,
        p2,
      };
    }

    public Universe(List<Pawn> pawns, int steps)
    {
      this.pawns = pawns;
      Steps = steps;
    }

    public List<Pawn> Pawns { get { return pawns; } }

    public int Steps { get; set; } = 0;

    public override bool Equals(object? obj) => Equals(obj as Universe);

    public bool Equals(Universe? other)
    {
      if (other is null) return false;

      if (ReferenceEquals(this, other)) return true;

      if (GetType() != other.GetType()) return false;

      return Steps == other.Steps && Pawns[0].Equals(other.Pawns[0]) && Pawns[1].Equals(other.Pawns[1]);
    }

    public override int GetHashCode() => (Steps, Pawns[0], Pawns[1]).GetHashCode();

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
using System;
using System.Collections.Generic;
using System.Linq;

namespace _23_Amphipod
{
  internal class Burrow
  {
    private readonly List<SideRoom> sideRooms;
    private readonly Hallway hallway;

    internal List<SideRoom> SideRooms { get => sideRooms; }

    public Burrow(Hallway hallway, List<SideRoom> sideRooms)
    {
      this.sideRooms = sideRooms;
      this.hallway = hallway;
    }

    internal bool IsFinal()
    {
      return sideRooms.All(s => s.IsFinal());
    }

    internal Burrow Clone()
    {
      return new Burrow(hallway.Clone(), sideRooms.Select( s => s.Clone()).ToList());
    }

    internal Hallway Hallway { get => hallway; }

    internal IEnumerable<(Burrow, long)> GetAllFollowingConfigs()
    {
      for (int sideRoom = 0; sideRoom < SideRooms.Count; ++sideRoom)
      {
        if (SideRooms[sideRoom].CanMoveOut())
        {
          for (int n = 0; n < hallway.NumPositions; ++n)
          {
            if (!IsSideRoomEntrence(n))
            {
              if (hallway.CanMoveInFrom(SideRooms[sideRoom].HallwayPosition, n))
              {
                var next = Clone();
                var amphipod = SideRooms[sideRoom].GetMoveOutAmphipod();
                int steps = next.SideRooms[sideRoom].MoveOut();
                steps += Math.Abs(n - next.SideRooms[sideRoom].HallwayPosition);
                next.Hallway.MoveIn(n, amphipod);

                yield return (next, GetMoveCosts(steps, amphipod));
              }
            }
          }

          for (int otherSideRoom = 0; otherSideRoom < SideRooms.Count; ++otherSideRoom)
          {
            if (otherSideRoom != sideRoom)
            {
              var amphipod = SideRooms[sideRoom].GetMoveOutAmphipod();
              if (SideRooms[otherSideRoom].CanMoveIn(amphipod))
              {
                if (hallway.CanMoveTo(SideRooms[sideRoom].HallwayPosition, SideRooms[otherSideRoom].HallwayPosition))
                {
                  var next = Clone();
                  int steps = next.SideRooms[sideRoom].MoveOut();
                  steps += Math.Abs(next.SideRooms[sideRoom].HallwayPosition - next.SideRooms[otherSideRoom].HallwayPosition);
                  steps += next.SideRooms[otherSideRoom].MoveIn(amphipod);

                  yield return (next, GetMoveCosts(steps, amphipod));
                }
              }
            }
          }
        }
      }

      for (int n = 0; n < hallway.NumPositions; ++n)
      {
        if (hallway.GetAmphipodAt(n) is char amphipod)
        {
          for (int sideRoom = 0; sideRoom < SideRooms.Count; ++sideRoom)
          {
            if (SideRooms[sideRoom].CanMoveIn(amphipod))
            {
              if (hallway.CanMoveTo(n, SideRooms[sideRoom].HallwayPosition))
              {
                var next = Clone();
                next.Hallway.MoveOut(n);
                int steps = Math.Abs(n - next.SideRooms[sideRoom].HallwayPosition);
                steps += next.SideRooms[sideRoom].MoveIn(amphipod);
                yield return (next, GetMoveCosts(steps, amphipod));
              }
            }
          }
        }
      }
    }

    private static long GetMoveCosts(int steps, char amphipod)
    {
      return amphipod switch
      {
        'A' => steps,
        'B' => 10 * steps,
        'C' => 100 * steps,
        'D' => 1000 * steps,
        _ => throw new ArgumentException($"{nameof(steps)} has invalid value"),
      };
    }

    private bool IsSideRoomEntrence(int position)
    {
      return SideRooms.Any(s => s.HallwayPosition == position);
    }
  }

  internal class BurrowComparer : EqualityComparer<Burrow>
  {
    public override bool Equals(Burrow? x, Burrow? y)
    {
      if (x == null && y == null)
        return true;

      if (x == null || y == null)
        return false;

      if (!x.Hallway.Equals(y.Hallway))
        return false;

      if (!x.SideRooms.SequenceEqual(y.SideRooms))
        return false;

      return true;
    }

    public override int GetHashCode(Burrow burrow)
    {
      var hash = burrow.GetHashCode();
      foreach (var s in burrow.SideRooms)
        hash ^= s.GetHashCode();
      return hash;
    }
  }
}
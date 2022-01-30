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

    internal IEnumerable<Burrow> GetAllFollowingConfigs()
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
                var move = next.SideRooms[sideRoom].MoveOut();
                next.Hallway.MoveIn(n, move);

                yield return next;
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
                next.SideRooms[sideRoom].MoveIn(amphipod);
                yield return next;
              }
            }
          }
        }
      }
    }

    private bool IsSideRoomEntrence(int position)
    {
      return SideRooms.Any(s => s.HallwayPosition == position);
    }
  }
}
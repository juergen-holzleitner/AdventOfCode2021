using System;
using System.Collections.Generic;

namespace _23_Amphipod
{
  internal class Burrow
  {
    private readonly SideRoom sideRoom;
    private readonly Hallway hallway;

    internal SideRoom SideRoom { get => sideRoom; }

    public Burrow(Hallway hallway, SideRoom sideRoom)
    {
      this.sideRoom = sideRoom;
      this.hallway = hallway;
    }

    internal bool IsFinal()
    {
      return sideRoom.IsFinal();
    }

    internal Burrow Clone()
    {
      return new Burrow(hallway.Clone(), sideRoom.Clone());
    }

    internal IEnumerable<Burrow> GetAllFollowingConfigs()
    {
      if (SideRoom.CanMoveOut())
      {
        var next = Clone();
        _ = next.SideRoom.MoveOut();

        yield return next;
      }
    }
  }
}
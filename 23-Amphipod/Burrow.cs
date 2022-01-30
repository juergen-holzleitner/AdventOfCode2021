﻿using System;
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

    internal Hallway Hallway { get => hallway; }

    internal IEnumerable<Burrow> GetAllFollowingConfigs()
    {
      if (SideRoom.CanMoveOut())
      {
        for (int n = 0; n < hallway.NumPositions; ++n)
        {
          if (!IsSideRoomEntrence(n))
          {
            if (hallway.CanMoveInFrom(SideRoom.HallwayPosition, n))
            {
              var next = Clone();
              var move = next.SideRoom.MoveOut();
              next.Hallway.MoveIn(n, move);

              yield return next;
            }
          }
        }
      }

      for (int n = 0; n < hallway.NumPositions; ++n)
      {
        if (hallway.GetAmphipodAt(n) is char amphipod)
        {
          if (sideRoom.CanMoveIn(amphipod))
          {
            if (hallway.CanMoveTo(n, sideRoom.HallwayPosition))
            {
              var next = Clone();
              next.Hallway.MoveOut(n);
              next.SideRoom.MoveIn(amphipod);
              yield return next;
            }
          }
        }
      }
    }

    private bool IsSideRoomEntrence(int position)
    {
      return sideRoom.HallwayPosition == position;
    }
  }
}
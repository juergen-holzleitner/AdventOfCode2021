using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _23_Amphipod
{
  internal static class BurrowProcessor
  {
    static internal Burrow GetSampleStartPosition()
    {
      var hallway = new Hallway(11);
      var sideRoomA = new SideRoom(2, 'A', new char?[] { 'B', 'A' });
      var sideRoomB = new SideRoom(4, 'B', new char?[] { 'C', 'D' });
      var sideRoomC = new SideRoom(6, 'C', new char?[] { 'B', 'C' });
      var sideRoomD = new SideRoom(8, 'D', new char?[] { 'D', 'A' });
      return new Burrow(hallway, new List<SideRoom>() { sideRoomA, sideRoomB, sideRoomC, sideRoomD });
    }

    internal static void GetFinalBurrow(Burrow startBurrow)
    {
      var current = new List<Burrow>() { startBurrow };

      for (; ; )
      {
        List<Burrow> next = new();
        
        foreach (var burrow in current)
        {
          if (burrow.IsFinal())
            return;
          foreach (var nextBurrow in burrow.GetAllFollowingConfigs())
          {
            next.Add(nextBurrow);
          }
        }

        current = next;
      }
    }
  }
}

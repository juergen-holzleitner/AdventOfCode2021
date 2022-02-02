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

    internal static long GetFinalBurrow(Burrow startBurrow)
    {
      var minReachableBurrowConfigurations = new Dictionary<Burrow, long>(new BurrowComparer());

      var burrows = new PriorityQueue<(Burrow, long), long>();
      burrows.Enqueue((startBurrow, 0), 0);
      for (; ; )
      {
        var burrow = burrows.Dequeue();
        if (burrow.Item1.IsFinal())
          return burrow.Item2;

        foreach (var (nextBurrow, cost) in burrow.Item1.GetAllFollowingConfigs())
        {
          var totalCost = burrow.Item2 + cost;

          if (!minReachableBurrowConfigurations.ContainsKey(nextBurrow))
          {
            burrows.Enqueue((nextBurrow, totalCost), totalCost);
            minReachableBurrowConfigurations.Add(nextBurrow, totalCost);
          }
          else if (minReachableBurrowConfigurations[nextBurrow] > totalCost)
          {
            burrows.Enqueue((nextBurrow, totalCost), totalCost);
            minReachableBurrowConfigurations[nextBurrow] = totalCost;
          }
        }
      }
    }
  }
}

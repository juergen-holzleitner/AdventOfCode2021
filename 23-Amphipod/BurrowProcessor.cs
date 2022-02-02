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

    static internal Burrow GetRealStartPosition()
    {
      var hallway = new Hallway(11);
      var sideRoomA = new SideRoom(2, 'A', new char?[] { 'C', 'B' });
      var sideRoomB = new SideRoom(4, 'B', new char?[] { 'D', 'A' });
      var sideRoomC = new SideRoom(6, 'C', new char?[] { 'D', 'B' });
      var sideRoomD = new SideRoom(8, 'D', new char?[] { 'A', 'C' });
      return new Burrow(hallway, new List<SideRoom>() { sideRoomA, sideRoomB, sideRoomC, sideRoomD });
    }

    internal static long GetFinalBurrow(Burrow startBurrow)
    {
      long positionsCalculated = 0;

      var minReachableBurrowConfigurations = new Dictionary<Burrow, long>(new BurrowComparer());
      long droppedDuplicates = 0;

      long? maxCosts = null;
      long droppedTooExpensive = 0;

      var burrows = new PriorityQueue<(Burrow, long), long>();
      burrows.Enqueue((startBurrow, 0), 0);
      for (; ; )
      {
        var burrow = burrows.Dequeue();
        if (burrow.Item1.IsFinal())
          return burrow.Item2;

        foreach (var (nextBurrow, cost) in burrow.Item1.GetAllFollowingConfigs())
        {
          ++positionsCalculated;
          var totalCost = burrow.Item2 + cost;

          if (nextBurrow.IsFinal())
          {
            if (!maxCosts.HasValue || maxCosts.Value > totalCost)
            {
              maxCosts = totalCost;
            }
          }

          if (!maxCosts.HasValue || totalCost <= maxCosts.Value)
          {
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
            else
            {
              ++droppedDuplicates;
            }
          }
          else
          {
            ++droppedTooExpensive;
          }
        }
      }
    }
  }
}

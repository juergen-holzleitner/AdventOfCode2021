using System;
using System.Collections.Generic;

namespace _22_ReactorReboot
{
  internal class Reactor
  {
    const int limit = 50;

    List<InputReader.Block> enabledCubes = new();

    public Reactor()
    {
    }

    public bool[,,] State { get; set; } = new bool[2 * limit + 1, 2 * limit + 1, 2 * limit + 1];

    internal long GetNumCubesOn()
    {
      long num = 0;
      foreach (var i in State)
        if (i)
          ++num;

      foreach (var b in enabledCubes)
        num += GetBlockSize(b);

      return num;
    }

    internal void ProcessStep(InputReader.Input input)
    {
      for (long x = input.Block.A.X; x <= input.Block.B.X; ++x)
        for (long y = input.Block.A.Y; y <= input.Block.B.Y; ++y)
          for (long z = input.Block.A.Z; z <= input.Block.B.Z; ++z)
          {
            var X = x + limit;
            var Y = y + limit;
            var Z = z + limit;
            if (X < State.GetLength(0) && Y < State.GetLength(1) && Z < State.GetLength(2) && X >= 0 && Y >= 0 && Z >= 0)
              State[X, Y, Z] = input.On;
          }
    }

    internal static IEnumerable<InputReader.Input> LimitInputs(IEnumerable<InputReader.Input> inputs)
    {
      foreach (var input in inputs)
      {
        if (input.Block.B.X < -limit)
          continue;
        if (input.Block.B.Y < -limit)
          continue;
        if (input.Block.B.Z < -limit)
          continue;

        if (input.Block.A.X > limit)
          continue;
        if (input.Block.A.Y > limit)
          continue;
        if (input.Block.A.Z > limit)
          continue;

        var newA = new InputReader.Position(Math.Max(-limit, input.Block.A.X), Math.Max(-limit, input.Block.A.Y), Math.Max(-limit, input.Block.A.Z));
        var newB = new InputReader.Position(Math.Min(limit, input.Block.B.X), Math.Min(limit, input.Block.B.Y), Math.Min(limit, input.Block.B.Z));
        yield return new InputReader.Input(input.On, new InputReader.Block(newA, newB));
      }
    }

    internal void ProcessAddBlockOn(InputReader.Block block)
    {
      enabledCubes.Add(block);
    }

    internal static long GetBlockSize(InputReader.Block block)
    {
      var dX = block.B.X - block.A.X + 1;
      var dY = block.B.Y - block.A.Y + 1;
      var dZ = block.B.Z - block.A.Z + 1;

      return dX * dY * dZ;
    }
  }
}
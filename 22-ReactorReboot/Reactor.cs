using System;

namespace _22_ReactorReboot
{
  internal class Reactor
  {
    public Reactor()
    {
    }

    public bool[,,] State { get; set; } = new bool[101,101,101];

    internal int GetNumCubesOn()
    {
      int num = 0;
      foreach (var i in State)
        if (i)
          ++num;
      return num;
    }

    internal void ProcessStep(InputReader.Input input)
    {
      if (input.Block.A.X > 50) return;
      if (input.Block.A.Y > 50) return;
      if (input.Block.A.Z > 50) return;

      if (input.Block.B.X < -50) return;
      if (input.Block.B.Y < -50) return;
      if (input.Block.B.Z < -50) return;

      for (long x = input.Block.A.X; x <= input.Block.B.X; ++x)
        for (long y = input.Block.A.Y; y <= input.Block.B.Y; ++y)
          for (long z = input.Block.A.Z; z <= input.Block.B.Z; ++z)
          {
            var X = x + 50;
            var Y = y + 50;
            var Z = z + 50;
            if (X < State.GetLength(0) && Y < State.GetLength(1) && Z < State.GetLength(2) && X >= 0 && Y >= 0 && Z >= 0)
              State[X, Y, Z] = input.On;
          }
    }
  }
}
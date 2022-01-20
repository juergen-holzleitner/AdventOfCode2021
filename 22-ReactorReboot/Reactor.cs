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
      if (input.Start.X > 50) return;
      if (input.Start.Y > 50) return;
      if (input.Start.Z > 50) return;

      if (input.End.X < -50) return;
      if (input.End.Y < -50) return;
      if (input.End.Z < -50) return;

      for (long x = input.Start.X; x <= input.End.X; ++x)
        for (long y = input.Start.Y; y <= input.End.Y; ++y)
          for (long z = input.Start.Z; z <= input.End.Z; ++z)
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
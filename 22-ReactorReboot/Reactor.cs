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
      for (int x = input.Start.X; x <= input.End.X; ++x)
        for (int y = input.Start.Y; y <= input.End.Y; ++y)
          for (int z = input.Start.Z; z <= input.End.Z; ++z)
            State[x, y, z] = input.On;
    }
  }
}
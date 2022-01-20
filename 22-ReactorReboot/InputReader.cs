using System;

namespace _22_ReactorReboot
{
  internal class InputReader
  {
    internal static Input InterpretLine(string line)
    {
      if (line == "off")
        return new Input(false, new Position(10));
      return new Input(true, new Position(10));
    }

    public readonly record struct Position(int X);

    public record struct Input(bool On, Position Start);
  }
}
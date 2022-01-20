using System;

namespace _22_ReactorReboot
{
  internal class InputReader
  {
    internal static Input InterpretLine(string line)
    {
      return new Input(true);
    }

    public record struct Input(bool On);
  }
}
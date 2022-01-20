using System;

namespace _22_ReactorReboot
{
  internal class InputReader
  {
    internal static Input InterpretLine(string line)
    {
      if (line == "off")
        return new Input(false);
      return new Input(true);
    }

    public record struct Input(bool On);
  }
}
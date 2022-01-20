using System;
using System.Text.RegularExpressions;

namespace _22_ReactorReboot
{
  internal class InputReader
  {
    internal static Input InterpretLine(string line)
    {
      var regex = new Regex(@"(?<on>on|off) x=(?<StartX>\d+)..(?<EndX>\d+),y=(?<StartY>\d+)..(?<EndY>\d+),z=(?<StartZ>\d+)..(?<EndZ>\d+)");
      var match = regex.Match(line);
      var on = match.Groups["on"].Value == "on";
      var startX = int.Parse(match.Groups["StartX"].Value);
      var startY = int.Parse(match.Groups["StartY"].Value);
      var startZ = int.Parse(match.Groups["StartZ"].Value);
      var endX = int.Parse(match.Groups["EndX"].Value);
      var endY = int.Parse(match.Groups["EndY"].Value);
      var endZ = int.Parse(match.Groups["EndZ"].Value);
      return new Input(on, new Position(startX, startY, startZ), new Position(endX, endY, endZ));
    }

    public readonly record struct Position(int X, int Y, int Z);

    public record struct Input(bool On, Position Start, Position End);
  }
}
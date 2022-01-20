using System;
using System.Text.RegularExpressions;

namespace _22_ReactorReboot
{
  internal class InputReader
  {
    internal static Input InterpretLine(string line)
    {
      var regex = new Regex(@"(?<on>on|off) x=(?<X>\d+)..(\d+),y=(?<Y>\d+)..(\d+),z=(?<Z>\d+)..(\d+)");
      var match = regex.Match(line);
      var on = match.Groups["on"].Value == "on";
      var X = int.Parse(match.Groups["X"].Value);
      var Y = int.Parse(match.Groups["Y"].Value);
      var Z = int.Parse(match.Groups["Z"].Value);
      return new Input(on, new Position(X, Y, Z));
    }

    public readonly record struct Position(int X, int Y, int Z);

    public record struct Input(bool On, Position Start);
  }
}
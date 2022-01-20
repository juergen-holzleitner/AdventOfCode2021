using System;
using System.Text.RegularExpressions;

namespace _22_ReactorReboot
{
  internal class InputReader
  {
    internal static Input InterpretLine(string line)
    {
      var regex = new Regex(@"(?<on>on|off) x=(?<X>\d+)..(\d+)");
      var match = regex.Match(line);
      var on = match.Groups["on"].Value == "on";
      var X = int.Parse(match.Groups["X"].Value);
      return new Input(on, new Position(X));
    }

    public readonly record struct Position(int X);

    public record struct Input(bool On, Position Start);
  }
}
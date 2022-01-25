using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _22_ReactorReboot
{
  internal class InputReader
  {
    internal static Input InterpretLine(string line)
    {
      var regex = new Regex(@"(?<on>on|off) x=(?<StartX>-?\d+)..(?<EndX>-?\d+),y=(?<StartY>-?\d+)..(?<EndY>-?\d+),z=(?<StartZ>-?\d+)..(?<EndZ>-?\d+)");
      var match = regex.Match(line);
      var on = match.Groups["on"].Value == "on";
      var startX = long.Parse(match.Groups["StartX"].Value);
      var startY = long.Parse(match.Groups["StartY"].Value);
      var startZ = long.Parse(match.Groups["StartZ"].Value);
      var endX = long.Parse(match.Groups["EndX"].Value);
      var endY = long.Parse(match.Groups["EndY"].Value);
      var endZ = long.Parse(match.Groups["EndZ"].Value);

      if (startX > endX)
        throw new ApplicationException("invalid input");
      if (startY > endY)
        throw new ApplicationException("invalid input");
      if (startZ > endZ)
        throw new ApplicationException("invalid input");

      return new Input(on, new Block(new Position(startX, startY, startZ), new Position(endX, endY, endZ)));
    }

    internal static IEnumerable<string> ReadAllInputLines(string fileName)
    {
      return System.IO.File.ReadLines(fileName);
    }

    internal static IEnumerable<Input> GetAllInputs(string fileName)
    {
      return from line in ReadAllInputLines(fileName)
             let input = InterpretLine(line)
             select input;
    }

    public readonly record struct Position(long X, long Y, long Z);

    public readonly record struct Block(Position A, Position B);

    public record struct Input(bool On, Block Block);
  }
}
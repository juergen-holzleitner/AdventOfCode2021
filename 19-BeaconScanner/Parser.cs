using System;
using System.Collections.Generic;

namespace _19_BeaconScanner
{
  internal static class Parser
  {
    internal static IEnumerable<Scanner> ParseScanner(IEnumerator<string> lines)
    {
      while (lines.MoveNext())
      { 
        if (string.IsNullOrEmpty(lines.Current))
          continue;

        var regex = new System.Text.RegularExpressions.Regex(@"--- (?<Name>scanner \d+) ---");
        var match = regex.Match(lines.Current);
        if (!match.Success)
          throw new ApplicationException("Failed to parse scanner name");

        var name = match.Groups["Name"].Value;

        var beacons = new List<Beacon>();
        while (lines.MoveNext())
        {
          if (string.IsNullOrEmpty(lines.Current))
            break;
          var b = ParseBeacon(lines.Current);
          beacons.Add(b);
        }
        yield return new Scanner(name, beacons);
      }
    }

    internal static Beacon ParseBeacon(string line)
    {
      var regex = new System.Text.RegularExpressions.Regex(@"(?<X>[+-]?\d+),(?<Y>[+-]?\d+),(?<Z>[+-]?\d+)");
      var match = regex.Match(line);
      if (!match.Success)
        throw new ApplicationException("Failed to parse beacon");

      int X = int.Parse(match.Groups["X"].Value);
      int Y = int.Parse(match.Groups["Y"].Value);
      int Z = int.Parse(match.Groups["Z"].Value);
      return new Beacon(X, Y, Z);
    }

    internal readonly record struct Beacon(int X, int Y, int Z);
    internal readonly record struct Scanner(string Name, IEnumerable<Beacon> Beacons);
  }
}

using System;
using System.Collections.Generic;

namespace _19_BeaconScanner
{
  internal static class Parser
  {
    internal static Scanner ParseScanner(string line)
    {
      var regex = new System.Text.RegularExpressions.Regex(@"--- (?<Name>scanner \d+) ---");
      var match = regex.Match(line);
      if (!match.Success)
        throw new ApplicationException("Failed to parse scanner");
      var name = match.Groups["Name"].Value;
      return new Scanner(name, new List<Beacon>());
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

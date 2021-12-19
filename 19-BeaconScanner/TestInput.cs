using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace _19_BeaconScanner
{
  [TestClass]
  public class TestInput
  {
    [TestMethod]
    public void ParseSingleBeacon()
    {
      var line = "-1,-1,1";
      var beacon = ParseBeacon(line);
      Assert.AreEqual(new Beacon(-1, -1, 1), beacon);
    }

    [TestMethod]
    public void ParseScannerHeader()
    {
      var line = "--- scanner 0 ---";
      var scanner = ParseScanner(line);
      Assert.AreEqual(new Scanner("scanner 0"), scanner);
    }

    private Scanner ParseScanner(string line)
    {
      var regex = new System.Text.RegularExpressions.Regex(@"--- (?<Name>scanner \d+) ---");
      var match = regex.Match(line);
      if (!match.Success)
        throw new ApplicationException("Failed to parse scanner");
      var name = match.Groups["Name"].Value;
      return new Scanner(name);
    }

    private Beacon ParseBeacon(string line)
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

    readonly record struct Beacon(int X, int Y, int Z);
    readonly record struct Scanner(string Name);
  }
}
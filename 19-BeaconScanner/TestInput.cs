using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _19_BeaconScanner
{
  [TestClass]
  public class TestInput
  {
    [TestMethod]
    public void ParseSingleBeacon()
    {
      var line = "-1,-1,1";
      var beacon = Parser.ParseBeacon(line);
      Assert.AreEqual(new Parser.Beacon(-1, -1, 1), beacon);
    }

    [TestMethod]
    public void ParseScannerName()
    {
      var scanner = ParseScannerFromText("--- scanner 0 ---");
      CheckScannerEqual(new Parser.Scanner("scanner 0", new List<Parser.Beacon>()), scanner.First());
    }

    [TestMethod]
    public void ParseScanner()
    {
      var line = @"--- scanner 0 ---
-1,-1,1
-2,-2,2
5,6,-4";

      var scanner = ParseScannerFromText(line);
      List<Parser.Beacon> expectedBeacons = new()
      { 
        new Parser.Beacon(-1,-1,1),
        new Parser.Beacon(-2,-2,2),
        new Parser.Beacon(5,6,-4),
      };
      CheckScannerEqual(new Parser.Scanner("scanner 0", expectedBeacons), scanner.First());
    }

    [TestMethod]
    public void ParseMultipleScanner()
    {
      var line = @"--- scanner 0 ---
-1,-1,1
-2,-2,2
5,6,-4

--- scanner 1 ---
-1,-1,1
-2,-2,2
5,6,-4
";

      var scanners = ParseScannerFromText(line);

      List<Parser.Beacon> expectedBeacons = new()
      {
        new Parser.Beacon(-1, -1, 1),
        new Parser.Beacon(-2, -2, 2),
        new Parser.Beacon(5, 6, -4),
      };
      var expected1 = new Parser.Scanner("scanner 0", expectedBeacons);
      var expected2 = new Parser.Scanner("scanner 1", expectedBeacons);
      CollectionAssert.AreEqual(new List<Parser.Scanner>() { expected1, expected2 }, scanners.ToList(), new ScannerComparer());
    }

    class ScannerComparer : IComparer
    {
      public int Compare(object? x, object? y)
      {
        var l = x as Parser.Scanner?;
        var r = y as Parser.Scanner?;

        if (l == null || r == null)
          throw new System.ApplicationException("can't compare");

        var n = l.Value.Name.CompareTo(r.Value.Name);
        if (n != 0)
          return n;

        if (l.Value.Beacons.SequenceEqual(r.Value.Beacons))
          return 0;

        return 1;
      }
    }

    static void CheckScannerEqual(Parser.Scanner expected, Parser.Scanner actual)
    {
      Assert.AreEqual(expected.Name, actual.Name);
      CollectionAssert.AreEqual((System.Collections.ICollection)expected.Beacons, (System.Collections.ICollection)actual.Beacons);
    }

    private static IEnumerable<Parser.Scanner> ParseScannerFromText(string text)
    {
      var lines = text.Split(System.Environment.NewLine) as IEnumerable<string>;
      return Parser.ParseScanner(lines.GetEnumerator());
    }
  }
}
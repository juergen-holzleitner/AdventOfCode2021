using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
      CheckScannerEqual(new Parser.Scanner("scanner 0", new List<Parser.Beacon>()), scanner);
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
      CheckScannerEqual(new Parser.Scanner("scanner 0", expectedBeacons), scanner);
    }

    static void CheckScannerEqual(Parser.Scanner expected, Parser.Scanner actual)
    {
      Assert.AreEqual(expected.Name, actual.Name);
      CollectionAssert.AreEqual((System.Collections.ICollection)expected.Beacons, (System.Collections.ICollection)actual.Beacons);
    }

    private static Parser.Scanner ParseScannerFromText(string text)
    {
      var lines = text.Split(System.Environment.NewLine) as IEnumerable<string>;
      return Parser.ParseScanner(lines.GetEnumerator());
    }
  }
}
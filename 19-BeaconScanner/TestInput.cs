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
      var line = "--- scanner 0 ---";
      var lines = line.Split(System.Environment.NewLine) as IEnumerable<string>;
      var scanner = Parser.ParseScanner(lines.GetEnumerator());
      Assert.AreEqual(new Parser.Scanner("scanner 0", new List<Parser.Beacon>()).Name, scanner.Name);
    }

    [TestMethod]
    public void ParseScanner()
    {
      var line = @"--- scanner 0 ---
-1,-1,1
-2,-2,2
-3,-3,3
-2,-3,1
5,6,-4
8,0,7";

      var lines = line.Split(System.Environment.NewLine) as IEnumerable<string>;
      var scanner = Parser.ParseScanner(lines.GetEnumerator());
    }
  }
}
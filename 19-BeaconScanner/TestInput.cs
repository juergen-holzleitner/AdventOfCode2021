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
      var scanner = Parser.ParseScanner(line);
      Assert.AreEqual(new Parser.Scanner("scanner 0", new List<Parser.Beacon>()).Name, scanner.Name);
    }
  }
}
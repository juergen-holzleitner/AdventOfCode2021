using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19_BeaconScanner
{
  [TestClass]
  public class TestSmallInput
  {
    [TestMethod]
    public void CheckNumMatches()
    {
      var allScanner = Parser.ParseScanner(File.ReadLines(@"input-small.txt").GetEnumerator()).ToList();
      Assert.AreEqual(5, allScanner.Count);

    }
  }
}

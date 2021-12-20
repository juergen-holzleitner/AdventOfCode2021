using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _19_BeaconScanner.Parser;

namespace _19_BeaconScanner
{
  [TestClass]
  public class TestManhattan
  {
    [TestMethod]
    public void TestDistance()
    {
      Beacon b1 = new Beacon(1105, -1205, 1229);
      Beacon b2 = new Beacon(-92, -2380, -20);

      var d = GetManhattanDistance(b1, b2);
      Assert.AreEqual(3621, d);
    }

    private int GetManhattanDistance(Beacon b1, Beacon b2)
    {
      return Math.Abs(b1.X - b2.X) + Math.Abs(b1.Y - b2.Y) + Math.Abs(b1.Z - b2.Z);
    }
  }
}

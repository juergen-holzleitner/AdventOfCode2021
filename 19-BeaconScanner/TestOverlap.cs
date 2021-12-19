using Microsoft.VisualStudio.TestTools.UnitTesting;
using static _19_BeaconScanner.Parser;
using static _19_BeaconScanner.TestMatrix;

namespace _19_BeaconScanner
{
  [TestClass]
  public class TestOverlap
  {
    [TestMethod]
    public void TestGetTranslation()
    {
      var b1 = new Beacon(618, 824, 621);
      var b2 = new Beacon(686, -422, 578);
      var res = GetBeaconDistance(b1, b2);
      Assert.AreEqual(new Beacon(68, -1246, -43), res);
    }

    record struct Alignment(Matrix Matrix, Beacon Translation);

    Beacon GetBeaconDistance(Beacon start, Beacon end)
    { 
      return new Beacon(end.X - start.X, end.Y - start.Y, end.Z - start.Z);
    }
  }
}

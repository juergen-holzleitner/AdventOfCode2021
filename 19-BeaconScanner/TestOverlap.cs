using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using static _19_BeaconScanner.Parser;
using static _19_BeaconScanner.TestMatrix;
using static _19_BeaconScanner.TestSmallInput;

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

    [TestMethod]
    public void TestScannerPosition()
    {
      var start = new Beacon(-618, -824, -621);
      var startAlign = new Alignment(Matrix.GetIdentity(), new Beacon(0, 0, 0));
      var scanner = new Beacon(686, 422, 578);
      var XNegRot180 = Matrix.GetRotationMatrices().ToList()[6];
      var d = GetScannerPosition(start, startAlign, scanner, XNegRot180);
      Assert.AreEqual(new Beacon(68, -1246, -43), d);
    }

    [TestMethod]
    public void TestEqual()
    {
      var line0 = @"--- scanner 0 ---
-618,-824,-621
-537,-823,-458
-447,-329,318
404,-588,-901
544,-627,-890
528,-643,409
-661,-816,-575
390,-675,-793
423,-701,434
-345,-311,381
459,-707,401
-485,-357,347
";
      var line1 = @"--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
-476,619,847
-460,603,-452
729,430,532
-322,571,750
-355,545,-477
413,935,-424
-391,539,-444
553,889,-390
";

      CheckScannerAreEqual(line0, line1);
    }

    private static void CheckScannerAreEqual(string line0, string line1)
    {
      var scanner0 = TestInput.ParseScannerFromText(line0).First();
      var aligned0 = new AligneedScanner(scanner0, new Alignment(Matrix.GetIdentity(), new Beacon(0, 0, 0)));

      var scanner1 = TestInput.ParseScannerFromText(line1).First();

      bool found = false;
      foreach (var mat in Matrix.GetRotationMatrices())
      {
        var scannerPos = GetScannerPosition(scanner0.Beacons.First(), aligned0.Alignment, scanner1.Beacons.First(), mat);
        var aligned1 = new AligneedScanner(scanner1, new Alignment(mat, scannerPos));

        var numMatches = GetNumMatches(aligned0, aligned1);
        if (numMatches >= scanner0.Beacons.Count())
          found = true;
      }
      Assert.IsTrue(found);
    }

    static internal int GetNumMatches(AligneedScanner aligned1, AligneedScanner aligned2)
    {
      int numMatches = 0;

      foreach (var b1 in aligned1.Scanner.Beacons)
      {
        var pos1 = Transform(b1, aligned1.Alignment);
        foreach (var b2 in aligned2.Scanner.Beacons)
        {
          var pos2 = Transform(b2, aligned2.Alignment);
          if (pos1 == pos2)
          {
            ++numMatches;
            break;
          }
        }
      }

      return numMatches;
    }

    static internal bool HasAtLeastMatches(TransformedScanner transformed, AligneedScanner aligned, int minMatches)
    {
      int numMatches = 0;

      foreach (var b1 in transformed.Scanner.Beacons)
      {
        foreach (var b2 in aligned.Scanner.Beacons)
        {
          var pos2 = Transform(b2, aligned.Alignment);
          if (b1 == pos2)
          {
            ++numMatches;
            if (numMatches >= minMatches)
              return true;
            break;
          }
        }
      }

      return false;
    }

    static internal Beacon GetScannerPosition(Beacon start, Alignment startAlign, Beacon scanner, Matrix endMatrix)
    {
      var s = Transform(start, startAlign);
      return GetScannerPosition(s, scanner, endMatrix);
    }

    static internal Beacon GetScannerPosition(Beacon transformedStart, Beacon scanner, Matrix endMatrix)
    {
      var e = endMatrix.Multiply(scanner);
      return GetBeaconDistance(e, transformedStart);
    }

    static internal Beacon Transform(Beacon b, Alignment a)
    {
      var pos = a.Matrix.Multiply(b);
      return Add(pos, a.Position);
    }

    internal record struct Alignment(Matrix Matrix, Beacon Position);

    internal record struct AligneedScanner(Scanner Scanner, Alignment Alignment);

    static Beacon GetBeaconDistance(Beacon start, Beacon end)
    { 
      return new Beacon(end.X - start.X, end.Y - start.Y, end.Z - start.Z);
    }

    static Beacon Add(Beacon start, Beacon end)
    {
      return new Beacon(start.X + end.X, start.Y + end.Y, start.Z + end.Z);
    }
  }
}

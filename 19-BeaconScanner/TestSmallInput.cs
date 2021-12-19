using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static _19_BeaconScanner.Parser;
using static _19_BeaconScanner.TestMatrix;
using static _19_BeaconScanner.TestOverlap;

namespace _19_BeaconScanner
{
  [TestClass]
  public class TestSmallInput
  {
    [TestMethod]
    public void CheckNumMatches()
    {
      const string fileName = @"input-small.txt";
      HashSet<Beacon> allBeacons = GetAllUniqueBeacons(fileName);

      Assert.AreEqual(79, allBeacons.Count);
    }

    private HashSet<Beacon> GetAllUniqueBeacons(string fileName)
    {
      var allScanner = ParseScanner(File.ReadLines(fileName).GetEnumerator()).ToList();
      
      List<AligneedScanner> alignedScanner = new();
      var identityAlign = new Alignment(Matrix.GetIdentity(), new Beacon(0, 0, 0));
      var startScanner = allScanner.First();
      allScanner.Remove(startScanner);

      alignedScanner.Add(new AligneedScanner(startScanner, identityAlign));

      while (allScanner.Any())
      {
        foreach (var scaner in allScanner)
        {
          if (IsScannerMatching(scaner, alignedScanner))
          {
            allScanner.Remove(scaner);
            break;
          }
        }

      }

      HashSet<Beacon> allBeacons = new();
      foreach (var scaner in alignedScanner)
      {
        foreach (var b in GetAllTransformedScannerBeacons(scaner))
          allBeacons.Add(b);
      }

      return allBeacons;
    }

    IEnumerable<Beacon> GetAllTransformedScannerBeacons(AligneedScanner scanner)
    {
      foreach (var b in scanner.Scanner.Beacons)
      {
        var transformedBeacon = Transform(b, scanner.Alignment);
        yield return transformedBeacon;
      }
    }

    bool IsScannerMatching(Scanner scanner, List<AligneedScanner> aligneedScanners)
    {
      foreach (var alScanner in aligneedScanners)
      {
        foreach (var mat in Matrix.GetRotationMatrices())
        {
          foreach (var b1 in alScanner.Scanner.Beacons)
          {
            foreach (var b2 in scanner.Beacons)
            {
              var scannerPos = TestOverlap.GetScannerPosition(b1, alScanner.Alignment, b2, mat);
              var aligned1 = new AligneedScanner(scanner, new Alignment(mat, scannerPos));

              var numMatches = TestOverlap.GetNumMatches(alScanner, aligned1);
              if (numMatches >= 12)
              {
                aligneedScanners.Add(aligned1);
                return true;
              }
            }
          }
        }
      }

      return false;
    }
  }
}

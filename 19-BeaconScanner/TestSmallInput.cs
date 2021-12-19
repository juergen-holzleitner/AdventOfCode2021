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

    static internal HashSet<Beacon> GetAllUniqueBeacons(string fileName)
    {
      var allScanner = ParseScanner(File.ReadLines(fileName).GetEnumerator()).ToList();
      
      List<TransformedScanner> transformedScanners = new();
      var identityAlign = new Alignment(Matrix.GetIdentity(), new Beacon(0, 0, 0));
      var startScanner = allScanner.First();
      allScanner.Remove(startScanner);

      var initialTransformed = TransformScaner(new AligneedScanner(startScanner, identityAlign));
      transformedScanners.Add(initialTransformed);

      HashSet<(Scanner, TransformedScanner)> failedChecks = new();
      
      while (allScanner.Any())
      {
        System.Console.WriteLine($"{allScanner.Count} scanner left");
        System.Diagnostics.Trace.TraceWarning($"{allScanner.Count} scanner left");
        
        bool found = false;
        foreach (var scaner in allScanner)
        {
          if (IsScannerMatching(scaner, transformedScanners, failedChecks))
          {
            allScanner.Remove(scaner);
            found = true;
            break;
          }
        }

        System.Diagnostics.Debug.Assert(found);
      }

      HashSet<Beacon> allBeacons = new();
      foreach (var scaner in transformedScanners)
      {
        foreach (var b in scaner.Scanner.Beacons)
          allBeacons.Add(b);
      }

      return allBeacons;
    }

    static TransformedScanner TransformScaner(AligneedScanner aligneedScanner)
    { 
      List<Beacon> transformedBeacons = new(GetAllTransformedScannerBeacons(aligneedScanner));
      var scanner = new Scanner(aligneedScanner.Scanner.Name, transformedBeacons);
      return new TransformedScanner(scanner, aligneedScanner.Alignment);

    }

    internal record struct TransformedScanner(Scanner Scanner, Alignment Alignment);

    static IEnumerable<Beacon> GetAllTransformedScannerBeacons(AligneedScanner scanner)
    {
      foreach (var b in scanner.Scanner.Beacons)
      {
        var transformedBeacon = Transform(b, scanner.Alignment);
        yield return transformedBeacon;
      }
    }

    static bool IsScannerMatching(Scanner scanner, List<TransformedScanner> transformedScanners, HashSet<(Scanner, TransformedScanner)> failedChecks)
    {
      foreach (var alScanner in transformedScanners)
      {
        if (failedChecks.Contains((scanner, alScanner)))
          continue;

        foreach (var mat in Matrix.GetRotationMatrices())
        {
          HashSet<Beacon> usedScannerPos = new();

          foreach (var b1 in alScanner.Scanner.Beacons)
          {
            foreach (var b2 in scanner.Beacons)
            {
              var scannerPos = TestOverlap.GetScannerPosition(b1, b2, mat);
              
              if (usedScannerPos.Contains(scannerPos))
                continue;
              usedScannerPos.Add(scannerPos);

              var aligned1 = new AligneedScanner(scanner, new Alignment(mat, scannerPos));

              if (TestOverlap.HasAtLeastMatches(alScanner, aligned1, 12))
              {
                transformedScanners.Add(TransformScaner(aligned1));
                return true;
              }
            }
          }
        }
        failedChecks.Add((scanner, alScanner));
      }

      return false;
    }
  }
}

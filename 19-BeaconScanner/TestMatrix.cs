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
  public class TestMatrix
  {
    [TestMethod]
    public void MultiplyIdentity()
    {
      var beacon = new Beacon(1, 2, 3);
      var mat = Matrix.GetIdentity();
      var res = mat.Multiply(beacon);
      Assert.AreEqual(beacon, res);
    }


    class Matrix
    {
      public static Matrix GetIdentity()
      {
        return new Matrix()
        {
          mat = new int[3, 3]
          {
            { 1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 1 },
          }
        };
      }

      internal Beacon Multiply(Beacon beacon)
      {
        System.Diagnostics.Debug.Assert(mat != null);

        var X = beacon.X * mat[0, 0] + beacon.X * mat[1, 0] + beacon.X * mat[2, 0];
        var Y = beacon.Y * mat[0, 1] + beacon.Y * mat[1, 1] + beacon.Y * mat[2, 1];
        var Z = beacon.Z * mat[0, 2] + beacon.Z * mat[1, 2] + beacon.Z * mat[2, 2];

        return new Beacon(X, Y, Z);
      }

      int[,]? mat = null;
    }
  }
}

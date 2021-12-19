using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
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

    [TestMethod]
    public void RotationsXPos()
    {
      var beacon = new Beacon(3, 1, 2);
      
      var res = Matrix.GetRotationMatrices().ToList()[0].Multiply(beacon);
      Assert.AreEqual(new Beacon(3, 1, 2), res);

      res = Matrix.GetRotationMatrices().ToList()[1].Multiply(beacon);
      Assert.AreEqual(new Beacon(3, -2, 1), res);

      res = Matrix.GetRotationMatrices().ToList()[2].Multiply(beacon);
      Assert.AreEqual(new Beacon(3, -1, -2), res);

      res = Matrix.GetRotationMatrices().ToList()[3].Multiply(beacon);
      Assert.AreEqual(new Beacon(3, 2, -1), res);
    }

    [TestMethod]
    public void RotationsXNeg()
    {
      var beacon = new Beacon(3, 1, 2);

      var res = Matrix.GetRotationMatrices().ToList()[4].Multiply(beacon);
      Assert.AreEqual(new Beacon(-3, -1, 2), res);

      res = Matrix.GetRotationMatrices().ToList()[5].Multiply(beacon);
      Assert.AreEqual(new Beacon(-3, -2, -1), res);

      res = Matrix.GetRotationMatrices().ToList()[6].Multiply(beacon);
      Assert.AreEqual(new Beacon(-3,  1, -2), res);

      res = Matrix.GetRotationMatrices().ToList()[7].Multiply(beacon);
      Assert.AreEqual(new Beacon(-3,  2,  1), res);
    }

    [TestMethod]
    public void RotationsYPos()
    {
      var beacon = new Beacon(3, 1, 2);

      var res = Matrix.GetRotationMatrices().ToList()[8].Multiply(beacon);
      Assert.AreEqual(new Beacon(1, -3, 2), res);

      res = Matrix.GetRotationMatrices().ToList()[9].Multiply(beacon);
      Assert.AreEqual(new Beacon(1, -2, -3), res);

      res = Matrix.GetRotationMatrices().ToList()[10].Multiply(beacon);
      Assert.AreEqual(new Beacon(1,  3, -2), res);

      res = Matrix.GetRotationMatrices().ToList()[11].Multiply(beacon);
      Assert.AreEqual(new Beacon(1,  2,  3), res);
    }

    [TestMethod]
    public void RotationsYNeg()
    {
      var beacon = new Beacon(3, 1, 2);

      var res = Matrix.GetRotationMatrices().ToList()[12].Multiply(beacon);
      Assert.AreEqual(new Beacon(-1,  3,  2), res);

      res = Matrix.GetRotationMatrices().ToList()[13].Multiply(beacon);
      Assert.AreEqual(new Beacon(-1, -2,  3), res);

      res = Matrix.GetRotationMatrices().ToList()[14].Multiply(beacon);
      Assert.AreEqual(new Beacon(-1, -3, -2), res);

      res = Matrix.GetRotationMatrices().ToList()[15].Multiply(beacon);
      Assert.AreEqual(new Beacon(-1,  2, -3), res);
    }

    [TestMethod]
    public void RotationsZPos()
    {
      var beacon = new Beacon(3, 1, 2);

      var res = Matrix.GetRotationMatrices().ToList()[16].Multiply(beacon);
      Assert.AreEqual(new Beacon(2, 1, -3), res);

      res = Matrix.GetRotationMatrices().ToList()[17].Multiply(beacon);
      Assert.AreEqual(new Beacon(2, 3, 1), res);

      res = Matrix.GetRotationMatrices().ToList()[18].Multiply(beacon);
      Assert.AreEqual(new Beacon(2, -1,  3), res);

      res = Matrix.GetRotationMatrices().ToList()[19].Multiply(beacon);
      Assert.AreEqual(new Beacon(2, -3, -1), res);
    }

    [TestMethod]
    public void RotationsZNeg()
    {
      var beacon = new Beacon(3, 1, 2);

      var res = Matrix.GetRotationMatrices().ToList()[20].Multiply(beacon);
      Assert.AreEqual(new Beacon(-2, 1, 3), res);

      res = Matrix.GetRotationMatrices().ToList()[21].Multiply(beacon);
      Assert.AreEqual(new Beacon(-2,  -3, 1), res);

      res = Matrix.GetRotationMatrices().ToList()[22].Multiply(beacon);
      Assert.AreEqual(new Beacon(-2,  1, -3), res);

      res = Matrix.GetRotationMatrices().ToList()[23].Multiply(beacon);
      Assert.AreEqual(new Beacon(-2,  3,  1), res);
    }

    public class Matrix
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

      public static IEnumerable<Matrix> GetRotationMatrices()
      {
        // +X
        yield return GetIdentity();

        // +X 90 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            { 1,  0, 0 },
            { 0,  0, 1 },
            { 0, -1, 0 },
          }
        };
        
        // +X 180 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            { 1,  0,  0 },
            { 0, -1,  0 },
            { 0,  0, -1 },
          }
        };

        // +X 270 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            { 1,  0,  0 },
            { 0,  0, -1 },
            { 0,  1,  0 },
          }
        };


        // -X
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            { -1,  0,  0 },
            {  0, -1,  0 },
            {  0,  0,  1 },
          }
        };

        // -X 90 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            { -1,  0,  0 },
            {  0,  0, -1 },
            {  0, -1,  0 },
          }
        };

        // -X 180 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            { -1,  0,  0 },
            {  0,  1,  0 },
            {  0,  0, -1 },
          }
        };

        // -X 270 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            { -1,  0, 0 },
            {  0,  0, 1 },
            {  0,  1, 0 },
          }
        };

        // Y
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0, -1,  0 },
            {  1,  0,  0 },
            {  0,  0,  1 },
          }
        };

        // Y 90 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  0, -1 },
            {  1,  0,  0 },
            {  0, -1,  0 },
          }
        };

        // Y 180 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  1,  0 },
            {  1,  0,  0 },
            {  0,  0, -1 },
          }
        };

        // Y 270 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  0,  1 },
            {  1,  0,  0 },
            {  0,  1,  0 },
          }
        };

        // -Y
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  1,  0 },
            { -1,  0,  0 },
            {  0,  0,  1 },
          }
        };

        // -Y 90 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  0,  1 },
            { -1,  0,  0 },
            {  0, -1,  0 },
          }
        };

        // -Y 180 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0, -1,  0 },
            { -1,  0,  0 },
            {  0,  0, -1 },
          }
        };

        // -Y 270 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  0, -1 },
            { -1,  0,  0 },
            {  0,  1,  0 },
          }
        };

        // Z
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  0, -1 },
            {  0,  1,  0 },
            {  1,  0,  0 },
          }
        };

        // Z 90 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  1,  0 },
            {  0,  0,  1 },
            {  1,  0,  0 },
          }
        };

        // Z 180 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  0,  1 },
            {  0, -1,  0 },
            {  1,  0,  0 },
          }
        };

        // Z 270 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0, -1,  0 },
            {  0,  0, -1 },
            {  1,  0,  0 },
          }
        };

        // -Z
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  0,  1 },
            {  0,  1,  0 },
            { -1,  0,  0 },
          }
        };

        // -Z 90 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0, -1,  0 },
            {  0,  0,  1 },
            { -1,  0,  0 },
          }
        };

        // -Z 180 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  0, -1 },
            {  0,  1,  0 },
            { -1,  0,  0 },
          }
        };

        // -Z 270 deg
        yield return new Matrix()
        {
          mat = new int[3, 3]
          {
            {  0,  1,  0 },
            {  0,  0,  1 },
            { -1,  0,  0 },
          }
        };
      }

      internal Beacon Multiply(Beacon beacon)
      {
        System.Diagnostics.Debug.Assert(mat != null);

        var X = beacon.X * mat[0, 0] + beacon.Y * mat[1, 0] + beacon.Z * mat[2, 0];
        var Y = beacon.X * mat[0, 1] + beacon.Y * mat[1, 1] + beacon.Z * mat[2, 1];
        var Z = beacon.X * mat[0, 2] + beacon.Y * mat[1, 2] + beacon.Z * mat[2, 2];

        return new Beacon(X, Y, Z);
      }

      int[,]? mat = null;
    }
  }
}

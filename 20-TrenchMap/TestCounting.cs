using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace _20_TrenchMap
{
  [TestClass]
  public class TestCounting
  {
    [TestMethod]
    public void TestNumLitPixels()
    {
      List<List<char>> newTestImage = new()
      {
        new List<char> { '.', '#', '#', '.', '#', '#', '.' },
        new List<char> { '#', '.', '.', '#', '.', '#', '.' },
        new List<char> { '#', '#', '.', '#', '.', '.', '#' },
        new List<char> { '#', '#', '#', '#', '.', '.', '#' },
        new List<char> { '.', '#', '.', '.', '#', '#', '.' },
        new List<char> { '.', '.', '#', '#', '.', '.', '#' },
        new List<char> { '.', '.', '.', '#', '.', '#', '.' },
      };

      int numLitPixels = GetNumLitPixels(newTestImage);
      Assert.AreEqual(24, numLitPixels);
    }

    [TestMethod]
    public void TestEnhanceTwice()
    {
      var input = TestInput.ReadInput(TestInput.smallInputFilename);
      var currImage = input.InputImage;
      var infinitePixel = '.';
      for (int n = 0; n < 2; ++n)
      {
        currImage = TestPixelEnhancement.EnhanceImage(currImage, input.EnhancmentAlgorithm, infinitePixel);
        infinitePixel = TestInfinitePixel.GetNextInfinitePixel(infinitePixel, input.EnhancmentAlgorithm);
      }

      int numLitPixels = GetNumLitPixels(currImage);
      Assert.AreEqual(35, numLitPixels);
    }

    [TestMethod]
    public void TestEnhanceFifty()
    {
      var input = TestInput.ReadInput(TestInput.smallInputFilename);
      var currImage = input.InputImage;
      var infinitePixel = '.';
      for (int n = 0; n < 50; ++n)
      {
        currImage = TestPixelEnhancement.EnhanceImage(currImage, input.EnhancmentAlgorithm, infinitePixel);
        infinitePixel = TestInfinitePixel.GetNextInfinitePixel(infinitePixel, input.EnhancmentAlgorithm);
      }

      int numLitPixels = GetNumLitPixels(currImage);
      Assert.AreEqual(3351, numLitPixels);
    }

    internal static int GetNumLitPixels(List<List<char>> image)
    {
      return (from r in image
              from c in r
              where c == '#'
              select c).Count();
    }
  }
}

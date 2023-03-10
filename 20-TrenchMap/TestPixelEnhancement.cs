using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace _20_TrenchMap
{
  [TestClass]
  public class TestPixelEnhancement
  {
    [TestMethod]
    public void TestCalculatePixelValue()
    {
      List<List<char>> testPixel = new()
      {
        new List<char> { '.', '.', '.' },
        new List<char> { '#', '.', '.' },
        new List<char> { '.', '#', '.' },
      };
      var num = ClaculateBinaryFromPixel(testPixel, 1, 1, '.');
      Assert.AreEqual(34, num);
    }

    [TestMethod]
    public void TestEnhancePixel()
    {
      var input = TestInput.ReadInput(TestInput.smallInputFilename);
      char ch = GetEnhancedPixel(input.EnhancmentAlgorithm, 34);
      Assert.AreEqual('#', ch);
    }

    [TestMethod]
    public void TestEnhancmentStep()
    {
      var input = TestInput.ReadInput(TestInput.smallInputFilename);
      var newImage = EnhanceImage(input.InputImage, input.EnhancmentAlgorithm, '.');

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

      Assert.AreEqual(newTestImage.Count, newImage.Count);
      for (int i = 0; i < newTestImage.Count; i++)
        CollectionAssert.AreEqual(newTestImage[i], newImage[i]);
    }

    internal static List<List<char>> EnhanceImage(List<List<char>> inputImage, string enhAlg, char infinitePixel)
    {
      var newImage = TestExpand.ExpandImageBy(inputImage, 1, infinitePixel);
      var orgImage = TestExpand.ExpandImageBy(inputImage, 1, infinitePixel);
      for (int row = 0; row < newImage.Count; ++row)
        for (int col = 0; col < newImage[row].Count; ++col)
        {
          var num = ClaculateBinaryFromPixel(orgImage, row, col, infinitePixel);
          newImage[row][col] = GetEnhancedPixel(enhAlg, num);
        }
      return newImage;
    }

    private static char GetEnhancedPixel(string enhAlg, int pos)
    {
      return enhAlg[pos];
    }

    private static int ClaculateBinaryFromPixel(List<List<char>> image, int row, int column, char infinitePixel)
    {
      if (row < 0 || row >= image.Count)
        throw new ArgumentException("arg is out of bounds", nameof(row));
      if (column < 0 || column >= image[row].Count)
        throw new ArgumentException("arg is out of bounds", nameof(column));

      int val = 0;
      for (int r = row - 1; r <= row + 1; ++r)
        for (int c = column - 1; c <= column + 1; ++c)
        {
          var pixel = infinitePixel;
          if (r >= 0 && r < image.Count)
            if (c >= 0 && c < image[r].Count)
              pixel = image[r][c];

          val <<= 1;
          if (pixel == '.')
          { }
          else if (pixel == '#')
            val |= 1;
          else
            throw new ApplicationException("invalid value");
        }

      return val;
    }
  }
}

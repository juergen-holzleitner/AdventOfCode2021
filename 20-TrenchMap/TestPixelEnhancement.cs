using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
      var num = ClaculateBinaryFromPixel(testPixel, 1, 1);
      Assert.AreEqual(34, num);
    }

    private int ClaculateBinaryFromPixel(List<List<char>> testPixel, int row, int column)
    {
      if (row < 1 || row > testPixel.Count - 2 )
        throw new ArgumentException("arg is out of bounds", nameof(row));
      if (column < 1 || column > testPixel[row].Count - 2)
        throw new ArgumentException("arg is out of bounds", nameof(column));

      int val = 0;
      for (int r = row -1; r <= row + 1; ++r)
        for (int c = column - 1; c <= column + 1; ++c)
        {
          val <<= 1;
          if (testPixel[r][c] == '.')
          { }
          else if (testPixel[r][c] == '#')
            val |= 1;
          else
            throw new ApplicationException("invalid value");
        }

      return val;
    }
  }
}

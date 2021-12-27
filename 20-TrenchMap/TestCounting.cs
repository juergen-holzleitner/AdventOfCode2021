using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    private int GetNumLitPixels(List<List<char>> image)
    {
      return (from r in image
              from c in r
              where c == '#'
              select c).Count();
    }
  }
}

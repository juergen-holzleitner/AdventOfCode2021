using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20_TrenchMap
{
  [TestClass]
  public class TestInfinitePixel
  {
    [TestMethod]
    public void TestInfinitePixelSwap()
    {
      var input = TestInput.ReadInput(@"input.txt");
      var infinitePixel = GetNextInfinitePixel('.', input.EnhancmentAlgorithm);
      Assert.AreEqual('#', infinitePixel);
    }

    internal static char GetNextInfinitePixel(char currInfinitePixel, string enhancmentAlgorithm)
    {
      int pos = currInfinitePixel == '.' ? 0 : 512;
      return enhancmentAlgorithm[pos];
    }
  }
}

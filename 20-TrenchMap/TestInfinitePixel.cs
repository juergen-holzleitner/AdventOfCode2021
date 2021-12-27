using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    [TestMethod]
    public void TestInfinitePixelSwapHash()
    {
      var input = TestInput.ReadInput(@"input.txt");
      var infinitePixel = GetNextInfinitePixel('#', input.EnhancmentAlgorithm);
      Assert.AreEqual('.', infinitePixel);
    }

    internal static char GetNextInfinitePixel(char currInfinitePixel, string enhancmentAlgorithm)
    {
      int pos = currInfinitePixel == '.' ? 0 : 511;
      return enhancmentAlgorithm[pos];
    }
  }
}

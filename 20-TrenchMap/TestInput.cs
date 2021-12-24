using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace _20_TrenchMap
{
  [TestClass]
  public class TestInput
  {
    [TestMethod]
    public void TestReadEnhancementAlgorithm()
    {
      var input = ReadInput(@"input-small.txt");
      Assert.AreEqual(512, input.EnhancmentAlgorithm.Length);
    }

    private static Input ReadInput(string fileName)
    {
      var lines = File.ReadLines(fileName).GetEnumerator();
      lines.MoveNext();
      var enhAlg = lines.Current;
      return new Input(enhAlg);
    }

    readonly record struct Input(string EnhancmentAlgorithm);
  }
}
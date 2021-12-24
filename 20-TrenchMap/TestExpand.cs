using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20_TrenchMap
{
  [TestClass]
  public class TestExpand
  {
    [TestMethod]
    public void TestExpandLine()
    {
      var line = "#..#.".ToList();
      var expandedLine = ExpandLineBy(line, 2);
      CollectionAssert.AreEqual("..#..#...".ToList(), expandedLine);
    }

    private static List<char> ExpandLineBy(List<char> line, int size)
    {
      var newLine = new List<char>();

      for (int i = 0; i < size; ++i)
        newLine.Add('.');

      newLine.AddRange(line);

      for (int i = 0; i < size; ++i)
        newLine.Add('.');

      return newLine;
    }
  }
}

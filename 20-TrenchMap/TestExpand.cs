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
      string line = "#..#.";
      string expandedLine = ExpandLineBy(line, 2);
      Assert.AreEqual(".." + line + "..", expandedLine);
    }

    private string ExpandLineBy(string line, int size)
    {
      var expansion = new string('.', size);
      return expansion + line + expansion;
    }
  }
}

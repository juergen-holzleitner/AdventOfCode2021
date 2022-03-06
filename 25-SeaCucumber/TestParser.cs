using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace _25_SeaCucumber
{
  public class TestParser
  {
    [Fact]
    public void TestParse()
    {
      var line = @"
..........
.>v....v..
.......>..
..........
";

      var input = Parser.Parse(line);

      input[1, 2].Should().Be('v');
    }
  }
}

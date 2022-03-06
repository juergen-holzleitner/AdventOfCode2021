using Xunit;
using FluentAssertions;

namespace _25_SeaCucumber
{
  public class TestMovement
  {
    [Theory]
    [InlineData(">.", ".>")]
    [InlineData(".>", ">.")]
    [InlineData("...>>>>>...", "...>>>>.>..")]
    [InlineData("...>>>>.>..", "...>>>.>.>.")]
    public void MoveHorizotal(string start, string expected)
    {
      var initial = start.ToCharArray();

      var final = Processor.MoveHorizontal(initial);

      final.Should().Equal(expected.ToCharArray());
    }

  }
}
using Xunit;
using FluentAssertions;

namespace _25_SeaCucumber
{
  public class TestMovement
  {
    [Fact]
    public void MoveForward()
    {
      var initial = new[] { '>', '.' };

      var final = Processor.MoveHorizontal(initial);

      final.Should().BeEquivalentTo(new[] { '.', '>' });
    }
  }
}
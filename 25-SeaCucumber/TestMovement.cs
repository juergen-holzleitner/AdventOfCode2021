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
      var initialArr = new char[1, initial.Length];
      for (var i = 0; i < initial.Length; i++)
        initialArr[0, i] = initial[i];

      var finalArr = Processor.MoveHorizontal(initialArr);

      var final = new char[finalArr.GetLength(1)];
      for (var i = 0; i < final.Length; i++)
        final[i] = finalArr[0, i];
      final.Should().Equal(expected.ToCharArray());
    }

  }
}
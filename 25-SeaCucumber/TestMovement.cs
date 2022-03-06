using Xunit;
using FluentAssertions;
using System;
using System.IO;

namespace _25_SeaCucumber
{
  public class Generic2DArrayAssertions<T>
  {
    readonly T[,] _actual;

    public Generic2DArrayAssertions(T[,] actual)
    {
      _actual = actual;
    }

    public bool Equal(T[,] expected)
    {
      for (int i = 0; i < expected.Rank; i++)
        _actual.GetUpperBound(i).Should().Be(expected.GetUpperBound(i),
                                             "dimensions should match");

      for (int x = expected.GetLowerBound(0); x <= expected.GetUpperBound(0); x++)
      {
        for (int y = expected.GetLowerBound(1); y <= expected.GetUpperBound(1); y++)
        {
          expected[x, y]
               .Should()
               .Be(_actual[x, y], "'{2}' should equal '{3}' at element [{0},{1}]",
                x, y, _actual[x, y], expected[x, y]);
        }
      }

      return true;
    }
  }

  public static class FluentExtensionMethods
  {
    public static Generic2DArrayAssertions<T> Should<T>(this T[,] actualValue)
    {
      return new Generic2DArrayAssertions<T>(actualValue);
    }
  }

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

      (var finalArr, _) = Processor.MoveHorizontal(initialArr);

      var final = new char[finalArr.GetLength(1)];
      for (var i = 0; i < final.Length; i++)
        final[i] = finalArr[0, i];
      final.Should().Equal(expected.ToCharArray());
    }

    [Fact]
    public void MoveVertical()
    {
      var arr = new char[3, 1] { { '.' }, { 'v' }, { '.' } };

      (var final, _) = Processor.MoveVertical(arr);

      var expected = new char[3, 1] { { '.' }, { '.' }, { 'v' } };
      final.Should().Equal(expected);
    }

    [Fact]
    public void MoveFirstHorizontalAndThenVertical()
    {
      var initialString = @"
..........
.>v....v..
.......>..
..........
";
      var initialArr = Parser.Parse(initialString);

      (var finalArr, _) = Processor.Move(initialArr);

      var finalString = @"
..........
.>........
..v....v>.
..........
";
      finalArr.Should().Equal(Parser.Parse(finalString));
    }

    [Fact]
    public void TestSecondExample()
    {
      var initialString = @"
...>...
.......
......>
v.....>
......>
.......
..vvv..
";
      var currentArr = Parser.Parse(initialString);

      for (int n = 0; n < 4; ++n)
        (currentArr, _) = Processor.Move(currentArr);

      var finalString = @"
>......
..v....
..>.v..
.>.v...
...>...
.......
v......
";
      currentArr.Should().Equal(Parser.Parse(finalString));
    }

    [Fact]
    public void TestThirdExample()
    {
      var initialString = @"
v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>
";
      var currentArr = Parser.Parse(initialString);

      bool wasMoving;
      int steps = 1;
      do {
        (currentArr, wasMoving) = Processor.Move(currentArr);
        if (wasMoving)
          ++steps;
      } while (wasMoving);

      steps.Should().Be(58);

      var finalString = @"
..>>v>vv..
..v.>>vv..
..>>v>>vv.
..>>>>>vv.
v......>vv
v>v....>>v
vvv.....>>
>vv......>
.>v.vv.v..
";
      currentArr.Should().Equal(Parser.Parse(finalString));
    }

    [Fact]
    public void TestInputSmall ()
    {
      var initialString = File.ReadAllText(@"input-small.txt");
      var currentArr = Parser.Parse(initialString);

      bool wasMoving;
      int steps = 1;
      do
      {
        (currentArr, wasMoving) = Processor.Move(currentArr);
        if (wasMoving)
          ++steps;
      } while (wasMoving);

      steps.Should().Be(58);

      var finalString = @"
..>>v>vv..
..v.>>vv..
..>>v>>vv.
..>>>>>vv.
v......>vv
v>v....>>v
vvv.....>>
>vv......>
.>v.vv.v..
";
      currentArr.Should().Equal(Parser.Parse(finalString));
    }

  }
}
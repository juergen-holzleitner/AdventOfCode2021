using FluentAssertions;
using Xunit;
using static _24_ALU.SymbolicALU;

namespace _24_ALU
{
  public class TestVariableRanges
  {
    [Fact]
    public void Range_of_number_is_number()
    {
      var term = new NumberOperand(5);

      var range = SymbolicALU.GetPossibleRange(term);

      range.Should().Be(new ValueRange(5, 5));
    }

    [Fact]
    public void Range_of_input_is_one_to_nine()
    {
      var term = new InputOperand(0);

      var range = SymbolicALU.GetPossibleRange(term);

      range.Should().Be(new ValueRange(1, 9));
    }

    [Fact]
    public void Range_of_add()
    {
      var term = new Term(Operation.add, new InputOperand(0), new NumberOperand(1));

      var range = SymbolicALU.GetPossibleRange(term);

      range.Should().Be(new ValueRange(2, 10));
    }

    [Fact]
    public void Range_of_mul()
    {
      var term = new Term(Operation.mul, new InputOperand(0), new InputOperand(1));

      var range = SymbolicALU.GetPossibleRange(term);

      range.Should().Be(new ValueRange(1, 81));
    }

    [Fact]
    public void Range_of_mul_with_negative()
    {
      var term = new Term(Operation.mul, new InputOperand(0), new NumberOperand(-1));

      var range = SymbolicALU.GetPossibleRange(term);

      range.Should().Be(new ValueRange(-9, -1));
    }

    public void Range_of_div_works()
    {
      var term = new Term(Operation.mul, new InputOperand(0), new NumberOperand(-2));

      var range = SymbolicALU.GetPossibleRange(term);

      range.Should().Be(new ValueRange(-9, -1));
    }
  }
}

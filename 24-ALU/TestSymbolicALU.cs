using Xunit;
using FluentAssertions;
using static _24_ALU.Parser;

namespace _24_ALU
{
  public class TestSymbolicALU
  {
    [Theory]
    [InlineData(Register.w)]
    [InlineData(Register.x)]
    [InlineData(Register.y)]
    [InlineData(Register.z)]
    public void SymbolicALU_is_initially_zero(Register register)
    {
      var sut = new SymbolicALU();

      var value = sut.GetValue(register);

      value.As<NumberOperand>().Number.Should().Be(0);
    }

    [Fact]
    public void Add_const_value_returns_value()
    {
      var sut = new SymbolicALU();
      const int testNumber = 5;
      var instruction = new Instruction(Operation.add, Register.x, new NumberOperand(testNumber));

      sut.ProcessInstruction(instruction);

      var value = sut.GetValue(Register.x);
      value.As<NumberOperand>().Number.Should().Be(testNumber);
    }

    [Fact]
    public void Inp_returns_Inp_Instruction()
    {
      var sut = new SymbolicALU();
      var instruction = new Instruction(Operation.inp, Register.x, null);

      sut.ProcessInstruction(instruction);

      var value = sut.GetValue(Register.x);
      value.As<InputOperand>().Index.Should().Be(0);
    }

    [Fact]
    public void Inp_MultipleTimes_returns_correct_input_index()
    {
      var sut = new SymbolicALU();
      var instruction = new Instruction(Operation.inp, Register.w, null);
      
      const int numInputs = 5;
      for (int n = 0; n < numInputs + 1; ++n)
      {
        sut.ProcessInstruction(instruction);
      }

      var value = sut.GetValue(Register.w);
      value.As<InputOperand>().Index.Should().Be(numInputs);
    }
  }
}

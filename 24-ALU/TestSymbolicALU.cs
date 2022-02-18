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
      var add = new Instruction(Operation.add, Register.x, new NumberOperand(testNumber));

      sut.ProcessInstruction(add);

      var value = sut.GetValue(Register.x);
      value.As<NumberOperand>().Number.Should().Be(testNumber);
    }

    [Fact]
    public void Inp_returns_Inp_Instruction()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.x, null);

      sut.ProcessInstruction(inp);

      var value = sut.GetValue(Register.x);
      value.As<InputOperand>().Index.Should().Be(0);
    }

    [Fact]
    public void Inp_MultipleTimes_returns_correct_input_index()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.w, null);
      
      const int numInputs = 5;
      for (int n = 0; n < numInputs + 1; ++n)
      {
        sut.ProcessInstruction(inp);
      }

      var value = sut.GetValue(Register.w);
      value.As<InputOperand>().Index.Should().Be(numInputs);
    }

    [Fact]
    public void Mul_with_zero_yields_zero()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.x, null);
      sut.ProcessInstruction(inp);
      var mul = new Instruction(Operation.mul, Register.x, new NumberOperand(0));

      sut.ProcessInstruction(mul);

      var value = sut.GetValue(Register.x);
      value.As<NumberOperand>().Number.Should().Be(0);
    }

    [Fact]
    public void Div_by_one_does_not_change()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.x, null);
      sut.ProcessInstruction(inp);
      var div = new Instruction(Operation.div, Register.x, new NumberOperand(1));

      sut.ProcessInstruction(div);

      var value = sut.GetValue(Register.x);
      value.As<InputOperand>().Index.Should().Be(0);
    }

    [Fact]
    public void Mul_by_one_does_not_change()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.x, null);
      sut.ProcessInstruction(inp);
      var mul = new Instruction(Operation.mul, Register.x, new NumberOperand(1));

      sut.ProcessInstruction(mul);

      var value = sut.GetValue(Register.x);
      value.As<InputOperand>().Index.Should().Be(0);
    }

    [Fact]
    public void Mod_by_one_is_zero()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.x, null);
      sut.ProcessInstruction(inp);
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(1));

      sut.ProcessInstruction(mod);

      var value = sut.GetValue(Register.x);
      value.As<NumberOperand>().Number.Should().Be(0);
    }
  }
}

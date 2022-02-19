using FluentAssertions;
using System;
using System.Linq;
using Xunit;

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

    [Fact]
    public void Multiply_zero_with_something_is_zero()
    {
      var sut = new SymbolicALU();
      var mul = new Instruction(Operation.mul, Register.x, new NumberOperand(5));

      sut.ProcessInstruction(mul);

      var value = sut.GetValue(Register.x);
      value.As<NumberOperand>().Number.Should().Be(0);
    }

    [Fact]
    public void Div_by_zero_throws()
    {
      var sut = new SymbolicALU();
      var div = new Instruction(Operation.div, Register.x, new NumberOperand(0));

      var act = () => sut.ProcessInstruction(div);

      act.Should().Throw<DivideByZeroException>();
    }

    [Fact]
    public void Mod_by_zero_throws()
    {
      var sut = new SymbolicALU();
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(0));

      var act = () => sut.ProcessInstruction(mod);

      act.Should().Throw<DivideByZeroException>();
    }

    [Fact]
    public void Div_by_number_returns_term()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.x, null);
      var div = new Instruction(Operation.div, Register.x, new NumberOperand(5));

      sut.ProcessInstruction(inp);
      sut.ProcessInstruction(div);

      var val = sut.GetValue(Register.x);
      val.As<Term>().Operation.Should().Be(Operation.div);
      val.As<Term>().Left.As<InputOperand>().Index.Should().Be(0);
      val.As<Term>().Right.As<NumberOperand>().Number.Should().Be(5);
    }

    [Fact]
    public void Zero_divided_by_something_is_zero()
    {
      var sut = new SymbolicALU();
      var div = new Instruction(Operation.div, Register.x, new NumberOperand(5));

      sut.ProcessInstruction(div);

      var val = sut.GetValue(Register.x);
      val.As<NumberOperand>().Number.Should().Be(0);
    }

    [Fact]
    public void Zero_mod_something_is_zero()
    {
      var sut = new SymbolicALU();
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(5));

      sut.ProcessInstruction(mod);

      var val = sut.GetValue(Register.x);
      val.As<NumberOperand>().Number.Should().Be(0);
    }

    [Fact]
    public void Mod_by_number_returns_term()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.x, null);
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(5));

      sut.ProcessInstruction(inp);
      sut.ProcessInstruction(mod);

      var val = sut.GetValue(Register.x);
      val.As<Term>().Operation.Should().Be(Operation.mod);
      val.As<Term>().Left.As<InputOperand>().Index.Should().Be(0);
      val.As<Term>().Right.As<NumberOperand>().Number.Should().Be(5);
    }

    [Fact]
    public void Add_number_to_input_returns_term()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.x, null);
      var add = new Instruction(Operation.add, Register.x, new NumberOperand(5));

      sut.ProcessInstruction(inp);
      sut.ProcessInstruction(add);

      var val = sut.GetValue(Register.x);
      val.As<Term>().Operation.Should().Be(Operation.add);
      val.As<Term>().Left.As<InputOperand>().Index.Should().Be(0);
      val.As<Term>().Right.As<NumberOperand>().Number.Should().Be(5);
    }

    [Fact]
    public void Multiply_two_number_returns_number()
    {
      var sut = new SymbolicALU();
      var add = new Instruction(Operation.add, Register.x, new NumberOperand(3));
      var mul = new Instruction(Operation.mul, Register.x, new NumberOperand(5));

      sut.ProcessInstruction(add);
      sut.ProcessInstruction(mul);

      var val = sut.GetValue(Register.x);
      val.As<NumberOperand>().Number.Should().Be(15);
    }

    [Fact]
    public void Div_two_number_returns_number()
    {
      var sut = new SymbolicALU();
      var add = new Instruction(Operation.add, Register.x, new NumberOperand(22));
      var div = new Instruction(Operation.div, Register.x, new NumberOperand(5));

      sut.ProcessInstruction(add);
      sut.ProcessInstruction(div);

      var val = sut.GetValue(Register.x);
      val.As<NumberOperand>().Number.Should().Be(4);
    }

    [Fact]
    public void Mod_two_number_returns_number()
    {
      var sut = new SymbolicALU();
      var add = new Instruction(Operation.add, Register.x, new NumberOperand(22));
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(5));

      sut.ProcessInstruction(add);
      sut.ProcessInstruction(mod);

      var val = sut.GetValue(Register.x);
      val.As<NumberOperand>().Number.Should().Be(2);
    }

    [Theory]
    [InlineData(22, 7, Operation.add, 29)]
    [InlineData(2, -3, Operation.mul, -6)]
    [InlineData(13, 3, Operation.div, 4)]
    [InlineData(13, 3, Operation.mod, 1)]
    public void Op_with_number_in_register_returns_number(int x, int y, Operation op, int expected)
    {
      var sut = new SymbolicALU();
      var add1 = new Instruction(Operation.add, Register.x, new NumberOperand(x));
      var add2 = new Instruction(Operation.add, Register.y, new NumberOperand(y));
      var operation = new Instruction(op, Register.x, new RegisterOperand(Register.y));

      sut.ProcessInstruction(add1);
      sut.ProcessInstruction(add2);
      sut.ProcessInstruction(operation);

      var val = sut.GetValue(Register.x);
      val.As<NumberOperand>().Number.Should().Be(expected);
    }

    [Fact]
    public void Mod_with_negative_operand_value_throws()
    {
      var sut = new SymbolicALU();
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(-1));

      var act = () => sut.ProcessInstruction(mod);

      act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Mod_with_negative_register_value_throws()
    {
      var sut = new SymbolicALU();
      var add = new Instruction(Operation.add, Register.x, new NumberOperand(-1));
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(5));
      sut.ProcessInstruction(add);

      var act = () => sut.ProcessInstruction(mod);

      act.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData(-1, 5)]
    [InlineData(1, -5)]
    public void Mod_with_negative_register_register_value_throws(int x, int y)
    {
      var sut = new SymbolicALU();
      var add1 = new Instruction(Operation.add, Register.x, new NumberOperand(x));
      var add2 = new Instruction(Operation.add, Register.y, new NumberOperand(y));
      var mod = new Instruction(Operation.mod, Register.y, new RegisterOperand(Register.x));
      sut.ProcessInstruction(add1);
      sut.ProcessInstruction(add2);

      var act = () => sut.ProcessInstruction(mod);

      act.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData(Operation.add)]
    [InlineData(Operation.mul)]
    [InlineData(Operation.div)]
    [InlineData(Operation.mod)]
    public void Op_with_two_inputs_yields_term(Operation op)
    {
      var sut = new SymbolicALU();
      var inp1 = new Instruction(Operation.inp, Register.x, null);
      var inp2 = new Instruction(Operation.inp, Register.y, null);
      var operation = new Instruction(op, Register.x, new RegisterOperand(Register.y));

      sut.ProcessInstruction(inp1);
      sut.ProcessInstruction(inp2);
      sut.ProcessInstruction(operation);

      var val = sut.GetValue(Register.x);
      val.As<Term>().Operation.Should().Be(op);
      val.As<Term>().Left.As<InputOperand>().Index.Should().Be(0);
      val.As<Term>().Right.As<InputOperand>().Index.Should().Be(1);
    }

    [Fact]
    public void Conditional_yiels_two_options()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.x, null);
      var cond = new Instruction(Operation.eql, Register.x, new NumberOperand(1));

      sut.ProcessInstruction(inp);
      sut.ProcessInstruction(cond);

      var options = sut.GetOptions();
      options.Count.Should().Be(2);
      var optionTrue = (from o in options
                        where o.Condition.Operands.Single().As<Term>().Operation == Operation.eql
                        select o).Single();
      var optionFalse = (from o in options
                         where o.Condition.Operands.Single().As<Term>().Operation == Operation.neq
                         select o).Single();
      optionTrue.State.Register[Register.x].Should().Be(new NumberOperand(1));
      optionFalse.State.Register[Register.x].Should().Be(new NumberOperand(0));
      optionTrue.Condition.Operands.Single().As<Term>().Left.Should().Be(new InputOperand(0));
      optionTrue.Condition.Operands.Single().As<Term>().Right.Should().Be(new NumberOperand(1));
      optionFalse.Condition.Operands.Single().As<Term>().Left.Should().Be(new InputOperand(0));
      optionFalse.Condition.Operands.Single().As<Term>().Right.Should().Be(new NumberOperand(1));
    }

    [Fact]
    public void Add_with_existing_zero_works()
    {
      var sut = new SymbolicALU();
      var inp = new Instruction(Operation.inp, Register.w, null);
      var add = new Instruction(Operation.add, Register.z, new RegisterOperand(Register.w));

      sut.ProcessInstruction(inp);
      sut.ProcessInstruction(add);

      var val = sut.GetValue(Register.z);
      val.Should().Be(new InputOperand(0));
    }
  }
}

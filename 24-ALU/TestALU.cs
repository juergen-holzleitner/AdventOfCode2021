using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using static _24_ALU.Parser;

namespace _24_ALU
{
  public class TestALU
  {
    [Theory]
    [InlineData(Register.w)]
    [InlineData(Register.x)]
    [InlineData(Register.y)]
    [InlineData(Register.z)]
    public void All_ALU_registers_are_zero_initially(Register register)
    {
      var sut = new ALU();

      var val = sut.GetValue(register);

      val.Should().Be(0);
    }

    [Fact]
    public void Add_increases_Register()
    {
      var sut = new ALU();
      const int TestNumber = 5;
      var instruction = new Instruction(Operation.add, Register.x, new NumberOperand(TestNumber));

      sut.ProcessInstruction(instruction);

      var val = sut.GetValue(Register.x);
      val.Should().Be(TestNumber);
    }

    [Fact]
    public void Mul_multiplies_Register()
    {
      var sut = new ALU();
      const int TestNumber = 5;
      var addX = new Instruction(Operation.add, Register.x, new NumberOperand(TestNumber));
      var addY = new Instruction(Operation.add, Register.y, new NumberOperand(TestNumber));
      var mul = new Instruction(Operation.mul, Register.y, new RegisterOperand(Register.x));
      sut.ProcessInstruction(addX);
      sut.ProcessInstruction(addY);

      sut.ProcessInstruction(mul);

      var val = sut.GetValue(Register.y);
      val.Should().Be(25);
    }

    [Fact]
    public void Div_truncates_value()
    {
      var sut = new ALU();
      var addW = new Instruction(Operation.add, Register.w, new NumberOperand(-7));
      var div = new Instruction(Operation.div, Register.w, new NumberOperand(3));
      sut.ProcessInstruction(addW);

      sut.ProcessInstruction(div);

      var val = sut.GetValue(Register.w);
      val.Should().Be(-2);
    }

    [Fact]
    public void DivByZero_is_handled()
    {
      var sut = new ALU();
      var addW = new Instruction(Operation.add, Register.w, new NumberOperand(-7));
      var div = new Instruction(Operation.div, Register.w, new NumberOperand(0));
      sut.ProcessInstruction(addW);

      var act = () => sut.ProcessInstruction(div);

      act.Should().Throw<DivideByZeroException>();
    }

    [Fact]
    public void Mod_works_for_positive_value()
    {
      var sut = new ALU();
      var addX = new Instruction(Operation.add, Register.x, new NumberOperand(10));
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(4));

      sut.ProcessInstruction(addX);
      sut.ProcessInstruction(mod);

      var val = sut.GetValue(Register.x);
      val.Should().Be(2);
    }

    [Fact]
    public void Mod_throws_if_register_is_negative()
    {
      var sut = new ALU();
      var addX = new Instruction(Operation.add, Register.x, new NumberOperand(-1));
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(1));

      sut.ProcessInstruction(addX);
      var act = () => sut.ProcessInstruction(mod);

      act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Mod_throws_if_operand_is_invalid()
    {
      var sut = new ALU();
      var addX = new Instruction(Operation.add, Register.x, new NumberOperand(1));
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(-2));

      sut.ProcessInstruction(addX);
      var act = () => sut.ProcessInstruction(mod);

      act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ModByZero_is_handled()
    {
      var sut = new ALU();
      var addX = new Instruction(Operation.add, Register.x, new NumberOperand(1));
      var mod = new Instruction(Operation.mod, Register.x, new NumberOperand(0));

      sut.ProcessInstruction(addX);
      var act = () => sut.ProcessInstruction(mod);

      act.Should().Throw<DivideByZeroException>();
    }

    [Fact]
    public void ReadInputValue_throws_on_empty_inputstream()
    {
      var sut = new ALU();
      var inpX = new Instruction(Operation.inp, Register.x, null);

      var act = () => sut.ProcessInstruction(inpX);

      act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ReadInput_stores_value_in_register()
    {
      var inputs = new List<int> { 5 };
      var sut = new ALU(inputs);
      var inpX = new Instruction(Operation.inp, Register.x, null);

      sut.ProcessInstruction(inpX);

      var val = sut.GetValue(Register.x);
      val.Should().Be(5);
    }

    [Fact]
    public void Equal_is_one_if_matches()
    {
      var sut = new ALU();
      var addX = new Instruction(Operation.add, Register.x, new NumberOperand(5));
      var equ = new Instruction(Operation.eql, Register.x, new NumberOperand(5));
      sut.ProcessInstruction(addX);

      sut.ProcessInstruction(equ);

      var val = sut.GetValue(Register.x);
      val.Should().Be(1);
    }

    [Fact]
    public void Equal_is_zero_if_nomatch()
    {
      var sut = new ALU();
      var addX = new Instruction(Operation.add, Register.x, new NumberOperand(5));
      var equ = new Instruction(Operation.eql, Register.x, new NumberOperand(6));
      sut.ProcessInstruction(addX);

      sut.ProcessInstruction(equ);

      var val = sut.GetValue(Register.x);
      val.Should().Be(0);
    }

  }
}

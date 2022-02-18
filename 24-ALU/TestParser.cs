using FluentAssertions;
using System;
using Xunit;

namespace _24_ALU
{
  public class TestParser
  {
    [Theory]
    [InlineData("inp x", Operation.inp)]
    [InlineData("add x y", Operation.add)]
    [InlineData("mul x y", Operation.mul)]
    [InlineData("div x y", Operation.div)]
    [InlineData("mod x y", Operation.mod)]
    [InlineData("eql x y", Operation.eql)]
    public void Can_read_instruction(string code, Operation operation)
    {
      var instruction = Parser.ParseLine(code);

      instruction.Operation.Should().Be(operation);
    }

    [Fact]
    public void Invalid_instruction_throws()
    {
      string code = "asdf x";

      var act = () => Parser.ParseLine(code);

      act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("inp x", Register.x)]
    [InlineData("inp y", Register.y)]
    [InlineData("inp z", Register.z)]
    [InlineData("inp w", Register.w)]
    public void Can_parse_register(string code, Register expectedRegister)
    {
      var instruction = Parser.ParseLine(code);

      instruction.Register.Should().Be(expectedRegister);
    }

    [Fact]
    public void Invalid_register_throws()
    {
      string code = "inp a";

      var act = () => Parser.ParseLine(code);

      act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("add x x", Register.x)]
    [InlineData("add x y", Register.y)]
    public void Can_parse_register_operand(string code, Register expectedRegister)
    {
      var instruction = Parser.ParseLine(code);

      instruction.Operand.As<RegisterOperand>().Register.Should().Be(expectedRegister);
    }

    [Theory]
    [InlineData("add x 10", 10)]
    [InlineData("add x -1", -1)]
    public void Can_parse_number_operand(string code, int expectedNumber)
    {
      var instruction = Parser.ParseLine(code);

      instruction.Operand.As<NumberOperand>().Number.Should().Be(expectedNumber);
    }

  }
}
using FluentAssertions;
using System;
using Xunit;

namespace _24_ALU
{
  public class TestParser
  {
    [Theory]
    [InlineData("inp x", Parser.Operation.inp)]
    [InlineData("add x y", Parser.Operation.add)]
    [InlineData("mul x y", Parser.Operation.mul)]
    [InlineData("div x y", Parser.Operation.div)]
    [InlineData("mod x y", Parser.Operation.mod)]
    [InlineData("eql x y", Parser.Operation.eql)]
    public void Can_read_instruction(string code, Parser.Operation operation)
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
    [InlineData("inp x", Parser.Register.x)]
    [InlineData("inp y", Parser.Register.y)]
    [InlineData("inp z", Parser.Register.z)]
    [InlineData("inp w", Parser.Register.w)]
    public void Can_parse_register(string code, Parser.Register expectedRegister)
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
    [InlineData("add x x", Parser.Register.x)]
    [InlineData("add x y", Parser.Register.y)]
    public void Can_parse_register_operand(string code, Parser.Register expectedRegister)
    {
      var instruction = Parser.ParseLine(code);

      instruction.Operand.As<Parser.RegisterOperand>().Register.Should().Be(expectedRegister);
    }

    [Theory]
    [InlineData("add x 10", 10)]
    [InlineData("add x -1", -1)]
    public void Can_parse_number_operand(string code, int expectedNumber)
    {
      var instruction = Parser.ParseLine(code);

      instruction.Operand.As<Parser.NumberOperand>().Number.Should().Be(expectedNumber);
    }

  }
}
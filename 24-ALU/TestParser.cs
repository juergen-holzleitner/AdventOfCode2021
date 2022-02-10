using Xunit;
using FluentAssertions;
using System;

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
      var sut = new Parser();

      var instruction = sut.ParseLine(code);

      instruction.Operation.Should().Be(operation);
    }

    [Fact]
    public void Invalid_instruction_throws()
    {
      var sut = new Parser();
      string code = "asdf x";

      var act = () => sut.ParseLine(code);

      act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("inp x", Parser.Register.x)]
    [InlineData("inp y", Parser.Register.y)]
    [InlineData("inp z", Parser.Register.z)]
    [InlineData("inp w", Parser.Register.w)]
    public void Can_Parse_Register(string code, Parser.Register expectedRegister)
    {
      var sut = new Parser();

      var instruction = sut.ParseLine(code);

      instruction.Register.Should().Be(expectedRegister);
    }

    [Fact]
    public void Invalid_register_throws()
    {
      var sut = new Parser();
      string code = "inp a";

      var act = () => sut.ParseLine(code);

      act.Should().Throw<ArgumentException>();
    }

  }
}
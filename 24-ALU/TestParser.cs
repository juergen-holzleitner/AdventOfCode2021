using Xunit;
using FluentAssertions;
using System;

namespace _24_ALU
{
  public class TestParser
  {
    [Theory]
    [InlineData("inp a", Parser.Operation.inp)]
    [InlineData("add a b", Parser.Operation.add)]
    [InlineData("mul a b", Parser.Operation.mul)]
    [InlineData("div a b", Parser.Operation.div)]
    [InlineData("mod a b", Parser.Operation.mod)]
    [InlineData("eql a b", Parser.Operation.eql)]
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
      string code = "asdf";

      var act = () => sut.ParseLine(code);

      act.Should().Throw<ArgumentException>();
    }
  }
}
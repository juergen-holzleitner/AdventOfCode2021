using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static _24_ALU.SymbolicALU;

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

    [Fact]
    public void Can_format_Number()
    {
      var operand = new NumberOperand(3);

      var str = Parser.Format(operand);

      str.Should().Be("3");
    }

    [Fact]
    public void Can_format_Input()
    {
      var operand = new InputOperand(2);

      var str = Parser.Format(operand);

      str.Should().Be("[2]");
    }

    [Fact]
    public void Can_format_Add()
    {
      var operand = new Term(Operation.add, new NumberOperand(3), new InputOperand(0));

      var str = Parser.Format(operand);

      str.Should().Be("3 + [0]");
    }

    [Fact]
    public void Can_format_Mul()
    {
      var add = new Term(Operation.add, new NumberOperand(3), new InputOperand(0));
      var operand = new Term(Operation.mul, new NumberOperand(2), add);

      var str = Parser.Format(operand);

      str.Should().Be("2 * (3 + [0])");
    }

    [Fact]
    public void Can_format_Div()
    {
      var mul = new Term(Operation.mul, new NumberOperand(3), new InputOperand(0));
      var operand = new Term(Operation.div, mul, new InputOperand(1));

      var str = Parser.Format(operand);

      str.Should().Be("(3 * [0]) / [1]");
    }

    [Fact]
    public void Can_format_Mod()
    {
      var mul = new Term(Operation.mul, new NumberOperand(3), new InputOperand(0));
      var div = new Term(Operation.div, new InputOperand(1), new NumberOperand(2));
      var operand = new Term(Operation.mod, mul, div);

      var str = Parser.Format(operand);

      str.Should().Be("(3 * [0]) % ([1] / 2)");
    }

    [Fact]
    public void Can_format_Equal()
    {
      var mul = new Term(Operation.mul, new NumberOperand(3), new InputOperand(0));
      var div = new Term(Operation.div, new InputOperand(1), new NumberOperand(2));
      var operand = new Term(Operation.eql, mul, div);

      var str = Parser.Format(operand);

      str.Should().Be("3 * [0] == [1] / 2");
    }

    [Fact]
    public void Can_format_NotEqual()
    {
      var mul = new Term(Operation.mul, new NumberOperand(3), new InputOperand(0));
      var div = new Term(Operation.div, new InputOperand(1), new NumberOperand(2));
      var operand = new Term(Operation.neq, mul, div);

      var str = Parser.Format(operand);

      str.Should().Be("3 * [0] != [1] / 2");
    }

    [Fact]
    public void Can_format_Condition()
    {
      var mul = new Term(Operation.mul, new NumberOperand(3), new InputOperand(0));
      var div = new Term(Operation.div, new InputOperand(1), new NumberOperand(2));
      var operand = new Term(Operation.eql, mul, div);

      var condition = new Condition(new List<IOperand>() { operand, operand });

      var str = Parser.Format(condition);

      str.Should().Be("(3 * [0] == [1] / 2) && (3 * [0] == [1] / 2)");
    }

    [Fact]
    public void Can_print_State()
    {
      var alu = new SymbolicALU();

      var str = Parser.Format(alu.GetOptions().Single().State);

      str.Should().Be("w: 0, x: 0, y: 0, z: 0");
    }
  }
}
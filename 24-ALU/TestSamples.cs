using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace _24_ALU
{
  public class TestSamples
  {
    [Fact]
    public void First_sample_works()
    {
      var program = @"
inp x
mul x -1
";
      var input = new List<int> { 5 };
      var sut = new ALU(input);

      foreach (var instruction in Parser.ReadProgramm(program))
        sut.ProcessInstruction(instruction);

      var val = sut.GetValue(Register.x);

      val.Should().Be(-5);
    }

    [Fact]
    public void Second_sample_works()
    {
      var program = @"
inp z
inp x
mul z 3
eql z x
";
      var input = new List<int> { 5, 15 };
      var sut = new ALU(input);

      foreach (var instruction in Parser.ReadProgramm(program))
        sut.ProcessInstruction(instruction);

      var val = sut.GetValue(Register.z);

      val.Should().Be(1);
    }

    [Fact]
    public void Third_sample_works()
    {
      var program = @"
inp w
add z w
mod z 2
div w 2
add y w
mod y 2
div w 2
add x w
mod x 2
div w 2
mod w 2
";
      var input = new List<int> { 0b1011 };
      var sut = new ALU(input);

      foreach (var instruction in Parser.ReadProgramm(program))
        sut.ProcessInstruction(instruction);

      var val1 = sut.GetValue(Register.z);
      var val2 = sut.GetValue(Register.y);
      var val4 = sut.GetValue(Register.x);
      var val8 = sut.GetValue(Register.w);
      val1.Should().Be(1);
      val2.Should().Be(1);
      val4.Should().Be(0);
      val8.Should().Be(1);
    }

    [Fact]
    public void Parser_can_split_model_number_intpu()
    {
      long modelNumber = 13579246899999;

      var input = Parser.SplitNumber(modelNumber);

      input.Count().Should().Be(14);
      input.First().Should().Be(1);
      input.Last().Should().Be(9);
    }

    [Fact]
    public void Check_model_number_works()
    {
      long modelNumber = 13579246899999;
      var input = Parser.SplitNumber(modelNumber);
      var alu = new ALU(input);
      var program = File.ReadLines(@"input.txt");

      foreach (var line in program)
      {
        var instruction = Parser.ParseLine(line);
        alu.ProcessInstruction(instruction);
      }

      var val = alu.GetValue(Register.z);
      val.Should().NotBe(0);
    }

    [Theory]
    [InlineData(13579246899999)]
    [InlineData(38576364913424)]
    [InlineData(38769131346953)]
    [InlineData(99862736452344)]
    [InlineData(25123762349234)]
    [InlineData(84628764641934)]
    [InlineData(34523412143546)]
    [InlineData(88876356245234)]
    [InlineData(34534534534665)]
    [InlineData(11111111111111)]
    [InlineData(99999999999999)]
    public void Optimized_programm_is_equal(long modelNumber)
    {
      var input = Parser.SplitNumber(modelNumber);
      var alu = new ALU(input);
      var programOriginal = File.ReadLines(@"input.txt");

      foreach (var line in programOriginal)
      {
        var instruction = Parser.ParseLine(line);
        alu.ProcessInstruction(instruction);
      }

      var valOriginal = alu.GetValue(Register.z);


      alu = new ALU(input);
      var programOptimized = File.ReadLines(@"input-small.txt");

      foreach (var line in programOptimized)
      {
        if (string.IsNullOrEmpty(line))
          continue;

        var instruction = Parser.ParseLine(line);
        alu.ProcessInstruction(instruction);
      }

      var valOptimized = alu.GetValue(Register.z);

      valOptimized.Should().Be(valOriginal);
    }

    [Fact]
    public void First_sample_works_symbolic()
    {
      var program = @"
inp x
mul x -1
";
      var sut = new SymbolicALU();

      foreach (var instruction in Parser.ReadProgramm(program))
        sut.ProcessInstruction(instruction);

      var val = sut.GetValue(Register.x);

      val.As<Term>().Operation.Should().Be(Operation.mul);
      val.As<Term>().Left.As<InputOperand>().Index.Should().Be(0);
      val.As<Term>().Right.As<NumberOperand>().Number.Should().Be(-1);
    }

    [Fact]
    public void Second_sample_works_symbolic()
    {
      var program = @"
inp z
inp x
mul z 3
eql z x
";
      var sut = new SymbolicALU();

      foreach (var instruction in Parser.ReadProgramm(program))
        sut.ProcessInstruction(instruction);

      var options = sut.GetOptions();
      options.Count.Should().Be(2);

      var optionTrue = (from o in options
                        where o.Condition.Operands.Single().As<Term>().Operation == Operation.eql
                        select o).Single();
      optionTrue.State.Register[Register.z].Should().Be(new NumberOperand(1));

      var conditionString = Parser.Format(optionTrue.Condition);
      conditionString.Should().Be("[0] * 3 == [1]");
      var stateString = Parser.Format(optionTrue.State);
      stateString.Should().Be("w: 0, x: [1], y: 0, z: 1");
    }

    [Fact]
    public void Third_sample_works_symbolic()
    {
      var program = @"
inp w
add z w
mod z 2
div w 2
add y w
mod y 2
div w 2
add x w
mod x 2
div w 2
mod w 2
";
      var sut = new SymbolicALU();

      foreach (var instruction in Parser.ReadProgramm(program))
        sut.ProcessInstruction(instruction);

      sut.GetOptions().Count.Should().Be(1);
      sut.GetOptions().Single().Condition.Operands.Should().BeEmpty();
      var stateString = Parser.Format(sut.GetOptions().Single().State);
      stateString.Should().Be("w: (([0] / 2) / 2) / 2, x: (([0] / 2) / 2) % 2, y: ([0] / 2) % 2, z: [0] % 2");
    }

    [Fact]
    public void Check_model_number_works_symbolic_for_first_input()
    {
      var alu = new SymbolicALU();
      var program = File.ReadLines(@"input.txt");

      foreach (var line in program.Take(18))
      {
        var instruction = Parser.ParseLine(line);
        alu.ProcessInstruction(instruction);
      }

      alu.GetOptions().Count.Should().Be(1);
      var option = alu.GetOptions().Single();
      var registerString = Parser.Format(option.State);
      registerString.Should().Be("w: [0], x: 1, y: [0] + 13, z: [0] + 13");
    }

  }
}

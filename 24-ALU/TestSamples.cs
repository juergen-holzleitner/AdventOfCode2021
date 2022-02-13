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
        sut.ProcessInstrution(instruction);

      var val = sut.GetValue(Parser.Register.x);

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
        sut.ProcessInstrution(instruction);

      var val = sut.GetValue(Parser.Register.z);

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
        sut.ProcessInstrution(instruction);

      var val1 = sut.GetValue(Parser.Register.z);
      var val2 = sut.GetValue(Parser.Register.y);
      var val4 = sut.GetValue(Parser.Register.x);
      var val8 = sut.GetValue(Parser.Register.w);
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
        alu.ProcessInstrution(instruction);
      }

      var val = alu.GetValue(Parser.Register.z);
      val.Should().NotBe(0);
    }
  }
}

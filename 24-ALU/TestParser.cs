using Xunit;

namespace _24_ALU
{
  public class TestParser
  {
    [Theory]
    [InlineData("inp a", Parser.Operation.inp)]
    [InlineData("add a b", Parser.Operation.add)]
    public void Can_read_instruction(string code, Parser.Operation operation)
    {
      var sut = new Parser();

      var instruction = sut.ParseLine(code);

      Assert.AreEqual(operation, instruction.Operation);
    }
  }
}
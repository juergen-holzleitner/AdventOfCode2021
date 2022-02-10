using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _24_ALU
{
  [TestClass]
  public class TestParser
  {
    [TestMethod]
    [DataRow("inp a", Parser.Operation.inp)]
    [DataRow("add a b", Parser.Operation.add)]
    public void Can_read_instruction(string code, Parser.Operation operation)
    {
      var sut = new Parser();

      var instruction = sut.ParseLine(code);

      Assert.AreEqual(operation, instruction.Operation);
    }
  }
}
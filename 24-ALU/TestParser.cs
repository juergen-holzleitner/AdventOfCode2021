using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _24_ALU
{
  [TestClass]
  public class TestParser
  {
    [TestMethod]
    public void Can_read_inp_instruction()
    {
      const string code = "inp x";
      var sut = new Parser();

      var instruction = sut.ParseLine(code);

      Assert.AreEqual(Parser.Operation.inp, instruction.Operation);
    }
  }
}
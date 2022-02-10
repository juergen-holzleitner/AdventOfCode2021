using System;

namespace _24_ALU
{
  public class Parser
  {
    public enum Operation { inp, add, mul, div, mod, eql };

    public Parser()
    {
    }

    public Instruction ParseLine(string codeLine)
    {
      var elements = codeLine.Split(' ');
      switch (elements[0])
      {
        case "inp": return new(Operation.inp);
        case "add": return new(Operation.add);
        case "mul": return new(Operation.mul);
        case "div": return new(Operation.div);
        case "mod": return new(Operation.mod);
        case "eql": return new(Operation.eql);
      }
      throw new ArgumentException("invalid instruction", nameof(codeLine));
    }

    public record Instruction(Operation Operation);
  }
}
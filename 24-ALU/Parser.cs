using System;

namespace _24_ALU
{
  internal class Parser
  {
    public enum Operation { inp };

    public Parser()
    {
    }

    public Instruction ParseLine(string codeLine)
    {
      return new(Operation.inp);
    }

    public record Instruction(Operation Operation);
  }
}
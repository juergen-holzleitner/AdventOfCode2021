using System;

namespace _24_ALU
{
  public class Parser
  {
    public enum Operation { inp, add };

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
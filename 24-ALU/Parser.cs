using System;

namespace _24_ALU
{
  public class Parser
  {
    public enum Operation { inp, add, mul, div, mod, eql };
    public enum Register { w, x, y, z}

    public Parser()
    {
    }

    public Instruction ParseLine(string codeLine)
    {
      var elements = codeLine.Split(' ');

      var operation = ParseOperation(elements[0]);
      var register = ParseRegister(elements[1]);
      return new(operation, register);
    }

    private Register ParseRegister(string register)
    {
      switch (register)
      {
        case "w": return Register.w;
        case "x": return Register.x;
        case "y": return Register.y;
        case "z": return Register.z;
      }

      throw new ArgumentException("invalid register", nameof(register));
    }

    private Operation ParseOperation(string operation)
    {
      switch (operation)
      {
        case "inp": return Operation.inp;
        case "add": return Operation.add;
        case "mul": return Operation.mul;
        case "div": return Operation.div;
        case "mod": return Operation.mod;
        case "eql": return Operation.eql;
      }
      throw new ArgumentException("invalid operation", nameof(operation));
    }

    public record Instruction(Operation Operation, Register Register);
  }
}
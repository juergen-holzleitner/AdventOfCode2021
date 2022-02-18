using System;
using System.Collections.Generic;
using static _24_ALU.Parser;

namespace _24_ALU
{
  internal class SymbolicALU
  {
    readonly Dictionary<Register, IOperand> register = new();

    public SymbolicALU()
    {
      foreach (var reg in Enum.GetValues(typeof(Register)))
        register.Add((Register)reg, new NumberOperand(0));
    }

    internal IOperand GetValue(Register register)
    {
      return this.register[register];
    }

    internal void ProcessInstruction(Instruction instruction)
    {
      if (instruction.Operation == Operation.add)
      {
        if (instruction.Operand is NumberOperand num2)
        {
          if (num2.Number == 0)
            return;

          if (register[instruction.Register] is NumberOperand num1)
          {
            register[instruction.Register] = new NumberOperand(num1.Number + num2.Number);
            return;
          }
        }
      }

      throw new NotImplementedException();
    }
  }
}
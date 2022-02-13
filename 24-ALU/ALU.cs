using System;
using System.Collections.Generic;

namespace _24_ALU
{
  internal class ALU
  {
    readonly Dictionary<Parser.Register, int> registerValue = new();
    readonly IEnumerator<int>? inputs = null;

    public ALU()
    {
      foreach (var reg in Enum.GetValues(typeof(Parser.Register)))
        registerValue.Add((Parser.Register)reg, 0);
    }

    public ALU(IEnumerable<int> inputs)
      : this()
    {
      this.inputs = inputs.GetEnumerator();
    }

    internal int GetValue(Parser.Register register)
    {
      return registerValue[register];
    }

    internal void ProcessInstrution(Parser.Instruction instruction)
    {
      switch (instruction.Operation)
      {
        case Parser.Operation.add:
          registerValue[instruction.Register] += GetOperandValue(instruction.Operand);
          break;
        case Parser.Operation.mul:
          registerValue[instruction.Register] *= GetOperandValue(instruction.Operand);
          break;
        case Parser.Operation.div:
          registerValue[instruction.Register] /= GetOperandValue(instruction.Operand);
          break;
        case Parser.Operation.mod:
          var oldVal = registerValue[instruction.Register];
          if (oldVal < 0)
            throw new InvalidOperationException("Module with negative register value");
          var operandVal = GetOperandValue(instruction.Operand);
          if (operandVal < 0)
            throw new InvalidOperationException("Module with negative operand value");
          registerValue[instruction.Register] = oldVal % operandVal;
          break;
        case Parser.Operation.inp:
          if (inputs != null && inputs.MoveNext())
          {
            registerValue[instruction.Register] = inputs.Current;
            break;
          }
          throw new InvalidOperationException();
        case Parser.Operation.eql:
          registerValue[instruction.Register] = registerValue[instruction.Register] == GetOperandValue(instruction.Operand) ? 1 : 0;
          break;
      }
    }

    private int GetOperandValue(Parser.IOperand? operand)
    {
      if (operand is Parser.RegisterOperand register)
        return registerValue[register.Register];
      else if (operand is Parser.NumberOperand number)
        return number.Number;
      else
        throw new ArgumentException("Invalid operand argument", nameof(operand));
    }
  }
}
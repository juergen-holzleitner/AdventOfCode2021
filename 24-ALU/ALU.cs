using System;
using System.Collections.Generic;

namespace _24_ALU
{
  internal class ALU
  {
    readonly Dictionary<Register, long> registerValue = new();
    readonly IEnumerator<int>? inputs = null;

    public ALU()
    {
      foreach (var reg in Enum.GetValues(typeof(Register)))
        registerValue.Add((Register)reg, 0);
    }

    public ALU(IEnumerable<int> inputs)
      : this()
    {
      this.inputs = inputs.GetEnumerator();
    }

    internal long GetValue(Register register)
    {
      return registerValue[register];
    }

    internal void ProcessInstruction(Instruction instruction)
    {
      switch (instruction.Operation)
      {
        case Operation.add:
          registerValue[instruction.Register] += GetOperandValue(instruction.Operand);
          break;
        case Operation.mul:
          registerValue[instruction.Register] *= GetOperandValue(instruction.Operand);
          break;
        case Operation.div:
          registerValue[instruction.Register] /= GetOperandValue(instruction.Operand);
          break;
        case Operation.mod:
          var oldVal = registerValue[instruction.Register];
          if (oldVal < 0)
            throw new InvalidOperationException("Module with negative register value");
          var operandVal = GetOperandValue(instruction.Operand);
          if (operandVal < 0)
            throw new InvalidOperationException("Module with negative operand value");
          registerValue[instruction.Register] = oldVal % operandVal;
          break;
        case Operation.inp:
          if (inputs != null && inputs.MoveNext())
          {
            registerValue[instruction.Register] = inputs.Current;
            break;
          }
          throw new InvalidOperationException();
        case Operation.eql:
          registerValue[instruction.Register] = registerValue[instruction.Register] == GetOperandValue(instruction.Operand) ? 1 : 0;
          break;
      }
    }

    private long GetOperandValue(IOperand? operand)
    {
      if (operand is RegisterOperand register)
        return registerValue[register.Register];
      else if (operand is NumberOperand number)
        return number.Number;
      else
        throw new ArgumentException("Invalid operand argument", nameof(operand));
    }
  }
}
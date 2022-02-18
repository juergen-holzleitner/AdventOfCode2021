using System;
using System.Collections.Generic;
using static _24_ALU.Parser;

namespace _24_ALU
{
  internal class SymbolicALU
  {
    readonly Dictionary<Register, IOperand> register = new();
    int inputIndex = 0;

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
      if(instruction.Operation == Operation.inp)
      {
        ProcessInp(instruction.Register);
        return;
      }
      else if (instruction.Operation == Operation.add && instruction.Operand is not null)
      {
        ProcessAdd(instruction.Register, instruction.Operand);
        return;
      }
      else if (instruction.Operation == Operation.mul && instruction.Operand is not null)
      {
        ProcessMul(instruction.Register, instruction.Operand);
        return;
      }
      else if (instruction.Operation == Operation.div && instruction.Operand is not null)
      {
        ProcessDiv(instruction.Register, instruction.Operand);
        return;
      }
      else if (instruction.Operation == Operation.mod && instruction.Operand is not null)
      {
        ProcessMod(instruction.Register, instruction.Operand);
        return;
      }

      throw new NotImplementedException();
    }

    private void ProcessMod(Register reg, IOperand operand)
    {
      if (operand is NumberOperand num2)
      {
        if (num2.Number == 1)
        {
          register[reg] = new NumberOperand(0);
          return;
        }
      }

      throw new NotImplementedException();
    }

    private void ProcessDiv(Register reg, IOperand operand)
    {
      if (operand is NumberOperand num2)
      {
        if (num2.Number == 1)
          return;
      }

      throw new NotImplementedException();
    }

    private void ProcessMul(Register reg, IOperand operand)
    {
      if (operand is NumberOperand num2)
      {
        if (num2.Number == 0)
        {
          register[reg] = new NumberOperand(0);
          return;
        }
        else if (num2.Number == 1)
        {
          return;
        }
      }

      throw new NotImplementedException();
    }

    private void ProcessAdd(Register reg, IOperand operand)
    {
      if (operand is NumberOperand num2)
      {
        if (num2.Number == 0)
          return;

        if (register[reg] is NumberOperand num1)
        {
          register[reg] = new NumberOperand(num1.Number + num2.Number);
          return;
        }
      }

      throw new NotImplementedException();
    }

    private void ProcessInp(Register reg)
    {
      register[reg] = new InputOperand(inputIndex);
      ++inputIndex;
    }
  }
}
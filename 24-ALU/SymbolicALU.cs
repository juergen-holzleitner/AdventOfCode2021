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

    private void ProcessInp(Register reg)
    {
      register[reg] = new InputOperand(inputIndex);
      ++inputIndex;
    }

    private void ProcessAdd(Register reg, IOperand operand)
    {
      if (operand is NumberOperand num)
      {
        ProcessAdd(reg, num);
        return;
      }

      if (operand is RegisterOperand regOp)
      {
        if (register[regOp.Register] is NumberOperand numOp)
        {
          ProcessAdd(reg, numOp);
          return;
        }
      }

      throw new NotImplementedException();
    }

    private void ProcessMul(Register reg, IOperand operand)
    {
      if (TargetRegisterIsZero(reg))
        return;

      if (operand is NumberOperand num)
      {
        ProcessMul(reg, num);
        return;
      }

      if (operand is RegisterOperand regOp)
      {
        if (register[regOp.Register] is NumberOperand numOp)
        {
          ProcessMul(reg, numOp);
          return;
        }
      }

      throw new NotImplementedException();
    }

    private void ProcessDiv(Register reg, IOperand operand)
    {
      CheckDivByZero(operand);

      if (TargetRegisterIsZero(reg))
        return;

      if (operand is NumberOperand num)
      {
        ProcessDiv(reg, num);
        return;
      }

      if (operand is RegisterOperand regOp)
      {
        if (register[regOp.Register] is NumberOperand numOp)
        {
          ProcessDiv(reg, numOp);
          return;
        }
      }

      throw new NotImplementedException();
    }

    private void ProcessMod(Register reg, IOperand operand)
    {
      CheckDivByZero(operand);

      if (operand is NumberOperand numNeg)
      {
        if (numNeg.Number < 0)
          throw new InvalidOperationException("Module with negative operand value");
      }

      if (TargetRegisterIsZero(reg))
        return;

      if (operand is NumberOperand num)
      {
        ProcessMod(reg, num);
        return;
      }

      if (operand is RegisterOperand regOp)
      {
        if (register[regOp.Register] is NumberOperand numOp)
        {
          ProcessMod(reg, numOp);
          return;
        }
      }

      throw new NotImplementedException();
    }

    private void ProcessMod(Register reg, NumberOperand num)
    {
      if (register[reg] is NumberOperand numNeg)
      {
        if (numNeg.Number < 0)
          throw new InvalidOperationException("Module with negative register value");
      }

      if (num.Number == 1)
      {
        register[reg] = new NumberOperand(0);
        return;
      }

      if (num.Number < 0)
        throw new InvalidOperationException("Module with negative register value");

      if (register[reg] is NumberOperand num1)
      {
        register[reg] = new NumberOperand(num1.Number % num.Number);
        return;
      }

      register[reg] = new Term(Operation.mod, register[reg], num);
    }

    private void ProcessDiv(Register reg, NumberOperand num)
    {
      if (num.Number == 1)
        return;

      if (register[reg] is NumberOperand num1)
      {
        register[reg] = new NumberOperand(num1.Number / num.Number);
        return;
      }

      register[reg] = new Term(Operation.div, register[reg], num);
    }

    private void ProcessMul(Register reg, NumberOperand num)
    {
      if (num.Number == 0)
      {
        register[reg] = new NumberOperand(0);
        return;
      }
      
      if (num.Number == 1)
        return;

      if (register[reg] is NumberOperand num1)
      {
        register[reg] = new NumberOperand(num1.Number * num.Number);
        return;
      }

      register[reg] = new Term(Operation.mul, register[reg], num);
    }

    private void ProcessAdd(Register reg, NumberOperand num)
    {
      if (num.Number == 0)
        return;

      if (register[reg] is NumberOperand num1)
      {
        register[reg] = new NumberOperand(num1.Number + num.Number);
        return;
      }

      register[reg] = new Term(Operation.add, register[reg], num);
    }

    private static void CheckDivByZero(IOperand operand)
    {
      if (operand is NumberOperand numCheckZero)
      {
        if (numCheckZero.Number == 0)
          throw new DivideByZeroException();
      }
    }

    private bool TargetRegisterIsZero(Register reg)
    {
      if (register[reg] is NumberOperand numReg)
      {
        if (numReg.Number == 0)
          return true;
      }

      return false;
    }

  }
}
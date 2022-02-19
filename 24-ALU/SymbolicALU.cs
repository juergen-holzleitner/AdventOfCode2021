using System;
using System.Collections.Generic;
using System.Linq;
using static _24_ALU.Parser;

namespace _24_ALU
{
  internal class SymbolicALU
  {
    internal record Option(Condition Condition, State State);
    internal record State(Dictionary<Register, IOperand> Register);

    internal record Condition(List<IOperand> Operands);

    List<Option> options = new();

    int inputIndex = 0;

    public SymbolicALU()
    {
      var state = new State(new Dictionary<Register, IOperand>());

      foreach (var reg in Enum.GetValues(typeof(Register)))
        state.Register.Add((Register)reg, new NumberOperand(0));

      options.Add(new Option(new(new List<IOperand>()), state));
    }

    internal IOperand GetValue(Register register)
    {
      return options.Single().State.Register[register];
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
      else if (instruction.Operation == Operation.eql && instruction.Operand is not null)
      {
        ProcessEql(instruction.Register, instruction.Operand);
        return;
      }

      throw new NotImplementedException();
    }

    private void ProcessInp(Register reg)
    {
      foreach (var o in options)
        ProcessInp(reg, o.State);
      
      ++inputIndex;
    }
    private void ProcessAdd(Register reg, IOperand operand)
    {
      foreach (var o in options)
        ProcessAdd(reg, operand, o.State);
    }
    private void ProcessMul(Register reg, IOperand operand)
    {
      foreach (var o in options)
        ProcessMul(reg, operand, o.State);
    }
    private void ProcessDiv(Register reg, IOperand operand)
    {
      foreach (var o in options)
        ProcessDiv(reg, operand, o.State);
    }
    private void ProcessMod(Register reg, IOperand operand)
    {
      foreach (var o in options)
        ProcessMod(reg, operand, o.State);
    }

    private void ProcessInp(Register reg, State state)
    {
      state.Register[reg] = new InputOperand(inputIndex);
    }

    private void ProcessAdd(Register reg, IOperand operand, State state)
    {
      if (operand is NumberOperand num)
      {
        ProcessAdd(reg, num, state);
        return;
      }

      if (operand is RegisterOperand regOp)
      {
        if (state.Register[regOp.Register] is NumberOperand numOp)
        {
          ProcessAdd(reg, numOp, state);
          return;
        }

        state.Register[reg] = new Term(Operation.add, state.Register[reg], state.Register[regOp.Register]);
        return;
      }

      throw new NotImplementedException();
    }

    private void ProcessMul(Register reg, IOperand operand, State state)
    {
      if (TargetRegisterIsZero(reg, state))
        return;

      if (operand is NumberOperand num)
      {
        ProcessMul(reg, num, state);
        return;
      }

      if (operand is RegisterOperand regOp)
      {
        if (state.Register[regOp.Register] is NumberOperand numOp)
        {
          ProcessMul(reg, numOp, state);
          return;
        }

        state.Register[reg] = new Term(Operation.mul, state.Register[reg], state.Register[regOp.Register]);
        return;
      }

      throw new NotImplementedException();
    }

    private void ProcessDiv(Register reg, IOperand operand, State state)
    {
      CheckDivByZero(operand);

      if (TargetRegisterIsZero(reg, state))
        return;

      if (operand is NumberOperand num)
      {
        ProcessDiv(reg, num, state);
        return;
      }

      if (operand is RegisterOperand regOp)
      {
        if (state.Register[regOp.Register] is NumberOperand numOp)
        {
          ProcessDiv(reg, numOp, state);
          return;
        }

        state.Register[reg] = new Term(Operation.div, state.Register[reg], state.Register[regOp.Register]);
        return;
      }

      throw new NotImplementedException();
    }

    private void ProcessMod(Register reg, IOperand operand, State state)
    {
      CheckDivByZero(operand);

      if (operand is NumberOperand numNeg)
      {
        if (numNeg.Number < 0)
          throw new InvalidOperationException("Module with negative operand value");
      }

      if (TargetRegisterIsZero(reg, state))
        return;

      if (operand is NumberOperand num)
      {
        ProcessMod(reg, num, state);
        return;
      }

      if (operand is RegisterOperand regOp)
      {
        if (state.Register[regOp.Register] is NumberOperand numOp)
        {
          ProcessMod(reg, numOp, state);
          return;
        }

        state.Register[reg] = new Term(Operation.mod, state.Register[reg], state.Register[regOp.Register]);
        return;
      }

      throw new NotImplementedException();
    }

    private void ProcessEql(Register reg, IOperand operand)
    {
      var newOptions = new List<Option>();
      foreach (var option in options)
      {
        IOperand op = operand;
        if (op is NumberOperand) { }
        else if (op is RegisterOperand regOp)
          op = option.State.Register[regOp.Register];
        else
          throw new InvalidOperationException();

        var conditionFalse = new Condition(option.Condition.Operands.ToList());
        conditionFalse.Operands.Add(new Term(Operation.neq, option.State.Register[reg], op));
        var stateFalse = new State(option.State.Register.ToDictionary(x => x.Key, x => x.Value));
        stateFalse.Register[reg] = new NumberOperand(0);

        var conditionTrue = option.Condition;
        conditionTrue.Operands.Add(new Term(Operation.eql, option.State.Register[reg], op));
        var stateTrue = option.State;
        stateTrue.Register[reg] = new NumberOperand(1);

        var optionFalse = new Option(conditionFalse, stateFalse);
        var optionTrue = new Option(conditionTrue, stateTrue);
        
        newOptions.Add(optionTrue);
        newOptions.Add(optionFalse);
      }

      options = newOptions;
    }

    private void ProcessMod(Register reg, NumberOperand num, State state)
    {
      if (state.Register[reg] is NumberOperand numNeg)
      {
        if (numNeg.Number < 0)
          throw new InvalidOperationException("Module with negative register value");
      }

      if (num.Number == 1)
      {
        state.Register[reg] = new NumberOperand(0);
        return;
      }

      if (num.Number < 0)
        throw new InvalidOperationException("Module with negative register value");

      if (state.Register[reg] is NumberOperand num1)
      {
        state.Register[reg] = new NumberOperand(num1.Number % num.Number);
        return;
      }

      state.Register[reg] = new Term(Operation.mod, state.Register[reg], num);
    }

    private void ProcessDiv(Register reg, NumberOperand num, State state)
    {
      if (num.Number == 1)
        return;

      if (state.Register[reg] is NumberOperand num1)
      {
        state.Register[reg] = new NumberOperand(num1.Number / num.Number);
        return;
      }

      state.Register[reg] = new Term(Operation.div, state.Register[reg], num);
    }

    private void ProcessMul(Register reg, NumberOperand num, State state)
    {
      if (num.Number == 0)
      {
        state.Register[reg] = new NumberOperand(0);
        return;
      }
      
      if (num.Number == 1)
        return;

      if (state.Register[reg] is NumberOperand num1)
      {
        state.Register[reg] = new NumberOperand(num1.Number * num.Number);
        return;
      }

      state.Register[reg] = new Term(Operation.mul, state.Register[reg], num);
    }

    private void ProcessAdd(Register reg, NumberOperand num, State state)
    {
      if (num.Number == 0)
        return;

      if (state.Register[reg] is NumberOperand num1)
      {
        state.Register[reg] = new NumberOperand(num1.Number + num.Number);
        return;
      }

      state.Register[reg] = new Term(Operation.add, state.Register[reg], num);
    }

    private static void CheckDivByZero(IOperand operand)
    {
      if (operand is NumberOperand numCheckZero)
      {
        if (numCheckZero.Number == 0)
          throw new DivideByZeroException();
      }
    }

    private bool TargetRegisterIsZero(Register reg, State state)
    {
      if (state.Register[reg] is NumberOperand numReg)
      {
        if (numReg.Number == 0)
          return true;
      }

      return false;
    }

    internal List<Option> GetOptions()
    {
      return options;
    }
  }
}
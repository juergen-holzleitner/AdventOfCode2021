using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static _24_ALU.SymbolicALU;

namespace _24_ALU
{
  public partial class Parser
  {
    internal static IEnumerable<Instruction> ReadProgramm(string program)
    {
      using var reader = new System.IO.StringReader(program);
      string? line;
      while ((line = reader.ReadLine()) != null)
      {
        if (!string.IsNullOrEmpty(line))
          yield return ParseLine(line);
      }
    }

    public Parser()
    {
    }

    public static Instruction ParseLine(string codeLine)
    {
      var elements = codeLine.Split(' ');

      var operation = ParseOperation(elements[0]);
      var register = ParseRegister(elements[1]);
      IOperand? operand = null;
      if (operation != Operation.inp)
        operand = ParseOperand(elements[2]);

      return new(operation, register, operand);
    }

    private static IOperand ParseOperand(string operand)
    {
      if (int.TryParse(operand, out var number))
        return new NumberOperand(number);
      return new RegisterOperand(ParseRegister(operand));
    }

    private static Register ParseRegister(string register)
    {
      return register switch
      {
        "w" => Register.w,
        "x" => Register.x,
        "y" => Register.y,
        "z" => Register.z,
        _ => throw new ArgumentException("invalid register", nameof(register)),
      };
    }

    private static Operation ParseOperation(string operation)
    {
      return operation switch
      {
        "inp" => Operation.inp,
        "add" => Operation.add,
        "mul" => Operation.mul,
        "div" => Operation.div,
        "mod" => Operation.mod,
        "eql" => Operation.eql,
        _ => throw new ArgumentException("invalid operation", nameof(operation)),
      };
    }

    internal static IEnumerable<int> SplitNumber(long number)
    {
      return GetDigitsReversed(number).Reverse();
    }

    private static IEnumerable<int> GetDigitsReversed(long number)
    {
      if (number == 0)
        yield return 0;
      else
      {
        while (number > 0)
        {
          yield return (int)(number % 10);
          number /= 10;
        }
      }
    }

    internal static string Format(Condition condition)
    {
      StringBuilder str = new();
      if (condition.Operands.Count == 1)
        return Format(condition.Operands[0]);

      foreach (var op in condition.Operands)
      {
        if (str.Length > 0)
          str.Append(" && ");

        str.Append($"({Format(op)})");
      }

      return str.ToString();
    }

    internal static string Format(State state)
    {
      StringBuilder sb = new StringBuilder();

      foreach (var reg in Enum.GetValues(typeof(Register)))
      {
        if (sb.Length > 0)
          sb.Append(", ");
        sb.Append(reg.ToString());
        sb.Append(": ");
        sb.Append(Format(state.Register[(Register)reg]));
      }

      return sb.ToString();
    }

    internal static string Format(IOperand operand)
    {
      if (operand is NumberOperand number)
        return number.Number.ToString();
      else if (operand is InputOperand input)
        return $"[{input.Index}]";
      else if (operand is Term term)
        return Format(term);

      throw new NotImplementedException();
    }

    internal static string Format(Term term)
    {
      if (term.Operation == Operation.add)
      {
        return $"{Format(term.Left)} + {Format(term.Right)}";
      }
      else if (term.Operation == Operation.mul)
      {
        return $"{FormatFactor(term.Left)} * {FormatFactor(term.Right)}";
      }
      else if (term.Operation == Operation.div)
      {
        return $"{FormatDiv(term.Left)} / {FormatDiv(term.Right)}";
      }
      else if (term.Operation == Operation.mod)
      {
        return $"{FormatDiv(term.Left)} % {FormatDiv(term.Right)}";
      }
      else if (term.Operation == Operation.eql)
      {
        return $"{Format(term.Left)} == {Format(term.Right)}";
      }
      else if (term.Operation == Operation.neq)
      {
        return $"{Format(term.Left)} != {Format(term.Right)}";
      }

      throw new NotImplementedException();
    }

    internal static string FormatFactor(IOperand operand)
    {
      if (operand is Term term)
      {
        if (term.Operation == Operation.add)
          return $"({Format(term)})";
      }

      return Format(operand);
    }

    internal static string FormatDiv(IOperand operand)
    {
      if (operand is Term term)
        return $"({Format(term)})";

      return Format(operand);
    }

    internal static string FormatSymbolicALU(SymbolicALU alu)
    {
      var sb = new StringBuilder();
      foreach (var option in alu.GetOptions())
      {
        if (option.Condition.Operands.Any())
        {
          var condition = Format(option.Condition);
          sb.AppendLine($"\t{condition}:");
        }

        foreach (var reg in Enum.GetValues(typeof(Register)))
        {
          sb.Append("\t\t");
          sb.Append(reg.ToString());
          var term = option.State.Register[(Register)reg];
          var valueRange = SymbolicALU.GetPossibleRange(term);
          sb.Append($" [{valueRange.Min},{valueRange.Max}]: ");
          sb.AppendLine(Format(term));
        }
      }
      return sb.ToString();
    }
  }
}
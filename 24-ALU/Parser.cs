using System;
using System.Collections.Generic;
using System.Linq;

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
  }
}
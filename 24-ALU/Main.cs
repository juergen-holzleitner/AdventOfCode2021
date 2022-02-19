using _24_ALU;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

//TODO:
// * optimize % with smaller value
// * optimize / with smaller value
// multiply out terms, i.e. mul, div, mod
// run old algorithm with random inputs to find a valid number

SymbolicAnalysis();
CalcFinalModelNumber();

void SymbolicAnalysis()
{
  int lineNumber = 0;
  var program = File.ReadLines(@"input.txt");
  var alu = new SymbolicALU();

  foreach (var line in program)
  {
    ++lineNumber;

    if (string.IsNullOrEmpty(line))
      continue;

    var instruction = Parser.ParseLine(line);
    alu.ProcessInstruction(instruction);

    //System.Console.WriteLine($"{lineNumber}: {line}");

    //System.Console.WriteLine(Parser.FormatSymbolicALU(alu));
    //System.Console.ReadKey();
  }

  System.Console.WriteLine(Parser.FormatSymbolicALU(alu));
}

void RandomAnalysis()
{
  var random = new System.Random(1234);
  var program = File.ReadLines(@"input.txt");
  var instructions = (from l in program select Parser.ParseLine(l)).ToList();

  for (; ; )
  {
    var modelNumber = RandomModelNumber(random);

    System.Console.Write(FormatModelNumber(modelNumber) + '\r');

    var alu = new ALU(modelNumber);
    foreach (var instruction in instructions)
      alu.ProcessInstruction(instruction);
    
    var val = alu.GetValue(Register.z);
    if (val == 0)
      System.Console.WriteLine("Found valid ModelNumber: " + FormatModelNumber(modelNumber));
  }
}

List<int> RandomModelNumber(System.Random random)
{
  List<int> modelNumber = new();

  for (int n = 0; n < 14; ++n)
    modelNumber.Add(1 + random.Next(9));

  return modelNumber;
}

string FormatModelNumber(List<int> modelNumber)
{
  StringBuilder sb = new StringBuilder();
  foreach (var n in modelNumber)
    sb.Append(n);
  return sb.ToString();
}

void CalcFinalModelNumber()
{
  for (long n0 = 9; n0 > 0; --n0)
    for (long n1 = 9; n1 > 0; --n1)
      for (long n2 = 9; n2 > 0; --n2)
      {
        long n3 = (((n0 + 13) * 26 + n1 + 10) * 26 + n2 + 3) % 26 + -11;
        if (n3 <= 0 || n3 >= 10)
          continue;

        for (long n4 = 9; n4 > 0; --n4)
        {
          long n5 = ((((n0 + 13) * 26 + n1 + 10) * 26 + n2 + 3) / 26 * 26 + n4 + 9) % 26 + -4;
          if (n5 <= 0 || n5 >= 10)
            continue;

          for (long n6 = 9; n6 > 0; --n6)
            for (long n7 = 9; n7 > 0; --n7)
              for (long n8 = 9; n8 > 0; --n8)
              {
                long n9 = (((((((n0 + 13) * 26 + n1 + 10) * 26 + n2 + 3) / 26 * 26 + n4 + 9) / 26 * 26 + n6 + 5) * 26 + n7 + 1) * 26 + n8) % 26 + -2;
                if (n9 <= 0 || n9 >= 10)
                  continue;

                long n10 = ((((((((n0 + 13) * 26 + n1 + 10) * 26 + n2 + 3) / 26 * 26 + n4 + 9) / 26 * 26 + n6 + 5) * 26 + n7 + 1) * 26 + n8) / 26) % 26 + -5;
                if (n10 <= 0 || n10 >= 10)
                  continue;

                long n11 = (((((((((n0 + 13) * 26 + n1 + 10) * 26 + n2 + 3) / 26 * 26 + n4 + 9) / 26 * 26 + n6 + 5) * 26 + n7 + 1) * 26 + n8) / 26) / 26) % 26 + -11;
                if (n11 <= 0 || n11 >= 10)
                  continue;

                long n12 = ((((((((((n0 + 13) * 26 + n1 + 10) * 26 + n2 + 3) / 26 * 26 + n4 + 9) / 26 * 26 + n6 + 5) * 26 + n7 + 1) * 26 + n8) / 26) / 26) / 26) % 26 + -13;
                if (n12 <= 0 || n12 >= 10)
                  continue;

                long n13 = ((((((((((n0 + 13) * 26 + n1 + 10) * 26 + n2 + 3) / 26 * 26 + n4 + 9) / 26 * 26 + n6 + 5) * 26 + n7 + 1) * 26 + n8) / 26) / 26) / 26) / 26 + -10;
                if (n13 <= 0 || n13 >= 10)
                  continue;

                System.Console.Write("Found Number: ");
                System.Console.Write(n0);
                System.Console.Write(n1);
                System.Console.Write(n2);
                System.Console.Write(n3);
                System.Console.Write(n4);
                System.Console.Write(n5);
                System.Console.Write(n6);
                System.Console.Write(n7);
                System.Console.Write(n8);
                System.Console.Write(n9);
                System.Console.Write(n10);
                System.Console.Write(n11);
                System.Console.Write(n12);
                System.Console.Write(n13);
                System.Console.WriteLine();
              }
        }
      }
}

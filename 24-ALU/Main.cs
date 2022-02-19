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

RandomAnalysis();

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

    System.Console.WriteLine($"{lineNumber}: {line}");

    System.Console.WriteLine(Parser.FormatSymbolicALU(alu));
    System.Console.ReadKey();
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

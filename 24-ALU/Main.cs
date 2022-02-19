using _24_ALU;
using System.IO;

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


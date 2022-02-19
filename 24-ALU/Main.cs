using _24_ALU;
using System.IO;

int instructionCount = 0;
var program = File.ReadLines(@"input.txt");
var alu = new SymbolicALU();

foreach (var line in program)
{
  var instruction = Parser.ParseLine(line);
  alu.ProcessInstruction(instruction);

  ++instructionCount;
  System.Console.WriteLine($"{instructionCount}: {line}");

  System.Console.WriteLine(Parser.FormatSymbolicALU(alu));
  System.Console.ReadKey();
}

System.Console.WriteLine(Parser.FormatSymbolicALU(alu));


using _24_ALU;
using System.IO;

int instructionCount = 0;
var program = File.ReadLines(@"input.txt");
var alu = new SymbolicALU();

foreach (var line in program)
{
  ++instructionCount;
  System.Console.WriteLine($"{instructionCount}: {line}");

  var instruction = Parser.ParseLine(line);
  alu.ProcessInstruction(instruction);

  foreach (var option in alu.GetOptions())
  {
    var condition = Parser.Format(option.Condition);
    System.Console.WriteLine($"\t[{condition}]:");
    var registers = Parser.Format(option.State).Replace(", ", "\n\t\t");
    System.Console.WriteLine("\t\t" + registers);
  }

  System.Console.ReadKey();
}

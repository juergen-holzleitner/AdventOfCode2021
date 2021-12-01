// See https://aka.ms/new-console-template for more information
var input = File.ReadAllLines("Input.txt");
int previousValue = int.MaxValue;
int numIncreases = 0;
foreach (var line in input)
{
  int currentValue = int.Parse(line.Trim());
  if (currentValue > previousValue)
  {
    ++numIncreases;
  }
  previousValue = currentValue;
}

Console.WriteLine("Increases: " + numIncreases);

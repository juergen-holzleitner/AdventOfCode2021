// See https://aka.ms/new-console-template for more information
var input = File.ReadAllLines("Input.txt");
int previousValue = int.MaxValue;
int numIncreases = 0;
var values = new Queue<int>();
const int slidingWindowSize = 3;
foreach (var line in input)
{
  int newVal = int.Parse(line.Trim());
  values.Enqueue(newVal);
  
  while (values.Count > slidingWindowSize)
    values.Dequeue();

  if (values.Count >= slidingWindowSize)
  {
    int currentValue = values.Sum();
    if (currentValue > previousValue)
    {
      ++numIncreases;
    }
    previousValue = currentValue;
  }
}

Console.WriteLine("Increases: " + numIncreases);

var input = GetInput(@"input.txt");

int? minNeededFuel = null;
for (int n = input.Min(); n <= input.Max(); n++)
{
  int neededFuel = GetNeededFuel(input, n);
  // Console.WriteLine($"{n}: {neededFuel}");
  if (!minNeededFuel.HasValue || neededFuel < minNeededFuel)
    minNeededFuel = neededFuel;
}

Console.WriteLine(minNeededFuel);

int GetNeededFuel(int[] input, int n)
{
  int fuel = 0;
  foreach (var i in input)
  {
    fuel += Math.Abs(n - i);
  }
  return fuel;
}

int[] GetInput(string fileName)
{
  var data = File.ReadAllText(fileName);
  var values = (from n in data.Split(',') select int.Parse(n)).ToArray();
  return values;
}

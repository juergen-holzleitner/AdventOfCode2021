var input = GetInput(@"input.txt");

int? minNeededFuel = null;
for (int n = input.Min(); n <= input.Max(); n++)
{
  int neededFuel = GetNeededFuel(input, n, false);
  // Console.WriteLine($"{n}: {neededFuel}");
  if (!minNeededFuel.HasValue || neededFuel < minNeededFuel)
    minNeededFuel = neededFuel;
}

Console.WriteLine(minNeededFuel);

int GetNeededFuel(int[] input, int n, bool part1)
{
  int fuel = 0;
  foreach (var i in input)
  {
    var diff = Math.Abs(n - i);
    if (part1)
    {
      fuel += diff;
    }
    else
    {
      fuel += diff * (diff + 1) / 2;
    }
  }
  return fuel;
}

int[] GetInput(string fileName)
{
  var data = File.ReadAllText(fileName);
  var values = (from n in data.Split(',') select int.Parse(n)).ToArray();
  return values;
}

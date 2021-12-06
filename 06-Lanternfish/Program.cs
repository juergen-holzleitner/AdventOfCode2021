var input = GetInput(@"input.txt");

for (int i = 0; i < 80; ++i)
{
  input = ProcessDay(input);
  // Console.WriteLine($"{i}: {String.Join(',', input)}");
}

Console.WriteLine(input.Count);

List<int> GetInput(string fileName)
{
  var s = File.ReadAllText(fileName);
  var data = from n in s.Split(',') select int.Parse(n);
  return data.ToList();
}

List<int> ProcessDay(List<int> current)
{
  List<int> newDay = new();

  int itemsToAdd = 0;
  foreach (var n in current)
  {
    if (n == 0)
    {
      ++itemsToAdd;
      newDay.Add(6);
    }
    else
    {
      newDay.Add(n - 1);
    }
  }

  for (int n = 0; n < itemsToAdd; ++n)
    newDay.Add(8);

  return newDay;
}
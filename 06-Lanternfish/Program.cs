var input = GetInput(@"input.txt");

for (int i = 0; i < 256; ++i)
{
  input = ProcessDay(input);
}

Console.WriteLine(input.Sum());

long[] GetInput(string fileName)
{
  var s = File.ReadAllText(fileName);
  var data = from n in s.Split(',') select int.Parse(n);
  var array = new long[9];
  foreach (var n in data)
  {
    ++array[n];
  }
  return array;
}

long[] ProcessDay(long[] current)
{
  var result = new long[current.Length];

  for (int n = 1; n < current.Length; ++n)
  {
    result[n - 1] = current[n];
  }

  result[6]+=current[0];
  result[8] += current[0];

  return result;
}
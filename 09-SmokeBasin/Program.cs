var input = GetInput(@"input.txt");

var lowPoints = GetLowPoints(input);
int risk = lowPoints.Sum(l => l + 1);
Console.WriteLine(risk);


List<int> GetLowPoints(List<List<int>> input)
{
  var lowPoints = new List<int>();

  for (int row = 0; row < input.Count; row++)
    for (int col = 0; col < input[row].Count; col++)
    {
      int height = input[row][col];

      if (col > 0 && input[row][col - 1] <= height)
        continue;
      if (col < input[row].Count - 1 && input[row][col + 1] <= height)
        continue;
      if (row > 0 && input[row - 1][col] <= height)
        continue;
      if (row < input.Count - 1 && input[row + 1][col] <= height)
        continue;

      lowPoints.Add(height);
    }
  return lowPoints;
}


List<List<int>> GetInput(string fileName)
{
  var lines = File.ReadLines(fileName);
  List<List<int>> input = new();

  foreach (var line in lines)
  {
    var row = (from x in line select int.Parse(x.ToString())).ToList();
    input.Add(row);
  }
  return input;
}

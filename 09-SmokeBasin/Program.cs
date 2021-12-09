var input = GetInput(@"input.txt");

var lowPoints = GetLowPoints(input, true);
// int risk = lowPoints.Sum(l => l + 1);
var result = (from l in lowPoints orderby l descending select l).Take(3);
var f = 1;
foreach (var r in result)
{
  f *= r;
}
Console.WriteLine(f);


List<int> GetLowPoints(List<List<int>> input, bool getBasins)
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

      if (getBasins)
        lowPoints.Add(GetBasins(input, row, col));
      else
        lowPoints.Add(height);
    }
  return lowPoints;
}

int GetBasins(List<List<int>> input, int row, int col)
{
  bool[,] visited = new bool[input.Count, input[0].Count];
  Queue<Pos> positionsToVisit = new();
  positionsToVisit.Enqueue(new Pos(row, col));

  int basinSize = 0;

  while (positionsToVisit.Any())
  {
    var currentPos = positionsToVisit.Dequeue();
    if (!visited[currentPos.Row, currentPos.Col])
    {
      visited[currentPos.Row, currentPos.Col] = true;
      var height = input[currentPos.Row][currentPos.Col];

      if (height < 9)
      {
        ++basinSize;


        if (currentPos.Row > 0)
        {
          var pos = new Pos(currentPos.Row - 1, currentPos.Col);
          if (input[pos.Row][pos.Col] > height)
            positionsToVisit.Enqueue(pos);
        }

        if (currentPos.Row < input.Count - 1)
        {
          var pos = new Pos(currentPos.Row + 1, currentPos.Col);
          if (input[pos.Row][pos.Col] > height)
            positionsToVisit.Enqueue(pos);
        }

        if (currentPos.Col > 0)
        {
          var pos = currentPos with { Col = currentPos.Col - 1 };
          if (input[pos.Row][pos.Col] > height)
            positionsToVisit.Enqueue(pos);
        }

        if (currentPos.Col < input[0].Count - 1)
        {
          var pos = currentPos with { Col = currentPos.Col + 1 };
          if (input[pos.Row][pos.Col] > height)
            positionsToVisit.Enqueue(pos);
        }
      }
    }
  }

  return basinSize;
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

readonly record struct Pos(int Row, int Col);

var input = GetInput(@"input.txt");
HashSet<Point> points = input.Points;
foreach (var fold in input.Folds)
{
  points = Fold(points, fold);
}

PrintPoints(points);


void PrintPoints(HashSet<Point> points)
{
  int maxX = points.Max(p => p.x);
  int maxY = points.Max(p => p.y);

  for (int y = 0; y <= maxY; y++)
  {
    for (int x = 0; x <= maxX; x++)
    {
      if (points.Contains(new Point(x, y)))
        Console.Write('#');
      else
        Console.Write(' ');
    }
    Console.WriteLine();
  }
}

HashSet<Point> Fold(HashSet<Point> input, Point foldPos)
{
  var newPoints = new HashSet<Point>();
  foreach (var point in input)
  {
    if (foldPos.x == 0)
    {
      System.Diagnostics.Debug.Assert(point.y != foldPos.y);
      if (point.y < foldPos.y)
        newPoints.Add(point);
      else
        newPoints.Add(point with { y = 2 * foldPos.y - point.y });
    }
    else if (foldPos.y == 0)
    {
      System.Diagnostics.Debug.Assert(point.x != foldPos.x);
      if (point.x < foldPos.x)
        newPoints.Add(point);
      else
        newPoints.Add(point with { x = 2 * foldPos.x - point.x });
    }
  }
  return newPoints;
}

Input GetInput(string fileName)
{
  HashSet<Point> points = new();
  List<Point> folds = new();

  var regPoints = new System.Text.RegularExpressions.Regex(@"(?<x>\d+),(?<y>\d+)");
  var regFold = new System.Text.RegularExpressions.Regex(@"fold along (?<axis>[x,y])=(?<pos>\d+)");

  foreach (var line in File.ReadLines(fileName))
  {
    var matchPoints = regPoints.Match(line);
    if (matchPoints.Success)
    { 
      var x = int.Parse(matchPoints.Groups["x"].Value);
      var y = int.Parse(matchPoints.Groups["y"].Value);
      var point = new Point(x, y);
      points.Add(point);
    }
    else
    {
      var matchFold = regFold.Match(line);
      if (matchFold.Success)
      {
        var axis = matchFold.Groups["axis"].Value;
        var pos = int.Parse(matchFold.Groups["pos"].Value);

        Point? p = null;
        if (axis == "x")
          p = new Point(pos, 0);
        else if (axis == "y")
          p = new Point(0, pos);

        System.Diagnostics.Debug.Assert(p != null);
        folds.Add(p.Value);
      }
      else
      {
        if (!string.IsNullOrEmpty(line))
        {
          Console.WriteLine(line);
          System.Diagnostics.Debug.Assert(false);
        }
      }
    }
  }

  return new Input(points, folds);
}

readonly record struct Point(int x, int y);

readonly record struct Input(HashSet<Point> Points, List<Point> Folds);



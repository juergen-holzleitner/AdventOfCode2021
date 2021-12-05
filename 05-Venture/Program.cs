// See https://aka.ms/new-console-template for more information

var data = ReadInput(@"input.txt");
var overlapInfo = GetOverlapInfo(data);
var numberWithMultipleIntersection = (from pi in overlapInfo.PositionInfos where pi.Value >= 2 select pi).Count();
Console.WriteLine(numberWithMultipleIntersection);


OverlapInfo GetOverlapInfo(Input input)
{
  var overlapInfo = new OverlapInfo();
  foreach (var line in input.Lines)
  {
    var distanceVector = line.end - line.start;
    int steps = 0;

    if (distanceVector.x == 0)
    {
      steps = Math.Abs(distanceVector.y);
      distanceVector = distanceVector with { y = distanceVector.y / steps };
    }
    else if (distanceVector.y == 0)
    {
      steps = Math.Abs(distanceVector.x);
      distanceVector = distanceVector with { x = distanceVector.x / steps };
    }
    else
    {
      steps = Math.Abs(distanceVector.y);
      System.Diagnostics.Debug.Assert(steps == Math.Abs(distanceVector.x));
      distanceVector /= steps;
    }

    for (int n = 0; n <= steps; ++n)
    {
      var p = line.start + n * distanceVector;
      AddPointToPositionInfo(p, overlapInfo);
    }
  }
  return overlapInfo;
}

void AddPointToPositionInfo(Point p, OverlapInfo overlapInfo)
{
  if (overlapInfo.PositionInfos.TryGetValue(p, out int value))
    overlapInfo.PositionInfos[p] = value + 1;
  else
    overlapInfo.PositionInfos.Add(p, 1);
}

Input ReadInput(string fileName)
{
  var data = File.ReadLines(fileName);
  var input = new Input();
  var regEx = new System.Text.RegularExpressions.Regex(@"(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)");
  foreach (var line in data)
  {
    var groups = regEx.Match(line).Groups;

    var p1 = new Point(int.Parse(groups["x1"].Value), int.Parse(groups["y1"].Value));
    var p2 = new Point(int.Parse(groups["x2"].Value), int.Parse(groups["y2"].Value));

    input.Lines.Add(new Line(p1, p2));
  }

  return input;
}

class OverlapInfo
{
  public Dictionary<Point, int> PositionInfos = new();
}

class Input
{
  public List<Line> Lines { get; set; } = new();
}

readonly record struct Point (int x, int y)
{
  public static Point operator -(Point a, Point b)
  {
    return new(a.x - b.x, a.y - b.y);
  }
  public static Point operator +(Point a, Point b)
  {
    return new(a.x + b.x, a.y + b.y);
  }
  public static Point operator *(int n, Point a)
  {
    return new(a.x * n, a.y * n);
  }
  public static Point operator /(Point a, int n)
  {
    return new(a.x / n, a.y / n);
  }
}

readonly record struct Line (Point start, Point end);

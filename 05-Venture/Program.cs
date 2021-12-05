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
    if (line.start.x == line.end.x)
    {
      int start = Math.Min(line.start.y, line.end.y);
      int end = Math.Max(line.start.y, line.end.y);
      for (int y = start; y <= end; ++y)
      {
        var p = line.start with { y = y };
        AddPointToPositionInfo(p, overlapInfo);
      }
    } 
    else if (line.start.y == line.end.y)
    {
      int start = Math.Min(line.start.x, line.end.x);
      int end = Math.Max(line.start.x, line.end.x);
      for (int x = start; x <= end; ++x)
      {
        var p = line.start with { x = x };
        AddPointToPositionInfo(p, overlapInfo);
      }
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

    if (p1.x == p2.x || p1.y == p2.y)
    {
      input.Lines.Add(new Line(p1, p2));
    }
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

readonly record struct Point (int x, int y);

readonly record struct Line (Point start, Point end);

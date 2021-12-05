// See https://aka.ms/new-console-template for more information

var data = ReadInput(@"input-small.txt");
foreach (var item in data.Lines)
{
  Console.WriteLine(item);
}

Input ReadInput(string fileName)
{
  var data = File.ReadLines(fileName);
  var input = new Input();
  var regEx = new System.Text.RegularExpressions.Regex(@"(?<x1>\d),(?<y1>\d) -> (?<x2>\d),(?<y2>\d)");
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

class Input
{
  public List<Line> Lines { get; set; } = new();
}

readonly record struct Point (int x, int y);

readonly record struct Line (Point start, Point end);

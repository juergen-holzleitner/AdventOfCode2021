var input = GetInput(@"input-small.txt");
Console.WriteLine(input);


Input GetInput(string fileName)
{
  List<Point> points = new();
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

readonly record struct Input(List<Point> Points, List<Point> Folds);



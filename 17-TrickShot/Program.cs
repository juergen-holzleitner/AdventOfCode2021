var input = GetInput(@"input.txt");

var steps = GetPossibleNumXSteps(input);

var (maxSpeed, maxPos) = GetMaxYPos(input);

Console.WriteLine($"maxSpeed: {maxSpeed}, maxPos: {maxPos}");


(int?, HashSet<int>) GetPossibleNumXSteps(Input input)
{
  HashSet<int> res = new();
  int? minInfinite = null;
  for (int speed = input.MaxX; speed >= 0; --speed)
  {
    int pos = 0;
    int steps = 0;
    int curSpeed = speed;
    while (curSpeed > 0)
    {
      pos += curSpeed;
      --curSpeed;
      ++steps;

      if (pos >= input.MinX && pos<= input.MaxX)
      {
        res.Add(steps);
        if (curSpeed == 0)
        {
          if (!minInfinite.HasValue || steps < minInfinite.Value)
            minInfinite = steps;
        }
      }
    }
  }

  return (minInfinite, res);
}

(int, int) GetMaxYPos(Input input)
{
  int? maxSpeed = null;
  int? maxPos = null;

  for (int speed = input.MinY; speed <= -input.MinY; ++speed)
  {
    int pos = 0;
    int maxYPos = pos;
    int curSpeed = speed;
    while (pos >= input.MinY)
    {
      pos += curSpeed;
      --curSpeed;
      if (pos > maxYPos)
      {
        maxYPos = pos;
      }

      if (pos >= input.MinY && pos <= input.MaxY)
      {
        if (!maxPos.HasValue || maxPos.Value < maxYPos)
        {
          maxPos = maxYPos;
          maxSpeed = speed;
        }
      }
    }
  }

  if (!maxPos.HasValue || !maxSpeed.HasValue)
    throw new ApplicationException("no solution found");

  return (maxSpeed.Value, maxPos.Value);
}

Input GetInput(string fileName)
{
  var line = File.ReadAllText(fileName);
  var regEx = new System.Text.RegularExpressions.Regex(@"target area: x=(?<x1>[+-]?\d+)..(?<x2>[+-]?\d+), y=(?<y1>[+-]?\d+)..(?<y2>[+-]?\d+)");
  var match = regEx.Match(line);
  if (match.Success)
  {
    var x1 = int.Parse(match.Groups["x1"].Value);
    var x2 = int.Parse(match.Groups["x2"].Value);
    var y1 = int.Parse(match.Groups["y1"].Value);
    var y2 = int.Parse(match.Groups["y2"].Value);

    return new Input(x1, x2, y1, y2);
  }
  throw new ApplicationException("invalid input");
}


readonly record struct Input(int MinX, int MaxX, int MinY, int MaxY);
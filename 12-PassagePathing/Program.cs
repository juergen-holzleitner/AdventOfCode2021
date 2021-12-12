var connections = GetInput(@"input.txt");

var startPositions = GetStartPositions(connections);
var endPositions = GetEndPositions(connections, startPositions);
Console.WriteLine(endPositions.Count());

List<Position> GetEndPositions(List<Connection> connections, List<Position> startPositions)
{
  List<Position> endPositions = new();

  Stack<Position> stack = new ();

  foreach (var position in startPositions)
  {
    stack.Push (position);
  }


  while (stack.Any())
  {
    var position = stack.Pop();
    var currentPos = position.Path.Last();
    if (currentPos == "end")
    {
      endPositions.Add(position);
    }
    else
    {
      var nextPos1 = from x in connections where x.Start == currentPos select x.End;
      var nextPos2 = from x in connections where x.End == currentPos select x.Start;
      var nextPos = nextPos1.Union(nextPos2);
      
      foreach (var n in nextPos)
      {
        if (IsSmallCave(n))
        {
          if (!position.VisitedSmallCaves.Contains(n))
          {
            var visitedSmallCaves = new List<string>(position.VisitedSmallCaves);
            visitedSmallCaves.Add(n);
            var path = new List<string>(position.Path);
            path.Add(n);

            var newPos = new Position(visitedSmallCaves, path);
            stack.Push(newPos);
          }
        }
        else
        {
          var path = new List<string>(position.Path);
          path.Add(n);
          var newPos = new Position(position.VisitedSmallCaves, path);
          stack.Push(newPos);
        }
      }
    }

  }

  return endPositions;
}

bool IsSmallCave(string cave)
{
  return char.IsLower(cave.First());
}

List<Connection> GetInput(string fileName)
{
  List<Connection> connections = new();

  var regExp = new System.Text.RegularExpressions.Regex(@"(?<start>\w+)-(?<end>\w+)");
  foreach (var line in File.ReadLines(fileName))
  {
    var match = regExp.Match(line);
    if (match.Success)
    {
      var start = match.Groups["start"].Value;
      var end = match.Groups["end"].Value;
      connections.Add(new Connection(start, end));
    }
  }

  return connections;
}

List<Position> GetStartPositions(List<Connection> connections)
{
  foreach (var connection in connections)
  {
    if (connection.Start == "start" || connection.End == "start")
    {
      return new List<Position>()
      {
        new Position(new List<string>(){"start"}, new List<string>(){"start"}),
      };
    }
  }

  System.Diagnostics.Debug.Assert(false);
  return null;
}

readonly record struct Connection(string Start , string End);

readonly record struct Position(List<string> VisitedSmallCaves, List<string> Path);


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
        if (n != "start")
        {
          if (IsSmallCave(n))
          {
            var visitedTwice = from v in position.VisitedSmallCaves where v.Value >= 2 select v;

            if (!position.VisitedSmallCaves.ContainsKey(n) || !visitedTwice.Any())
            {
              var visitedSmallCaves = new Dictionary<string, int>(position.VisitedSmallCaves);

              if (visitedSmallCaves.ContainsKey(n))
                visitedSmallCaves[n]++;
              else
              {
                if (n == "start" || n == "end")
                  visitedSmallCaves.Add(n, 2);
                else
                  visitedSmallCaves.Add(n, 1);
              }
            
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
      var visited = new Dictionary<string, int>()
      {
        { "start", 1 },
      };

      return new List<Position>()
      {
       
        new Position(visited, new List<string>(){"start"}),
      };
    }
  }

  System.Diagnostics.Debug.Assert(false);
  return null;
}

readonly record struct Connection(string Start , string End);

readonly record struct Position(Dictionary<string, int> VisitedSmallCaves, List<string> Path);


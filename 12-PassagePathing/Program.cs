var connections = GetInput(@"input-small.txt");
Console.WriteLine();

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


readonly record struct Connection(string start , string end);
var input = File.ReadLines(@"Input.txt");
int horizontalPosition = 0;
int depth = 0;
foreach (var line in input)
{
  var tokens = line.Split(' ');
  
  // skip empty lines
  if (tokens.Length < 2)
    continue;

  int value = int.Parse(tokens[1]);
  switch (tokens[0].Trim())
  {
    case "forward":
      horizontalPosition += value;
      break;
    case "down":
      depth += value;
      break;
    case "up":
      depth -= value;
      break;
    default:
      System.Diagnostics.Debug.Assert(false, $"Invalid token {tokens[0]}");
      break;

  }
}
Console.WriteLine(horizontalPosition * depth);

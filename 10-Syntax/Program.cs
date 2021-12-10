var input = GetInput(@"input.txt");

Dictionary<char, char> braceMatches = new()
{
  { '(', ')' },
  { '[', ']' },
  { '{', '}' },
  { '<', '>' },
};

Dictionary<char, int> errorScore = new()
{
  { ')', 3 },
  { ']', 57 },
  { '}', 1197 },
  { '>', 25137 },
};

Dictionary<char, int> closingScore = new()
{
  { ')', 1 },
  { ']', 2 },
  { '}', 3 },
  { '>', 4 },
};

//int totalScore = 0;
//foreach (var line in input)
//{
//  var invalidClosing = GetInvalidClosing(line);
//  if (invalidClosing != null)
//  {
//    totalScore += errorScore[invalidClosing.Value];
//  }
//}

List<long> closingScores = new();
foreach (var line in input)
{
  var missingClosings = GetMissingClosings(line);
  if (!missingClosings.Any())
    continue;

  long totalClosingScore = 0;
  foreach (var c in missingClosings)
  {
    totalClosingScore *= 5;
    totalClosingScore += closingScore[c];
  }
  if (totalClosingScore > 0)
    closingScores.Add(totalClosingScore);
}

var sortedScores = closingScores.OrderBy(s => s).ToList();

var middleScore = sortedScores[sortedScores.Count() / 2];

Console.WriteLine(middleScore);

List<char> GetMissingClosings(string line)
{
  Stack<char> openings = new();

  List<char> missingClosings = new();

  foreach (char c in line)
  {
    if (IsOpening(c))
      openings.Push(c);
    else
    {
      var stacked = openings.Pop();
      if (c != braceMatches[stacked])
        return missingClosings;
    }
  }

  while (openings.Any())
  {
    var closing = braceMatches[openings.Pop()];
    missingClosings.Add(closing);
  }
  return missingClosings;
}

char? GetInvalidClosing(string line)
{
  Stack<char> openings = new();

  foreach (char c in line)
  {
    if (IsOpening(c))
      openings.Push(c);
    else
    {
      var stacked = openings.Pop();
      if (c != braceMatches[stacked])
        return c;
    }
  }

  return null;
}

IEnumerable<string> GetInput(string fileName)
{
  return File.ReadLines(fileName);
}



bool IsOpening(char ch)
{
  return braceMatches.ContainsKey(ch);
}

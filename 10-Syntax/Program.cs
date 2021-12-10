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

int totalScore = 0;
foreach (var line in input)
{
  var invalidClosing = GetInvalidClosing(line);
  if (invalidClosing != null)
  {
    totalScore += errorScore[invalidClosing.Value];
  }
}

Console.WriteLine(totalScore);

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

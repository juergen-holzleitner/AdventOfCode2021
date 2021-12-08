var input = GetInput(@"input-small.txt");

Console.WriteLine();

IEnumerable<Input> GetInput(string fileName)
{
  var fileData = File.ReadLines(fileName);
  var regEx = new System.Text.RegularExpressions.Regex(@"(?<pattern>.+) \| (?<output>.+)");
  foreach (var line in fileData)
  {
    var match = regEx.Match(line);
    if (match.Success)
    {
      var pattern = match.Groups["pattern"].Value;
      var output = match.Groups["output"].Value;


      yield return new Input(pattern.Split(' '), output.Split(' '));
    }
  }
}

readonly record struct Input(IEnumerable<string> Patterns, IEnumerable<string> Outputs);

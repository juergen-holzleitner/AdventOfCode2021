var input = GetInput(@"input.txt");

var part1 = from i in input 
            from o in i.Outputs
            where o.Length == 2 || o.Length == 4 || o.Length == 3 || o.Length == 7 
            select o;

Console.WriteLine(part1.Count());

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

var input = GetInput(@"input.txt");
var newPolymer = input.PolymerTemplate;

for (int i = 0; i < 40; ++i)
  newPolymer = ProcessStep(newPolymer, input.InsertionRules);

var charDistribution = (from c in newPolymer
                        group c by c into g
                        orderby g.Count() descending
                        select new { ch = g.Key, cnt = g.Count() }
                        );
var most = charDistribution.First().cnt;
var least = charDistribution.Last().cnt;
Console.WriteLine(most - least);


string ProcessStep(string polymerTemplate, IDictionary<string, string> insertionRules)
{
  System.Text.StringBuilder sb = new();
  for (int i=0; i < polymerTemplate.Length - 1; i++)
  {
    sb.Append(polymerTemplate[i]);
    if (insertionRules.TryGetValue(polymerTemplate.Substring(i, 2), out var value))
      sb.Append(value);
  }
  sb.Append(polymerTemplate[^1]);

  return sb.ToString();
}

Input GetInput(string fileName)
{
  Dictionary<string, string> rules = new();
  string? template = null;

  var lines = File.ReadLines(fileName);
  foreach (var line in lines)
  {
    var regexInsertion = new System.Text.RegularExpressions.Regex(@"(?<pair>[A-Z]{2}) -> (?<ins>[A-Z])");
    var regexTemp = new System.Text.RegularExpressions.Regex(@"[A-Z]+");
    var match = regexInsertion.Match(line);
    if (match.Success)
    {
      rules.Add(match.Groups["pair"].Value, match.Groups["ins"].Value);
    }
    else
    { 
      var m = regexTemp.Match(line);
      if (m.Success)
      {
        System.Diagnostics.Debug.Assert(template == null);
        template = m.Value;
      }
      else
      {
        System.Diagnostics.Debug.Assert(string.IsNullOrEmpty(line));
      }
    }
  }

  System.Diagnostics.Debug.Assert(template != null);
  return new Input(template, rules);
}

readonly record struct Input(string PolymerTemplate, IDictionary<string, string> InsertionRules);

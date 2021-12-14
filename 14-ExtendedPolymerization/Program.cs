var input = GetInput(@"input.txt");
const int maxDepth = 10;

System.Text.StringBuilder sb = new();
sb.Append('N');
for (int n = 0; n < input.PolymerTemplate.Length - 1; n++)
{
  Process(0, input.PolymerTemplate[n], input.PolymerTemplate[n + 1], input.InsertionRules);
}


var charDistribution = (from c in sb.ToString()
                        group c by c into g
                        orderby g.Count() descending
                        select new { ch = g.Key, cnt = g.Count() }
                        );
var most = charDistribution.First().cnt;
var least = charDistribution.Last().cnt;
Console.WriteLine(most - least);


void Process(int depth, char left, char right, IDictionary<Tuple<char, char>, char> insertionRules)
{
  if (depth < maxDepth && insertionRules.TryGetValue(new (left, right), out char value))
  {
    Process(depth + 1, left, value, insertionRules);
    Process(depth + 1, value, right, insertionRules);
  }
  else
  {
    sb.Append(right);
  }
}

Input GetInput(string fileName)
{
  Dictionary<Tuple<char, char>, char> rules = new();
  string? template = null;

  var lines = File.ReadLines(fileName);
  foreach (var line in lines)
  {
    var regexInsertion = new System.Text.RegularExpressions.Regex(@"(?<pair>[A-Z]{2}) -> (?<ins>[A-Z])");
    var regexTemp = new System.Text.RegularExpressions.Regex(@"[A-Z]+");
    var match = regexInsertion.Match(line);
    if (match.Success)
    {
      var p = new Tuple<char, char>(match.Groups["pair"].Value[0], match.Groups["pair"].Value[1]);
      rules.Add(p, match.Groups["ins"].Value[0]);
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

readonly record struct Input(string PolymerTemplate, IDictionary<Tuple<char, char>, char> InsertionRules);

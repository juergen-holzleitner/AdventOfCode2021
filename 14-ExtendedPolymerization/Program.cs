var input = GetInput(@"input-small.txt");

Input GetInput(string fileName)
{
  List<InsertionRule> rules = new List<InsertionRule>();
  string? template = null;

  var lines = File.ReadLines(fileName);
  foreach (var line in lines)
  {
    var regexInsertion = new System.Text.RegularExpressions.Regex(@"(?<pair>[A-Z]{2}) -> (?<ins>[A-Z])");
    var regexTemp = new System.Text.RegularExpressions.Regex(@"[A-Z]+");
    var match = regexInsertion.Match(line);
    if (match.Success)
    {
      var insertionRule = new InsertionRule(match.Groups["pair"].Value, match.Groups["ins"].Value);
      rules.Add(insertionRule);
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

readonly record struct InsertionRule(string pair, string insert);
readonly record struct Input(string PolymerTemplate, IEnumerable<InsertionRule> InsertionRules);

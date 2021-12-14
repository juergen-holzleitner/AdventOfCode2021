var input = GetInput(@"input.txt");
int maxDepth = 10;

var hist = GetHistogram(input.PolymerTemplate, input.InsertionRules);

var most = (from h in hist orderby h.Value descending select h).First();
var least = (from h in hist orderby h.Value select h).First();

Console.WriteLine($"{most.Key}: {most.Value}, {least.Key}: {least.Value} -> {most.Value - least.Value}");

Dictionary<char, long> GetHistogram(string template, IDictionary<Tuple<char, char>, char> rules)
{
  Dictionary<char, long> histogram = new();
  AddToHistogram(histogram, template[0]);

  for (int n = 0; n < template.Length - 1; n++)
  {
    Process(0, template[n], template[n + 1], histogram, rules);
  }
  return histogram;
}

void Process(int depth, char left, char right, IDictionary<char, long> histogram, IDictionary<Tuple<char, char>, char> insertionRules)
{
  if (depth < maxDepth && insertionRules.TryGetValue(new (left, right), out char value))
  {
    Process(depth + 1, left, value, histogram, insertionRules);
    Process(depth + 1, value, right, histogram, insertionRules);
  }
  else
  {
    AddToHistogram(histogram, right);
  }
}

void AddToHistogram(IDictionary<char, long> histogram, char ch)
{
  if (!histogram.ContainsKey(ch))
    histogram.Add(ch, 1);
  else
    histogram[ch]++;
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

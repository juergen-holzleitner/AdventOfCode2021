var input = GetInput(@"input.txt");
const int maxDepth = 10;

Dictionary<char, long> histogram = new();
AddToHistogram(histogram, input.PolymerTemplate[0]);

for (int n = 0; n < input.PolymerTemplate.Length - 1; n++)
{
  Process(0, input.PolymerTemplate[n], input.PolymerTemplate[n + 1], input.InsertionRules);
}

var vals = histogram.Values;
var most = vals.Max();
var least = vals.Min();

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
    AddToHistogram(histogram, right);
  }
}

void AddToHistogram(IDictionary<char, long> h, char ch)
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

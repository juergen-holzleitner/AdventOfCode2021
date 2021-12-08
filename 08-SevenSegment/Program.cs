using System.Text;

var input = GetInput(@"input.txt");

//var part1 = from i in input 
//            from o in i.Outputs
//            where o.Length == 2 || o.Length == 4 || o.Length == 3 || o.Length == 7 
//            select o;

//Console.WriteLine(part1.Count());


int sum = 0;
foreach (var val in input)
{
  var mapping = GetMapping(val.Patterns);
  sum += GetNumber(mapping, val.Outputs);
}
Console.WriteLine(sum);

IDictionary<string, int> GetMapping(IEnumerable<string> patterns)
{
  Dictionary<string, int> digits = new();
  var one = (from p in patterns where p.Length == 2 select p).Single();
  digits.Add(SortedString(one), 1);
  var seven = (from p in patterns where p.Length == 3 select p).Single();
  digits.Add(SortedString(seven), 7);
  var four = (from p in patterns where p.Length == 4 select p).Single();
  digits.Add(SortedString(four), 4);
  var eight = (from p in patterns where p.Length == 7 select p).Single();
  digits.Add(SortedString(eight), 8);

  var h1 = new HashSet<char>(one);
  var h4 = new HashSet<char>(four);
  var h7 = new HashSet<char>(seven);

  var three = ( from p in patterns where p.Length == 5
             let h = new HashSet<char>(p)
             where !h1.Except(h).Any()
             select p).Single();
  digits.Add(SortedString(three), 3);

  var five = (from p in patterns
              where p.Length == 5 && p != three
              let h = new HashSet<char>(p)
              where h.Intersect(h4).Count() == 3
              select p).Single();
  digits.Add(SortedString(five), 5);

  var two = (from p in patterns
             where p.Length == 5 && p != three && p != five
             select p).Single();
  digits.Add(SortedString(two), 2);

  var nine = (from p in patterns
               where p.Length == 6
               let h = new HashSet<char>(p)
               where !h4.Except(h).Any()
               select p).Single();
  digits.Add(SortedString(nine), 9);

  var zero = (from p in patterns
              where p.Length == 6 && p != nine
              let h = new HashSet<char>(p)
              where !h1.Except(h).Any()
              select p).Single();
  digits.Add(SortedString(zero), 0);

  var six = (from p in patterns
             where p.Length == 6 && p != nine && p != zero
             select p).Single();
  digits.Add(SortedString(six), 6);

  return digits;
}

int GetNumber(IDictionary<string, int> mapping, IEnumerable<string> outputs)
{
  int n = 0;
  foreach (var output in outputs)
  {
    n *= 10;
    n += mapping[SortedString(output)];
  }
  return n;
}

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

static string SortedString(string s)
{
  return string.Concat(s.OrderBy(c => c));
}

readonly record struct Input(IEnumerable<string> Patterns, IEnumerable<string> Outputs);

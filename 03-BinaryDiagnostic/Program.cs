// See https://aka.ms/new-console-template for more information

(int[]?, int) CountIndividualBits(IEnumerable<string> input)
{
  int[]? individualBits = null;
  int numTotal = 0;

  foreach (var line in input)
  {
    if (individualBits == null)
    {
      if (line.Length <= 0)
        continue;

      individualBits = new int[line.Length];
    }

    bool? isLineValid = null;
    for (int i = 0; i < line.Length; i++)
    {
      if (!isLineValid.HasValue)
        isLineValid = true;

      if (line[i] == '1')
        ++individualBits[i];
      else if (line[i] == '0')
      {
      }
      else
      {
        System.Diagnostics.Trace.Assert(false, $"invalid character {line[i]}");
        isLineValid = false;
      }
    }

    if (isLineValid == true)
      ++numTotal;

  }
  return (individualBits, numTotal);
}

void Step1(string fileName)
{
  var input = File.ReadLines(fileName);

  (var individualBits, var numTotal) = CountIndividualBits(input);

  System.Diagnostics.Trace.Assert(individualBits != null);

  int regular = 0, inverted = 0;
  for (int i = 0; i < individualBits?.Length; ++i)
  {
    regular <<= 1;
    inverted <<= 1;

    if (individualBits[i] * 2 > numTotal)
      regular |= 1;
    else
      inverted |= 1;
  }

  Console.WriteLine(regular * inverted);
}

string GetMostSignificant(string fileName, bool most)
{
  var input = File.ReadAllLines(fileName).ToList();

  for (int pos = 0; ; ++pos)
  {
    (var individualBits, var numTotal) = CountIndividualBits(input);

    if (numTotal <= 1)
      break;

    var target = individualBits?[pos] * 2 >= numTotal ? '1' : '0';
    if (!most)
      target = individualBits?[pos] * 2 < numTotal ? '1' : '0';

    input = (from i in input where i[pos] == target select i).ToList();

    if (input.Count <= 1)
      break;
  }

  return input.Single();
}

void Step2(string fileName)
{
  var oxygen = GetMostSignificant(fileName, true);
  Console.WriteLine(oxygen);

  var scrubber = GetMostSignificant(fileName, false);
  Console.WriteLine(scrubber);
}

const string? fileName = @"input.txt";

// Step1(fileName);
Step2(fileName);

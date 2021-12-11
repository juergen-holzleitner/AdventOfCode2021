var input = GetInput(@"input.txt");

//int numFlashes = 0;
//for (int i = 0; i < 100; ++i)
//  numFlashes += ProcessNextStep(input); 
//Console.WriteLine(numFlashes);

int numSteps = 0;
while (!AreSynchronized(input))
{
  ProcessNextStep(input);
  ++numSteps;
}
Console.WriteLine(numSteps);


bool AreSynchronized(List<List<int>> input)
{
  for (int row = 0; row < input.Count; ++row)
    for (int col = 0; col < input[row].Count; ++col)
    {
      if (input[row][col] != input[0][0])
        return false;
    }
  return true;
}

int ProcessNextStep(List<List<int>> input)
{
  for (int row = 0; row < input.Count; ++row)
    for (int col = 0; col < input[row].Count; ++col)
      ++input[row][col];


  int numFlashes = 0;


  bool flashHappened;
  do
  {
    flashHappened = false;

    for (int row = 0; row < input.Count; ++row)
      for (int col = 0; col < input[row].Count; ++col)
      {
        if (input[row][col] > 9)
        {
          ++numFlashes;
          input[row][col] = 0;
          flashHappened = true;

          ProcessFlashIncrement(input, row, col);
        }
      }
  } while (flashHappened);


  return numFlashes;
}

void ProcessFlashIncrement(List<List<int>> input, int row, int col)
{
  for (int r = row -1; r <= row + 1; ++r)
    for (int c = col - 1; c <= col + 1; ++c)
    {
      if (!IsPosValid(input, r, c))
        continue;

      if (input[r][c] > 0)
        ++input[r][c];
    }
}

bool IsPosValid(List<List<int>> input, int r, int c)
{
  if (r < 0)
    return false;
  if (r > input.Count - 1)
    return false;
  if (c < 0)
    return false;
  if (c > input[r].Count - 1)
    return false;

  return true;
}

List<List<int>> GetInput(string fileName)
{
  var input = File.ReadLines(fileName);
  List<List<int>> data = new();

  foreach (var line in input)
  {
    var d = (from c in line select int.Parse(c.ToString())).ToList();
    data.Add(d);
  }

  return data;
}
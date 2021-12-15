var input = GetInput(@"input.txt");

input = GetExpandedBy(input, 5);

var finalPos = new Pos(input.Count() - 1, input[input.Count() - 1].Count() - 1);
var startPos = new Pos(0, 0);
input[startPos.row][startPos.col].MinTotalCost = 0;

ProcessPossibilities(startPos);

System.Diagnostics.Debug.Assert(input[finalPos.row][finalPos.col].MinTotalCost.HasValue);
Console.WriteLine(input[finalPos.row][finalPos.col].MinTotalCost);

void ProcessPossibilities(Pos startPos)
{
  Queue<Pos> queue = new Queue<Pos>();
  queue.Enqueue(startPos);

  while (queue.Any())
  {
    var currentPos = queue.Dequeue();
    foreach (var newPos in GetNextSteps(currentPos))
    {
      var newCost = input[currentPos.row][currentPos.col].MinTotalCost + input[newPos.row][newPos.col].Cost;
      if (input[newPos.row][newPos.col].MinTotalCost == null || input[newPos.row][newPos.col].MinTotalCost > newCost)
      {
        input[newPos.row][newPos.col].MinTotalCost = newCost;
        queue.Enqueue(newPos);
      }
    }
  }
}

IEnumerable<Pos> GetNextSteps(Pos currentPos)
{
  System.Diagnostics.Debug.Assert(input != null);

  if (currentPos.row > 0)
    yield return currentPos with { row = currentPos.row - 1 };
  if (currentPos.col > 0)
    yield return currentPos with { col = currentPos.col - 1 };
  if (currentPos.row < input.Count() -1)
    yield return currentPos with { row = currentPos.row + 1 };
  if (currentPos.col < input[currentPos.row].Count() - 1)
    yield return currentPos with { col = currentPos.col + 1 };
}

List<List<Field>> GetExpandedBy(List<List<Field>> field, int expansion)
{ 
  var res = new List<List<Field>>();
  for (int row = 0; row < field.Count; row++)
  {
    var r = new List<Field>();
    for (int col = 0; col < field[row].Count * expansion; col++)
    {
      var newCost = field[row][col % field[row].Count].Cost;
      newCost += col / field[row].Count;
      newCost = CalcModTen(newCost);

      r.Add(new Field(newCost, null));
    }

    res.Add(r);
  }

  for (int row = field.Count; row< field.Count*expansion; row++)
  {
    var baseRow = row % field.Count;
    int additionalCose = row / field.Count;
    var r = new List<Field>();

    for (int col = 0; col < res[baseRow].Count; ++col)
    {
      var newCost = res[baseRow][col].Cost;
      newCost += additionalCose;
      newCost = CalcModTen(newCost);
      r.Add(new Field(newCost, null));
    }

    res.Add(r);
  }

  return res;
}

int CalcModTen(int val)
{
  while (val >= 10)
  {
    val -= 9;
  }
  return val;
}

List<List<Field>> GetInput(string fileName)
{
  var input = new List<List<Field>>();
 
  foreach (var line in File.ReadLines(fileName))
  {
    var row = (from c in line select new Field(int.Parse(c.ToString()), null)).ToList();
    input.Add(row);
  }

  return input;
}

record Field(int Cost, int? MinTotalCost)
{
  public int? MinTotalCost { get; set; } = MinTotalCost;
}

readonly record struct Pos(int row, int col);

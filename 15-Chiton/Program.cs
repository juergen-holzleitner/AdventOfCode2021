var input = GetInput(@"input.txt");

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

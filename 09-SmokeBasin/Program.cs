var input = GetInput(@"input-small.txt");

Console.WriteLine();

List<List<int>> GetInput(string fileName)
{
  var lines = File.ReadLines(fileName);
  List<List<int>> input = new();

  foreach (var line in lines)
  {
    var row = (from x in line select int.Parse(x.ToString())).ToList();
    input.Add(row);
  }
  return input;
}

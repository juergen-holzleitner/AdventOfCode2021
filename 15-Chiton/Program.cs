var intput = GetInput(@"input-small.txt");

Console.WriteLine();

List<List<int>> GetInput(string fileName)
{
  var input = new List<List<int>>();
 
  foreach (var line in File.ReadLines(fileName))
  {
    var row = (from c in line select int.Parse(c.ToString())).ToList();
    input.Add(row);
  }

  return input;
}
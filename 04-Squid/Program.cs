// See https://aka.ms/new-console-template for more information

var input = ReadFileData(@"input-small.txt");
Console.WriteLine("Hello, World!");


BingoGame ReadFileData(string fileName)
{
  var fileData = File.ReadLines(fileName);
  var bingoGame = new BingoGame();

  var enumerator = fileData.GetEnumerator();
  if (!enumerator.MoveNext())
    return bingoGame;

  var firstLine = enumerator.Current;

  bingoGame.drawnNumbers = from n in firstLine.Split(',') select int.Parse(n);


  while (enumerator.MoveNext())
  { 
  
  }

  return bingoGame;
}

struct BingoPosition
{
  public int Number { get; set; }
  public bool IsHit { get; set; } = false;
}

struct BingoGame
{
  public IEnumerable<int> drawnNumbers;
}



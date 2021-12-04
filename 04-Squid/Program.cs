// See https://aka.ms/new-console-template for more information

var input = ReadFileData(@"input-small.txt");
Console.WriteLine("Hello, World!");


BingoGame ReadFileData(string fileName)
{
  var fileData = File.ReadLines(fileName);

  var enumerator = fileData.GetEnumerator();
  if (!enumerator.MoveNext())
    throw new ApplicationException("no data available");

  var firstLine = enumerator.Current;
  var drawnNumbers = from n in firstLine.Split(',') select int.Parse(n);
  var bingoGame = new BingoGame(drawnNumbers);

  BingoCard? currentCard = null;
  while (enumerator.MoveNext())
  {
    string line = enumerator.Current;
    if (string.IsNullOrEmpty(line))
    {
      AddCardToGame(currentCard, bingoGame);
      currentCard = new BingoCard();
      continue;
    }

    System.Diagnostics.Debug.Assert(currentCard != null);

    var row = (from n in line.Split(' ', StringSplitOptions.RemoveEmptyEntries) select int.Parse(n)).ToList();
    currentCard.bingoPositions.Add(row);
  }
  AddCardToGame(currentCard, bingoGame);

  return bingoGame;
}

void AddCardToGame(BingoCard? currentCard, BingoGame bingoGame)
{
  if (currentCard != null)
  {
    System.Diagnostics.Debug.Assert(currentCard.bingoPositions.Count == 5);
    foreach (var row in currentCard.bingoPositions)
    {
      System.Diagnostics.Debug.Assert(row.Count == 5);
      foreach (var number in row)
        System.Diagnostics.Debug.Assert(number >= 0);
    }

    bingoGame.bingoCards.Add(currentCard);
  }
}

struct BingoPosition
{
  public int Number { get; set; } = -1;
  public bool IsHit { get; set; } = false;
}

class BingoCard
{
  public List<List<int>> bingoPositions = new List<List<int>>();
}

struct BingoGame
{
  public BingoGame(IEnumerable<int> drawnNumbers)
  {
    this.drawnNumbers = drawnNumbers;
  }

  public IEnumerable<int> drawnNumbers;
  public List<BingoCard> bingoCards = new List<BingoCard>();
}



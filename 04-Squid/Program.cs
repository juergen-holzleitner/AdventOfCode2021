// See https://aka.ms/new-console-template for more information

var bingoGame = ReadFileData(@"input.txt");
(var winningCard, var n) = GetWinningCard(bingoGame, true);

var sum = (
    from r in winningCard.bingoPositions
    from c in r
    where !c.IsHit
    select c.Number
  ).Sum();

Console.WriteLine(sum * n);


(BingoCard, int) GetWinningCard(BingoGame bingoGame, bool checkLast)
{ 
  foreach (var n in bingoGame.drawnNumbers)
  {
    foreach (var card in bingoGame.bingoCards)
    {
      DrawNumber(card, n);

      if (checkLast)
      {
        if (bingoGame.bingoCards.All(b => IsWinningCard(b)))
          return (card, n);
      }
      else
      {
        if (IsWinningCard(card))
          return (card, n);
      }
    }
  }

  throw new ApplicationException("no winning card found");
}

void DrawNumber(BingoCard card, int n)
{
  for (int row = 0; row < card.bingoPositions.Count; ++row)
    for (int col = 0; col < card.bingoPositions[row].Count; ++col)
      if (card.bingoPositions[row][col].Number == n)
        card.bingoPositions[row][col].IsHit = true;
}

bool IsWinningCard(BingoCard card)
{
  foreach (var row in card.bingoPositions)
  {
    if (row.All(p => p.IsHit))
      return true;
  }

  for (int n = 0; n < card.bingoPositions.First().Count; n++)
  {
    var column = from c in card.bingoPositions select c[n];
    if (column.All(p => p.IsHit))
      return true;
  }

  return false;
}

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

    var row = (from n in line.Split(' ', StringSplitOptions.RemoveEmptyEntries) 
               select new BingoPosition() { Number = int.Parse(n) }).ToList();
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
        System.Diagnostics.Debug.Assert(number.Number >= 0);
    }

    bingoGame.bingoCards.Add(currentCard);
  }
}

class BingoPosition
{
  public int Number { get; set; } = -1;
  public bool IsHit { get; set; } = false;
}

class BingoCard
{
  public List<List<BingoPosition>> bingoPositions = new();
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



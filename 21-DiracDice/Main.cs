using _21_DiracDice;
using System.Collections.Generic;
using System.Linq;

var pawns = new List<Pawn>() {
        new Pawn(1),
        new Pawn(5)
      };

var dice = new DeterministicDice();

int n = 0;
for (; ; ++n)
{
  GameBoard.ProcessMove(pawns[n % 2], dice);
  if (GameBoard.IsWinningScore(pawns[n % 2].Score))
    break;
}

var otherScore = pawns[(n + 1) % 2].Score;
int rolls = (n + 1) * 3;
int result = rolls * otherScore;
System.Console.WriteLine(result);





Pawn p1 = new Pawn(1);
Pawn p2 = new Pawn(5);

var universe = new Universe(p1, p2);

var activeUniverses = new Dictionary<Universe, long>()
    {
      {universe, 1 }
    };

var winCounts = new long[2] { 0, 0 };

while (activeUniverses.Any())
{
  var newUniverses = new Dictionary<Universe, long>();

  foreach (var u in activeUniverses)
  {
    int step = u.Key.Steps % 2;
    
    foreach (var d in QuantumDie.GenerateUniverses())
    {
      int newPos = GameBoard.IncrementPosBy(u.Key.Pawns[step].Position, d.Key);
      var newPawn = new Pawn(u.Key.Pawns[step]);
      newPawn.MoveTo(newPos);
      
      long numUniverses = u.Value * d.Value;

      if (newPawn.Score >= 21)
      {
        winCounts[step]+=numUniverses;
        continue;
      }

      var newList = new List<Pawn>(u.Key.Pawns);
      newList[step] = newPawn;

      var newU = new Universe(newList, u.Key.Steps + 1);
      if (newUniverses.ContainsKey(newU))
      {
        newUniverses[newU] += numUniverses;
      }
      else
      {
        newUniverses.Add(newU, numUniverses);
      }
    }
  }

  activeUniverses = newUniverses;
}

foreach (var wc in winCounts)
{
  System.Console.WriteLine(wc);
}

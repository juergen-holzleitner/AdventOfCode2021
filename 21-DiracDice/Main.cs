using _21_DiracDice;
using System.Collections.Generic;

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
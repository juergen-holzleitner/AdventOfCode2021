using System;

namespace _25_SeaCucumber
{
  internal class Processor
  {
    internal static char[,] MoveHorizontal(char[,] current)
    {
      var pos = new char[current.GetLength(0), current.GetLength(1)];
      for (int y = 0; y < current.GetLength(0); ++y)
        for (int x = 0; x < current.GetLength(1); ++x)
          pos[y,x] = '.';

      for (int y = 0; y < current.GetLength(0); ++y)
        for (int x = 0; x < current.GetLength(1); ++x)
        {
          if (current[y,x] == '>')
          {
            var rightPos = (x + 1) % current.GetLength(1);
            if (current[y, rightPos] == '.')
              pos[y, rightPos] = '>';
            else
              pos[y, x] = '>';
          }
        }
      return pos;
    }
  }
}
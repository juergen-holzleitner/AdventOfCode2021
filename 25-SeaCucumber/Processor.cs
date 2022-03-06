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
              pos[y, rightPos] = current[y, x];
            else
              pos[y, x] = current[y, x];
          }
          else if (current[y, x] == 'v')
            pos[y, x] = current[y, x];

        }
      return pos;
    }

    internal static char[,] MoveVertical(char[,] current)
    {
      var pos = new char[current.GetLength(0), current.GetLength(1)];
      for (int y = 0; y < current.GetLength(0); ++y)
        for (int x = 0; x < current.GetLength(1); ++x)
          pos[y, x] = '.';

      for (int x = 0; x < current.GetLength(1); ++x)
        for (int y = 0; y < current.GetLength(0); ++y)
        {
          if (current[y, x] == 'v')
          {
            var lowerPos = (y + 1) % current.GetLength(0);
            if (current[lowerPos, x] == '.')
              pos[lowerPos, x] = current[y, x];
            else
              pos[y, x] = current[y, x];
          }
          else if (current[y, x] == '>')
            pos[y, x] = current[y, x];
        }
      return pos;
    }

    internal static char[,] Move(char[,] initialArr)
    {
      var horizontal = MoveHorizontal(initialArr);
      return MoveVertical(horizontal);
    }
  }
}
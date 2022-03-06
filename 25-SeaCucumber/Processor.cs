using System;

namespace _25_SeaCucumber
{
  internal class Processor
  {
    internal static char[] MoveHorizontal(char[] current)
    {
      var pos = new char[current.Length];
      for (int i = 0; i < current.Length; i++)
        pos[i] = '.';

      for (int i = 0; i < current.Length; i++)
      {
        if (current[i] == '>')
        {
          var nextPos = (i + 1) % current.Length;
          if (current[nextPos] == '.')
            pos[nextPos] = '>';
          else
            pos[i] = '>';
        }
      }
      return pos;
    }
  }
}
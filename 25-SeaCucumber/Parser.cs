using System;
using System.Linq;

namespace _25_SeaCucumber
{
  internal class Parser
  {
    internal static char[,] Parse(string input)
    {
      var lines = input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l));
      var arr = new char[lines.Count(), lines.First().Length];

      int y = 0;
      foreach (var l in lines)
      {
        System.Diagnostics.Trace.Assert(l.Length == arr.GetLength(1));

        for (int x = 0; x < arr.GetLength(0); ++x)
        {
          var ch = l[x];
          System.Diagnostics.Trace.Assert(ch == '.' || ch == '>' || ch == 'v');
          arr[y, x] = ch;
        }

        ++y;
      }

      return arr;
    }
  }
}
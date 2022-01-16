using System;
using System.Collections.Generic;

namespace _21_DiracDice
{
  internal static class QuantumDie
  {
    static Dictionary<int, int> universes = new();

    static QuantumDie()
    {
      const int numSides = 3;
      for (int i = 0; i < numSides; ++i)
        for (int j = 0; j < numSides; ++j)
          for (int k = 0; k < numSides; ++k)
          {
            var sum = i + j + k + 3;
            if (!universes.ContainsKey(sum))
              universes.Add(sum, 1);
            else
              ++universes[sum];
          }
    }

    internal static Dictionary<int, int> GenerateUniverses()
    {
      return universes;
    }
  }
}
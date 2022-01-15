using System;

namespace _21_DiracDice
{
  internal class DeterministicDice
  {
    int currentValue = 100;

    public DeterministicDice()
    {
    }

    internal DeterministicDice(int val)
    {
      currentValue = val;
    }

    internal int GetNextValue()
    {
      ++currentValue;
      if (currentValue > 100)
        currentValue = 1;

      return currentValue;
    }

    internal int GetValueAfterRolls(int numRolls)
    {
      int val = 0;
      for (int i = 0; i < numRolls; i++)
      {
        val += GetNextValue();
      }
      return val;
    }
  }
}
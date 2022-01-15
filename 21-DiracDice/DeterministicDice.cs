using System;

namespace _21_DiracDice
{
  internal class DeterministicDice
  {
    int currentValue = 1000;

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
      if (currentValue > 1000)
        currentValue = 1;

      return currentValue;
    }
  }
}
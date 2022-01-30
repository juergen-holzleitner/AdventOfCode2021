using System;

namespace _23_Amphipod
{
  internal class Hallway
  {
    private readonly char?[] hallway;

    public Hallway(int numPositions)
    {
      hallway = new char?[numPositions];
    }

    private Hallway(char?[] other)
    {
      hallway = (char?[])other.Clone();
    }

    internal Hallway Clone()
    {
      return new Hallway(hallway);
    }

    public int NumPositions { get => hallway.Length; }

    internal void MoveIn(int position, char amphipod)
    {
      if (!CanMoveIn(position))
        throw new InvalidOperationException("Position is already occupied");

      hallway[position] = amphipod;
    }

    internal char? GetAmphipodAt(int position)
    {
      return hallway[position];
    }

    internal bool CanMoveIn(int position)
    {
      return hallway[position] is null;
    }

    internal bool CanMoveInFrom(int startPosition, int finalPosition)
    {
      int min = Math.Min(startPosition, finalPosition);
      int max = Math.Max(startPosition, finalPosition);
      for (int n = min; n <= max; n++)
        if (!CanMoveIn(n))
          return false;

      return true;
    }
  }
}
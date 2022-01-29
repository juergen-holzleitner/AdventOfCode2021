namespace _23_Amphipod
{
  internal class Hallway
  {
    public Hallway(int numPositions)
    {
      NumPositions = numPositions;
    }

    internal Hallway Clone()
    {
      return new Hallway(NumPositions);
    }

    public int NumPositions { get; internal set; }
  }
}
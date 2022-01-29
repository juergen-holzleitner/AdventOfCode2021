using System;

namespace _23_Amphipod
{
  internal class SideRoom
  {
    char[] amphipodsInTheRoom;

    public SideRoom(int hallwayPosition, char targetAmphipod, char[] initialAmphipods)
    {
      HallwayPosition = hallwayPosition;
      TargetAmphipod = targetAmphipod;
      amphipodsInTheRoom = initialAmphipods;
    }

    public static int NumPositions { get => 2; }
    public int HallwayPosition { get; internal set; }
    public char TargetAmphipod { get; internal set; }

    internal char GetAmphipodAt(int positionFromDoor)
    {
      return amphipodsInTheRoom[positionFromDoor];
    }
  }
}
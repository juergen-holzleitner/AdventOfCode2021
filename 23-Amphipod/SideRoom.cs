using System;
using System.Linq;

namespace _23_Amphipod
{
  internal class SideRoom
  {
    readonly char?[] amphipodsInTheRoom;

    public SideRoom(int hallwayPosition, char targetAmphipod, char?[] initialAmphipods)
    {
      HallwayPosition = hallwayPosition;
      TargetAmphipod = targetAmphipod;
      amphipodsInTheRoom = initialAmphipods.Cast<char?>().ToArray();
    }

    public int HallwayPosition { get; internal set; }
    public char TargetAmphipod { get; internal set; }

    internal char? GetAmphipodAt(int positionFromDoor)
    {
      return amphipodsInTheRoom[positionFromDoor];
    }

    internal bool IsFinal()
    {
      return amphipodsInTheRoom.All(a => a == TargetAmphipod);
    }

    internal bool CanMoveOut()
    {
      if (IsFinal())
        return false;

      return amphipodsInTheRoom.Any(a => a != null);
    }

    internal char GetMoveOutAmphipod()
    {
      if (!CanMoveOut())
        throw new InvalidOperationException("Can not move out");
      for (int i = 0; i < amphipodsInTheRoom.Length; ++i)
        if (amphipodsInTheRoom[i] is char amphipod)
        {
          return amphipod;
        }
      throw new InvalidOperationException("Unreachable state reached!");
    }

    internal int MoveOut()
    {
      if (!CanMoveOut())
        throw new InvalidOperationException("Nothing to move out from the side room");

      for (int i = 0; i < amphipodsInTheRoom.Length; ++i)
        if (amphipodsInTheRoom[i] is not null)
        {
          amphipodsInTheRoom[i] = null;
          return i + 1;
        }
      
      throw new InvalidOperationException("Unreachable state reached!");
    }

    internal SideRoom Clone()
    {
      return new SideRoom(HallwayPosition, TargetAmphipod, (char?[])amphipodsInTheRoom.Clone());
    }

    internal bool CanMoveIn(char amphipod)
    {
      if (amphipod != TargetAmphipod)
        return false;

      if (amphipodsInTheRoom.Any(a => a is not null && a != TargetAmphipod))
        return false;

      return amphipodsInTheRoom.Any(a => a is null);
    }

    internal int MoveIn(char amphipod)
    {
      if (!CanMoveIn(amphipod))
        throw new InvalidOperationException("Can not move into sideroom");

      for (int n = amphipodsInTheRoom.Length - 1; n >= 0; --n)
      {
        if (amphipodsInTheRoom[n] is null)
        {
          amphipodsInTheRoom[n] = amphipod;
          return n + 1;
        }
      }

      throw new InvalidOperationException("Can not move into sideroom");
    }

    public override bool Equals(object? obj)
    {
      if ((obj == null) || !GetType().Equals(obj.GetType()))
      {
        return false;
      }
      else
      {
        var other = (SideRoom)obj;
        return amphipodsInTheRoom.SequenceEqual(other.amphipodsInTheRoom);
      }
    }

    public override int GetHashCode()
    {
      int hash = 0;
      foreach (var x in amphipodsInTheRoom)
      {
        hash *= 5;
        hash += (x ?? 64) - 64;
      }
      return hash;
    }
  }
}
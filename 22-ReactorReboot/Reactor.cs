using System;
using System.Collections.Generic;

namespace _22_ReactorReboot
{
  internal class Reactor
  {
    const int limit = 50;

    List<InputReader.Block> enabledCubes = new();

    public Reactor()
    {
    }

    public bool[,,] State { get; set; } = new bool[2 * limit + 1, 2 * limit + 1, 2 * limit + 1];

    internal long GetNumCubesOn()
    {
      long num = 0;
      foreach (var i in State)
        if (i)
          ++num;

      foreach (var b in enabledCubes)
        num += GetBlockSize(b);

      return num;
    }

    internal void ProcessStep(InputReader.Input input)
    {
      for (long x = input.Block.A.X; x <= input.Block.B.X; ++x)
        for (long y = input.Block.A.Y; y <= input.Block.B.Y; ++y)
          for (long z = input.Block.A.Z; z <= input.Block.B.Z; ++z)
          {
            var X = x + limit;
            var Y = y + limit;
            var Z = z + limit;
            if (X < State.GetLength(0) && Y < State.GetLength(1) && Z < State.GetLength(2) && X >= 0 && Y >= 0 && Z >= 0)
              State[X, Y, Z] = input.On;
          }
    }

    internal static IEnumerable<InputReader.Input> LimitInputs(IEnumerable<InputReader.Input> inputs)
    {
      foreach (var input in inputs)
      {
        if (input.Block.B.X < -limit)
          continue;
        if (input.Block.B.Y < -limit)
          continue;
        if (input.Block.B.Z < -limit)
          continue;

        if (input.Block.A.X > limit)
          continue;
        if (input.Block.A.Y > limit)
          continue;
        if (input.Block.A.Z > limit)
          continue;

        var newA = new InputReader.Position(Math.Max(-limit, input.Block.A.X), Math.Max(-limit, input.Block.A.Y), Math.Max(-limit, input.Block.A.Z));
        var newB = new InputReader.Position(Math.Min(limit, input.Block.B.X), Math.Min(limit, input.Block.B.Y), Math.Min(limit, input.Block.B.Z));
        yield return new InputReader.Input(input.On, new InputReader.Block(newA, newB));
      }
    }

    internal void ProcessAddBlockOn(InputReader.Block block)
    {
      enabledCubes.Add(block);
    }

    internal static long GetBlockSize(InputReader.Block block)
    {
      var dX = block.B.X - block.A.X + 1;
      var dY = block.B.Y - block.A.Y + 1;
      var dZ = block.B.Z - block.A.Z + 1;

      return dX * dY * dZ;
    }

    internal static bool AreBlocksIntersect(InputReader.Block blockA, InputReader.Block blockB)
    {
      if (blockA.A.X > blockB.B.X)
        return false;
      if (blockA.A.Y > blockB.B.Y)
        return false;
      if (blockA.A.Z > blockB.B.Z)
        return false;

      if (blockA.B.X < blockB.A.X)
        return false;
      if (blockA.B.Y < blockB.A.Y)
        return false;
      if (blockA.B.Z < blockB.A.Z)
        return false;

      return true;
    }

    internal static bool BlockContainsOther(InputReader.Block block1, InputReader.Block block2)
    {
      if (!AreBlocksIntersect(block1, block2))
        return false;

      if (block2.A.X < block1.A.X)
        return false;
      if (block2.A.Y < block1.A.Y)
        return false;
      if (block2.A.Z < block1.A.Z)
        return false;

      if (block2.B.X > block1.B.X)
        return false;
      if (block2.B.Y > block1.B.Y)
        return false;
      if (block2.B.Z > block1.B.Z)
        return false;

      return true;
    }

    internal static IEnumerable<InputReader.Block> GetFragmentsToAdd(InputReader.Block existingBlock, InputReader.Block newBlock)
    {
      List<InputReader.Block> fragmentsToAdd = new();

      if (BlockContainsOther(existingBlock, newBlock))
        return fragmentsToAdd;

      fragmentsToAdd.Add(newBlock);
      
      if (!AreBlocksIntersect(existingBlock, newBlock))
        return fragmentsToAdd;

      // check X
      List<InputReader.Block> newFragments = new();
      foreach (var f in fragmentsToAdd)
      {
        if (existingBlock.B.X >= f.A.X && existingBlock.B.X < f.B.X)
        {
          var lower = f with { B = f.B with { X = existingBlock.B.X } };
          if (!BlockContainsOther(existingBlock, lower))
            newFragments.Add(lower);

          var upper = f with { A = f.A with { X = existingBlock.B.X + 1 } };
          if (!BlockContainsOther(existingBlock, upper))
            newFragments.Add(upper);
        }
        else
        {
          newFragments.Add(f);
        }
      }
      fragmentsToAdd = newFragments;
      newFragments = new();

      foreach (var f in fragmentsToAdd)
      {
        if (existingBlock.A.X > f.A.X && existingBlock.A.X <= f.B.X)
        {
          var lower = f with { B = f.B with { X = existingBlock.A.X - 1 } };
          if (!BlockContainsOther(existingBlock, lower))
            newFragments.Add(lower);

          var upper = f with { A = f.A with { X = existingBlock.A.X } };
          if (!BlockContainsOther(existingBlock, upper))
            newFragments.Add(upper);
        }
        else
        {
          newFragments.Add(f);
        }
      }

      fragmentsToAdd = newFragments;
      newFragments = new();

      return fragmentsToAdd;
    }
  }
}
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

    internal long GetNumCubesOn()
    {
      long num = 0;
      foreach (var b in enabledCubes)
        num += GetBlockSize(b);

      return num;
    }

    internal void ProcessStep(InputReader.Input input)
    {
      if (input.On)
        ProcessAddBlockOn(input.Block);
      else
        ProcessAddBlockOff(input.Block);
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
      var fragmentsToAdd = new List<InputReader.Block>()
      {
        block
      };

      foreach (var eb in enabledCubes)
      {
        var newFragments = new List<InputReader.Block>();
        foreach (var f in fragmentsToAdd)
        {
          newFragments.AddRange(GetFragmentsToAdd(eb, f));
        }

        fragmentsToAdd = newFragments;
      }
      
      enabledCubes.AddRange(fragmentsToAdd);
    }

    internal void ProcessAddBlockOff(InputReader.Block block)
    {
      var fragmentsToAdd = new List<InputReader.Block>();

      foreach (var eb in enabledCubes)
      {
        fragmentsToAdd.AddRange(GetFragmentsToAdd(block, eb));
      }

      enabledCubes = fragmentsToAdd;
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
      fragmentsToAdd = SplitX(existingBlock, fragmentsToAdd, existingBlock.B.X);
      fragmentsToAdd = SplitX(existingBlock, fragmentsToAdd, existingBlock.A.X - 1);

      // check Y
      fragmentsToAdd = SplitY(existingBlock, fragmentsToAdd, existingBlock.B.Y);
      fragmentsToAdd = SplitY(existingBlock, fragmentsToAdd, existingBlock.A.Y - 1);

      // check Z
      fragmentsToAdd = SplitZ(existingBlock, fragmentsToAdd, existingBlock.B.Z);
      fragmentsToAdd = SplitZ(existingBlock, fragmentsToAdd, existingBlock.A.Z - 1);

      return fragmentsToAdd;
    }

    private static List<InputReader.Block> SplitX(InputReader.Block existingBlock, List<InputReader.Block> fragmentsToAdd, int splitX)
    {
      List<InputReader.Block> newFragments = new();
      foreach (var f in fragmentsToAdd)
      {
        if (splitX >= f.A.X && splitX < f.B.X)
        {
          var lower = f with { B = f.B with { X = splitX } };
          if (!BlockContainsOther(existingBlock, lower))
            newFragments.Add(lower);

          var upper = f with { A = f.A with { X = splitX + 1 } };
          if (!BlockContainsOther(existingBlock, upper))
            newFragments.Add(upper);
        }
        else
        {
          newFragments.Add(f);
        }
      }

      return newFragments;
    }

    private static List<InputReader.Block> SplitY(InputReader.Block existingBlock, List<InputReader.Block> fragmentsToAdd, int splitY)
    {
      List<InputReader.Block> newFragments = new();
      foreach (var f in fragmentsToAdd)
      {
        if (splitY >= f.A.Y && splitY < f.B.Y)
        {
          var lower = f with { B = f.B with { Y = splitY } };
          if (!BlockContainsOther(existingBlock, lower))
            newFragments.Add(lower);

          var upper = f with { A = f.A with { Y = splitY + 1 } };
          if (!BlockContainsOther(existingBlock, upper))
            newFragments.Add(upper);
        }
        else
        {
          newFragments.Add(f);
        }
      }

      return newFragments;
    }

    private static List<InputReader.Block> SplitZ(InputReader.Block existingBlock, List<InputReader.Block> fragmentsToAdd, int splitZ)
    {
      List<InputReader.Block> newFragments = new();
      foreach (var f in fragmentsToAdd)
      {
        if (splitZ >= f.A.Z && splitZ < f.B.Z)
        {
          var lower = f with { B = f.B with { Z = splitZ } };
          if (!BlockContainsOther(existingBlock, lower))
            newFragments.Add(lower);

          var upper = f with { A = f.A with { Z = splitZ + 1 } };
          if (!BlockContainsOther(existingBlock, upper))
            newFragments.Add(upper);
        }
        else
        {
          newFragments.Add(f);
        }
      }

      return newFragments;
    }
  }
}
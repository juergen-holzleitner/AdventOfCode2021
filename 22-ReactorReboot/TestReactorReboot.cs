using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace _22_ReactorReboot
{
  [TestClass]
  public class TestReactorReboot
  {
    [TestMethod]
    [DataRow("on x=10..12,y=3..7,z=2..9", true, 10, 3, 2, 12, 7, 9)]
    [DataRow("off x=10..12,y=3..7,z=2..9", false, 10, 3, 2, 12, 7, 9)]
    [DataRow("on x=5..12,y=3..7,z=2..9", true, 5, 3, 2, 12, 7, 9)]
    public void Read_Start_Works(string line, bool on, int expectedStartX, int expectedStartY, int expectedStartZ, int expectedEndX, int expectedEndY, int expectedEndZ)
    {
      var input = InputReader.InterpretLine(line);
      Assert.AreEqual(input.On, on);
      Assert.AreEqual(input.Block.A.X, expectedStartX);
      Assert.AreEqual(input.Block.A.Y, expectedStartY);
      Assert.AreEqual(input.Block.A.Z, expectedStartZ);

      Assert.AreEqual(input.Block.B.X, expectedEndX);
      Assert.AreEqual(input.Block.B.Y, expectedEndY);
      Assert.AreEqual(input.Block.B.Z, expectedEndZ);
    }

    [TestMethod]
    public void ReactorHas_101_ElementsInEachDimension()
    {
      var reactor = new Reactor();
      Assert.AreEqual(101, reactor.State.GetLength(0));
      Assert.AreEqual(101, reactor.State.GetLength(1));
      Assert.AreEqual(101, reactor.State.GetLength(2));
    }

    [TestMethod]
    public void Reactor_Supports_OnCubes()
    {
      var reactor = new Reactor();
      var numCubesOn = reactor.GetNumCubesOn();
      Assert.AreEqual(0, numCubesOn);
    }

    [TestMethod]
    public void Reactor_NumberOn_IsCorrectAfterSampleStep()
    {
      var reactor = new Reactor();
      
      var input = InputReader.InterpretLine("on x=10..12,y=10..12,z=10..12");
      reactor.ProcessStep(input);
      
      input = InputReader.InterpretLine("on x=11..13,y=11..13,z=11..13");
      reactor.ProcessStep(input);

      input = InputReader.InterpretLine("off x=9..11,y=9..11,z=9..11");
      reactor.ProcessStep(input);

      var numCubesOn = reactor.GetNumCubesOn();
      Assert.AreEqual(38, numCubesOn);
    }

    [TestMethod]
    public void Reactor_IsCorrect_SmallInput()
    {
      var reactor = new Reactor();

      var allInput = InputReader.GetAllInputs(@"input-small.txt");
      var limitedInput = Reactor.LimitInputs(allInput);
      foreach (var input in limitedInput)
      {
        reactor.ProcessStep(input);
      }

      var numCubesOn = reactor.GetNumCubesOn();
      Assert.AreEqual(590784, numCubesOn);
    }

    [TestMethod]
    public void Reactor_IsCorrect_Part1Input()
    {
      var reactor = new Reactor();

      var allInput = InputReader.GetAllInputs(@"input.txt");
      var limitedInput = Reactor.LimitInputs(allInput);
      foreach (var input in limitedInput)
      {
        reactor.ProcessStep(input);
      }

      var numCubesOn = reactor.GetNumCubesOn();
      Assert.AreEqual(596598, numCubesOn);
    }

    [TestMethod]
    public void LimitInput_Works()
    {
      var input = InputReader.InterpretLine("on x=10..12,y=10..12,z=10..12");
      var input2 = InputReader.InterpretLine("on x=-100..-60,y=10..12,z=10..12");
      var input3 = InputReader.InterpretLine("on x=-30..12,y=51..51,z=10..12");
      var input4 = InputReader.InterpretLine("on x=-30..12,y=10..51,z=10..12");
      var inputList = new List<InputReader.Input>() { input, input2, input3, input4 };

      var limitedInput = Reactor.LimitInputs(inputList);

      var input4Limited = InputReader.InterpretLine("on x=-30..12,y=10..50,z=10..12");
      var expectedList = new List<InputReader.Input>() { input, input4Limited };

      CollectionAssert.AreEqual(expectedList, limitedInput.ToList());
    }

    [TestMethod]
    public void Get_BlockSize_Returns_CorrectValue()
    {
      var block = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 1, 2));
      var blockSize = Reactor.GetBlockSize(block);

      Assert.AreEqual(6, blockSize);
    }

    [TestMethod]
    public void EnableBlock_InReactor_Works()
    {
      Reactor reactor = new();
      var block = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 1, 2));
      reactor.ProcessAddBlockOn(block);

      Assert.AreEqual(6, reactor.GetNumCubesOn());
    }

    [TestMethod]
    public void EnableMultipleBlocks_InReactor_Works()
    {
      Reactor reactor = new();
      var block = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 1, 2));
      var block2 = new InputReader.Block(new InputReader.Position(10, 10, 10), new InputReader.Position(12, 10, 10));
      reactor.ProcessAddBlockOn(block);
      reactor.ProcessAddBlockOn(block2);

      Assert.AreEqual(9, reactor.GetNumCubesOn());
    }

    [TestMethod]
    public void NonInteractingBlocksAreRecognized()
    {
      var block1 = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 0, 0));
      var block2 = new InputReader.Block(new InputReader.Position(1, 1, 1), new InputReader.Position(1, 1, 1));
      bool intersect = Reactor.AreBlocksIntersect(block1, block2);
      Assert.IsFalse(intersect);
    }

    [TestMethod]
    public void InteractingBlocksAreRecognized()
    {
      var block1 = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 0, 0));
      var block2 = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 0, 0));
      bool intersect = Reactor.AreBlocksIntersect(block1, block2);
      Assert.IsTrue(intersect);
    }

    [TestMethod]
    public void BlockContainsOtherBlock_ReturnsFalse()
    {
      var block1 = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 0, 0));
      var block2 = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 0, 1));
      bool contains = Reactor.BlockContainsOther(block1, block2);
      Assert.IsFalse(contains);
    }

    [TestMethod]
    public void BlockContainsOtherBlock_ReturnsTrue()
    {
      var block1 = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 0, 1));
      var block2 = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 0, 0));
      bool contains = Reactor.BlockContainsOther(block1, block2);
      Assert.IsTrue(contains);
    }

    [TestMethod]
    public void FragementsToAdd_WithoutIntersection_ReturnsAll()
    {
      var existingBlock = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(0, 0, 0));
      var newBlock = new InputReader.Block(new InputReader.Position(1, 1, 1), new InputReader.Position(1, 1, 1));

      var fragementsToAdd = Reactor.GetFragmentsToAdd(existingBlock, newBlock);
      CollectionAssert.AreEqual(new List<InputReader.Block>() { newBlock }, fragementsToAdd.ToList());
    }

    [TestMethod]
    public void FragementsToAdd_WhenContained_ReturnsNothing()
    {
      var existingBlock = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(1, 2, 10));
      var newBlock = new InputReader.Block(new InputReader.Position(1, 1, 1), new InputReader.Position(1, 1, 1));

      var fragementsToAdd = Reactor.GetFragmentsToAdd(existingBlock, newBlock);
      Assert.IsFalse(fragementsToAdd.Any());
    }

    [TestMethod]
    public void FragementsToAdd_WhenHalfUpper_ReturnsCorrect()
    {
      var existingBlock = new InputReader.Block(new InputReader.Position(-1, 0, 0), new InputReader.Position(1, 2, 0));
      var newBlock = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(3, 4, 0));

      var fragementsToAdd = Reactor.GetFragmentsToAdd(existingBlock, newBlock);
      
      var expected = new List<InputReader.Block>() 
      { 
        new InputReader.Block(new InputReader.Position(0, 3, 0), new InputReader.Position(1, 4, 0)),
        new InputReader.Block(new InputReader.Position(2, 0, 0), new InputReader.Position(3, 2, 0)),
        new InputReader.Block(new InputReader.Position(2, 3, 0), new InputReader.Position(3, 4, 0)),
      };

      CollectionAssert.AreEqual(expected, fragementsToAdd.ToList());
    }

    [TestMethod]
    public void FragementsToAdd_WhenHalfXRight_ReturnsCorrect()
    {
      var existingBlock = new InputReader.Block(new InputReader.Position(0, 0, 0), new InputReader.Position(1, 0, 0));
      var newBlock = new InputReader.Block(new InputReader.Position(-1, 0, -1), new InputReader.Position(0, 0, 1));

      var fragementsToAdd = Reactor.GetFragmentsToAdd(existingBlock, newBlock);

      var expected = new List<InputReader.Block>()
      {
        new InputReader.Block(new InputReader.Position(-1, 0, -1), new InputReader.Position(-1, 0, -1)),
        new InputReader.Block(new InputReader.Position(-1, 0,  0), new InputReader.Position(-1, 0,  0)),
        new InputReader.Block(new InputReader.Position(-1, 0,  1), new InputReader.Position(-1, 0,  1)),
        new InputReader.Block(new InputReader.Position( 0, 0, -1), new InputReader.Position( 0, 0, -1)),
        new InputReader.Block(new InputReader.Position( 0, 0,  1), new InputReader.Position( 0, 0,  1)),
      };

      CollectionAssert.AreEqual(expected, fragementsToAdd.ToList());
    }

    [TestMethod]
    public void FragementsToAdd_WhenBothX_ReturnsCorrect()
    {
      var existingBlock = new InputReader.Block(new InputReader.Position(-1, 0, 0), new InputReader.Position(1, 0, 0));
      var newBlock = new InputReader.Block(new InputReader.Position(-3, 0, 0), new InputReader.Position(3, 0, 0));

      var fragementsToAdd = Reactor.GetFragmentsToAdd(existingBlock, newBlock);

      var expected = new List<InputReader.Block>() 
      {
        new InputReader.Block(new InputReader.Position(-3, 0, 0), new InputReader.Position(-2, 0, 0)),
        new InputReader.Block(new InputReader.Position(2, 0, 0), new InputReader.Position(3, 0, 0)) ,
      };

      CollectionAssert.AreEqual(expected, fragementsToAdd.ToList());
    }
  }
}
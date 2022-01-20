using Microsoft.VisualStudio.TestTools.UnitTesting;
using static _22_ReactorReboot.InputReader;

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
      var input = InterpretLine(line);
      Assert.AreEqual(input.On, on);
      Assert.AreEqual(input.Start.X, expectedStartX);
      Assert.AreEqual(input.Start.Y, expectedStartY);
      Assert.AreEqual(input.Start.Z, expectedStartZ);

      Assert.AreEqual(input.End.X, expectedEndX);
      Assert.AreEqual(input.End.Y, expectedEndY);
      Assert.AreEqual(input.End.Z, expectedEndZ);
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
    public void Reactor_StartsWithAllOff()
    {
      var reactor = new Reactor();
      for (int x = 0; x < reactor.State.GetLength(0); ++x)
        for (int y = 0; y < reactor.State.GetLength(1); ++y)
          for (int z = 0; z < reactor.State.GetLength(2); ++z)
            Assert.IsFalse(reactor.State[x, y, z]);
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
      
      var input = InterpretLine("on x=10..12,y=10..12,z=10..12");
      reactor.ProcessStep(input);
      
      input = InterpretLine("on x=11..13,y=11..13,z=11..13");
      reactor.ProcessStep(input);

      input = InterpretLine("off x=9..11,y=9..11,z=9..11");
      reactor.ProcessStep(input);

      var numCubesOn = reactor.GetNumCubesOn();
      Assert.AreEqual(38, numCubesOn);
    }

    [TestMethod]
    public void Reactor_IsCorrect_SmallInput()
    {
      var reactor = new Reactor();

      foreach (var line in ReadAllInputLines(@"input-small.txt"))
      {
        var input = InterpretLine(line);
        reactor.ProcessStep(input);
      }

      var numCubesOn = reactor.GetNumCubesOn();
      Assert.AreEqual(590784, numCubesOn);
    }
  }
}
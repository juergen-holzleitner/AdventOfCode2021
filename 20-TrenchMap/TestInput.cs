using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _20_TrenchMap
{
  [TestClass]
  public class TestInput
  {
    private const string smallInputFilename = @"input-small.txt";

    [TestMethod]
    public void TestReadEnhancementAlgorithm()
    {
      var input = ReadInput(smallInputFilename);
      Assert.AreEqual(512, input.EnhancmentAlgorithm.Length);
    }

    [TestMethod]
    public void TestReadInputImage()
    {
      var input = ReadInput(smallInputFilename);
      Assert.AreEqual(5, input.InputImage.Count);
    }

    [TestMethod]
    public void TestInputIsRectangle()
    {
      var input = ReadInput(smallInputFilename);
      int first = input.InputImage.First().Count();
      foreach (var i in input.InputImage)
        Assert.AreEqual(first, i.Count());
    }

    private static Input ReadInput(string fileName)
    {
      var lines = File.ReadLines(fileName).GetEnumerator();
      lines.MoveNext();
      var enhAlg = lines.Current;
      lines.MoveNext();
      if (!string.IsNullOrEmpty(lines.Current))
        throw new ApplicationException("Expected empty line");

      List<List<char>> inputImage = new();
      while (lines.MoveNext())
      {
        var l = lines.Current.ToList();
        inputImage.Add(l);
      }

      return new Input(enhAlg, inputImage);
    }

    readonly record struct Input(string EnhancmentAlgorithm, List<List<char>> InputImage);
  }
}
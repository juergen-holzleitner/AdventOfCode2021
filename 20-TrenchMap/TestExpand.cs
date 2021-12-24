﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace _20_TrenchMap
{
  [TestClass]
  public class TestExpand
  {
    [TestMethod]
    public void TestExpandLine()
    {
      var line = "#..#.".ToList();
      var expandedLine = ExpandLineBy(line, 2);
      CollectionAssert.AreEqual("..#..#...".ToList(), expandedLine);
    }

    [TestMethod]
    public void TextExpandImage()
    {
      List<List<char>> image = new()
      {
        new List<char> {'.'}
      };

      var expandedImage = ExpandImageBy(image, 1);

      List<List<char>> expandedTestImage = new()
      {
        new List<char> { '.', '.', '.' },
        new List<char> { '.', '.', '.' },
        new List<char> { '.', '.', '.' },
      };

      Assert.AreEqual(expandedTestImage.Count, expandedImage.Count);
      for (int i = 0; i < image.Count; i++)
        CollectionAssert.AreEqual(expandedTestImage[i], expandedImage[i]);
    }

    private static List<List<char>> ExpandImageBy(List<List<char>> image, int size)
    {
      var initialLength = image.First().Count;
      var expanded = new List<List<char>>();
      var expandLine = new List<char>();
      for (int i = 0; i < initialLength + 2 * size; i++)
        expandLine.Add('.');

      for (int i = 0; i < size; i++)
        expanded.Add(expandLine.ToList());

      foreach (var l in image)
        expanded.Add(ExpandLineBy(l, size));

      for (int i = 0; i < size; i++)
        expanded.Add(expandLine.ToList());

      return expanded;
    }

    private static List<char> ExpandLineBy(List<char> line, int size)
    {
      var newLine = new List<char>();

      for (int i = 0; i < size; ++i)
        newLine.Add('.');

      newLine.AddRange(line);

      for (int i = 0; i < size; ++i)
        newLine.Add('.');

      return newLine;
    }
  }
}

﻿// See https://aka.ms/new-console-template for more information

var intput = File.ReadLines(@"input.txt");

int[]? individualBits = null;
int numTotal = 0;

foreach (var line in intput)
{
  if (individualBits == null)
  {
    if (line.Length<=0)
      continue;

    individualBits = new int[line.Length];
  }

  bool? isLineValid = null;
  for (int i =0; i < line.Length; i++)
  {
    if (!isLineValid.HasValue)
      isLineValid = true;

    if (line[i] == '1')
      ++individualBits[i];
    else if (line[i] == '0')
    { 
    }
    else
    {
      System.Diagnostics.Trace.Assert(false, $"invalid character {line[i]}");
      isLineValid = false;
    }
  }

  if (isLineValid == true)
    ++numTotal;

}

int regular = 0, inverted = 0;
for (int i = 0; i < individualBits.Length; ++i)
{
  regular <<= 1;
  inverted <<= 1;

  if (individualBits[i] * 2 > numTotal)
    regular |= 1;
  else
    inverted |= 1;
}

Console.WriteLine(regular * inverted);

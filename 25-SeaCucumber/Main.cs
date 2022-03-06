using _25_SeaCucumber;
using System.IO;

Step1();

void Step1()
{
  var initialString = File.ReadAllText(@"input.txt");
  var currentArr = Parser.Parse(initialString);

  bool wasMoving;
  int steps = 1;
  do
  {
    (currentArr, wasMoving) = Processor.Move(currentArr);
    if (wasMoving)
      ++steps;

    System.Console.Write(steps + "\r");
  } while (wasMoving);

  System.Console.WriteLine($"No movement after {steps} steps");
}
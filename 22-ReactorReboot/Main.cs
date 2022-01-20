using _22_ReactorReboot;

var reactor = new Reactor();

foreach (var line in InputReader.ReadAllInputLines(@"input.txt"))
{
  var input = InputReader.InterpretLine(line);
  reactor.ProcessStep(input);
}

var numCubesOn = reactor.GetNumCubesOn();
System.Console.WriteLine(numCubesOn);
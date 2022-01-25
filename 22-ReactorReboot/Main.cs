using _22_ReactorReboot;

var reactor = new Reactor();

var allInput = InputReader.GetAllInputs(@"input.txt");
foreach (var input in allInput)
{
  reactor.ProcessStep(input);
}

var numCubesOn = reactor.GetNumCubesOn();
System.Console.WriteLine(numCubesOn);

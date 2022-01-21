using _22_ReactorReboot;

var reactor = new Reactor();

var allInput = InputReader.GetAllInputs(@"input.txt");
var limitedInput = Reactor.LimitInputs(allInput);
foreach (var input in limitedInput)
{
  reactor.ProcessStep(input);
}

var numCubesOn = reactor.GetNumCubesOn();
System.Console.WriteLine(numCubesOn);
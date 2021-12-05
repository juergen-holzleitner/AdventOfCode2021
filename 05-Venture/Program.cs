// See https://aka.ms/new-console-template for more information

var data = ReadInput(@"input-small.txt");

Input ReadInput(string fileName)
{
  var data = File.ReadLines(fileName);
  var input = new Input();
  foreach (var line in data)
  {
    Console.WriteLine(line);
  }

  return input;
}

class Input
{

}


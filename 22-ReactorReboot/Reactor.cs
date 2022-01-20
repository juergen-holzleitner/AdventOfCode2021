namespace _22_ReactorReboot
{
  internal class Reactor
  {
    public Reactor()
    {
    }

    public bool[,,] State { get; set; } = new bool[101,101,101];
  }
}
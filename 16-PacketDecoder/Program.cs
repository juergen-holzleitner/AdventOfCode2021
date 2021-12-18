var input = ReadInput(@"input-small.txt");
foreach (var item in input)
{
  var packet = item.Decode();
  if (packet is Literal literal)
  {
    Console.WriteLine($"V: {literal.Version} Literal: {literal.Value}");
  }
  else
  { 
    Console.WriteLine($"V: {packet.Version} T: {packet.Type}");
  }
}

IEnumerable<Decoder> ReadInput(string fileName)
{
  var data = File.ReadLines(fileName);
  foreach (var line in data)
  {
    Decoder decoder = new Decoder();
    foreach (var c in line)
    {
      var digit = byte.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber);
      decoder.AddHexDigit(digit);

    }
    
    yield return decoder;
  }
}


class Decoder
{
  public void AddHexDigit(byte digit)
  {
    System.Diagnostics.Debug.Assert(digit <= 15);

    for (int i = 3; i >= 0; --i)
    {

      bool bit = (digit & (1 << i)) != 0;
      bits.Enqueue(bit);
    }
  }

  public Packet Decode()
  {
    var version = (byte)DecodeBits(3);
    var type = (byte)DecodeBits(3);
    if (type == 4)
    {
      var literal = DecodeLiteral();
      return new Literal(version, type, literal);
    }
    else
    {
      return new Packet(version, type);
    }
  }

  int DecodeBits(int n)
  {
    System.Diagnostics.Debug.Assert(n >= 0 && n < bits.Count);
    int val = 0;
    for (int i = 0; i < n; ++i)
    {
      val <<= 1;
      var bit = bits.Dequeue();
      if (bit)
        val |= 1;
    }
    return val;
  }

  ulong DecodeLiteral()
  {
    ulong val = 0;
    var maxDigits = sizeof(ulong) * 2;
    int numDigits = 0;
    bool continueReading;
    do
    {
      continueReading = bits.Dequeue();
      val <<= 4;
      var digit = (ulong)DecodeBits(4);
      val |= digit;
      
      ++numDigits;

      System.Diagnostics.Debug.Assert(numDigits <= maxDigits );
    } while (continueReading);

    return val;
  }
  private Queue<bool> bits = new();
}

record Packet(byte Version, byte Type);

record Literal(byte Version, byte Type, ulong Value) : Packet(Version, Type);

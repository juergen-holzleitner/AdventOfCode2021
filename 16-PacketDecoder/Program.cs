var input = ReadInput(@"input.txt");
foreach (var item in input)
{
  var packet = item.Decode();
  int versionSum = PrintPacket(packet, 0);
  Console.WriteLine("Version Sum: " + versionSum);
}

int PrintPacket(Packet packet, int level)
{
  for (int i = 0; i < level; ++i)
    Console.Write('\t');
  if (packet is Literal literal)
  {
    Console.WriteLine($"V: {literal.Version} Literal: {literal.Value}");
    return literal.Version;
  }
  else if (packet is Operator op)
  {
    Console.WriteLine($"V: {op.Version} T: {op.Type}");
    int version = op.Version;
    foreach (var p in op.Packets)
    {
      version += PrintPacket(p, level + 1);
    }
    return version;
  }
  else
  {
    System.Diagnostics.Debug.Assert(false);
    Console.WriteLine($"V: {packet.Version} T: {packet.Type}");
    return 0;
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
      List<Packet> packets = new();
      
      var lengthType = bits.Dequeue();
      if (lengthType)
      {
        int numPackets = DecodeBits(11);
        for (int n = 0; n < numPackets; ++n)
        {
          var packet = Decode();
          packets.Add(packet);
        }
      }
      else
      {
        var numBits = DecodeBits(15);
        Decoder subDecoder = new Decoder();
        for (int n = 0; n < numBits; ++n)
        {
          subDecoder.bits.Enqueue(bits.Dequeue());
        }
        while (subDecoder.bits.Any())
        {
          var packet = subDecoder.Decode();
          packets.Add(packet);
        }
      }
      
      return new Operator(version, type, packets);
    }
  }

  int DecodeBits(byte numBits)
  {
    System.Diagnostics.Debug.Assert(numBits <= bits.Count);
    int val = 0;
    for (int i = 0; i < numBits; ++i)
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
record Operator(byte Version, byte Type, List<Packet> Packets) : Packet(Version, Type);

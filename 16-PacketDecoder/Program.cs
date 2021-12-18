var input = ReadInput(@"input.txt");
foreach (var (line, item) in input)
{
  var packet = item.Decode();
  Console.WriteLine(line);
  PrintPacket(packet, 0);
  Console.WriteLine();
}

void PrintPacket(Packet packet, int level)
{
  for (int i = 0; i < level; ++i)
    Console.Write('\t');

  if (packet is Literal literal)
  {
    Console.WriteLine($"V: {literal.Version} T: {GetPackageTypeString(literal.Type)}");
  }
  else if (packet is Operator op)
  {
    Console.WriteLine($"V: {op.Version} T: {GetPackageTypeString(op.Type)}");
    foreach (var p in op.Packets)
    {
      PrintPacket(p, level + 1);
    }
  }
  else
  {
    System.Diagnostics.Debug.Assert(false);
    Console.WriteLine($"V: {packet.Version} T: {GetPackageTypeString(packet.Type)}");
  }

  for (int i = 0; i < level; ++i)
    Console.Write('\t');
  Console.WriteLine("-> " + GetPacketValue(packet));
}

long GetPacketValue(Packet packet)
{
  if (packet is Literal literal)
  {
    return literal.Value;
  }
  else if (packet is Operator op)
  {
    long value = 0;
    switch (op.Type)
    {
      case 0:
        value = op.Packets.Sum(x => GetPacketValue(x));
        break;
      case 1:
        value = op.Packets.Aggregate(1, (long acc, Packet x) => acc * GetPacketValue(x));
        break;
      case 2:
        value = op.Packets.Min(x => GetPacketValue(x));
        break;
      case 3:
        value = op.Packets.Max(x => GetPacketValue(x));
        break;
      case 5:
        System.Diagnostics.Debug.Assert(op.Packets.Count == 2);
        value = GetPacketValue(op.Packets[0]) > GetPacketValue(op.Packets[1]) ? 1 : 0;
        break;
      case 6:
        System.Diagnostics.Debug.Assert(op.Packets.Count == 2);
        value = GetPacketValue(op.Packets[0]) < GetPacketValue(op.Packets[1]) ? 1 : 0;
        break;
      case 7:
        System.Diagnostics.Debug.Assert(op.Packets.Count == 2);
        value = GetPacketValue(op.Packets[0]) == GetPacketValue(op.Packets[1]) ? 1 : 0;
        break;
      default:
        System.Diagnostics.Debug.Assert(false);
        break;
    }
    
    return value;
  }
  else
  {
    System.Diagnostics.Debug.Assert(false);
    return 0;
  }
}

string GetPackageTypeString(byte type)
{
  switch (type)
  {
    case 0: return "sum";
    case 1: return "product";
    case 2: return "minimum";
    case 3: return "maximum";
    case 4: return "literal";
    case 5: return "greater than";
    case 6: return "less than";
    case 7: return "equal";
    default:
      System.Diagnostics.Debug.Assert(false);
      return "<unknown>";
  }
}

IEnumerable<(string, Decoder)> ReadInput(string fileName)
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
    
    yield return (line, decoder);
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

  long DecodeLiteral()
  {
    long val = 0;
    var maxDigits = sizeof(long) * 2;
    int numDigits = 0;
    bool continueReading;
    do
    {
      continueReading = bits.Dequeue();
      val <<= 4;
      var digit = (long)DecodeBits(4);
      val |= digit;
      
      ++numDigits;

      System.Diagnostics.Debug.Assert(numDigits <= maxDigits );
    } while (continueReading);

    return val;
  }
  private Queue<bool> bits = new();
}

record Packet(byte Version, byte Type);

record Literal(byte Version, byte Type, long Value) : Packet(Version, Type);
record Operator(byte Version, byte Type, List<Packet> Packets) : Packet(Version, Type);

TestAdd("[1,2]", "[[3,4],5]", "[[1,2],[[3,4],5]]");
TestReduceExplode("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]");
TestReduceExplode("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]");
TestReduceExplode("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]");
TestReduceExplode("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]");
TestReduceExplode("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]");
TestReduceSplit("[[[[0,7],4],[15,[0,13]]],[1,1]]", "[[[[0,7],4],[[7,8],[0,13]]],[1,1]]");
TestReduceSplit("[[[[0,7],4],[[7,8],[0,13]]],[1,1]]", "[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]");
TestReduce("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]");
TestAdd("[[[[4,3],4],4],[7,[[8,4],9]]]", "[1,1]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]");
TestAddAll("[[[[1,1],[2,2]],[3,3]],[4,4]]", "[1,1]", "[2,2]", "[3,3]", "[4,4]");
TestAddAll("[[[[3,0],[5,3]],[4,4]],[5,5]]", "[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]");
TestAddAll("[[[[5,0],[7,4]],[5,5]],[6,6]]", "[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]", "[6,6]");
TestAdd("[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]", "[7,[5,[[3,8],[1,4]]]]", "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]");
TestAdd("[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]", "[[2,[2,2]],[8,[8,1]]]", "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]");
TestAdd("[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]", "[2,9]", "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]");
TestAdd("[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]", "[1,[[[9,3],9],[[9,0],[0,7]]]]", "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]");
TestAdd("[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]", "[[[5,[7,4]],7],1]", "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]");
TestAdd("[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]", "[[[[4,2],2],6],[8,7]]", "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]");
TestReduceSplit("[[[[8,7],[7,7]],[[8,7],[7,0]]],[[[13,0],12],[8,7]]]", "[[[[8,7],[7,7]],[[8,7],[7,0]]],[[[[6,7],0],12],[8,7]]]");
TestMagnitude("[9,1]", 29);
TestMagnitude("[1,9]", 21);
TestMagnitude("[[9,1],[1,9]]", 129);
TestMagnitude("[[1,2],[[3,4],5]]", 143);
TestMagnitude("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384);
TestMagnitude("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445);
TestMagnitude("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791);
TestMagnitude("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137);
TestMagnitude("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488);

var input = GetInput(@"input.txt");
var result = AddAll(input.ToArray());
Console.WriteLine(GetNumberString(result));
Console.WriteLine($"Magnitude: {GetMagnitude(result)}");

void TestMagnitude(string input, long result)
{
  var n = ParseNumber(input);
  var m = GetMagnitude(n);
  System.Diagnostics.Debug.Assert(m == result);
}

long GetMagnitude(Number number)
{
  if (number.Num != null)
  {
    System.Diagnostics.Debug.Assert(number.Left == null);
    System.Diagnostics.Debug.Assert(number.Right == null);
    return number.Num.Value;
  }

  System.Diagnostics.Debug.Assert(number.Left != null);
  System.Diagnostics.Debug.Assert(number.Right != null);

  return 3 * GetMagnitude(number.Left) + 2 * GetMagnitude(number.Right);
}

void TestAddAll(string result, params string[] input)
{
  var sum = AddAll(input);
  var n = GetNumberString(sum);
  System.Diagnostics.Debug.Assert(n == result);
}

Number AddAll(params string[] input)
{
  System.Diagnostics.Debug.Assert(input.Any());

  var res = ParseNumber(input[0]);

  for (int n = 1; n < input.Length; ++n)
  {
    var num = ParseNumber(input[n]);
    res = Add(res, num);
  }

  return res;
}

void TestAdd(string left, string right, string result)
{
  var l = ParseNumber(left);
  var r = ParseNumber(right);
  var res = Add(l, r);
  var x = GetNumberString(res);
  System.Diagnostics.Debug.Assert(x == result);
}

void TestReduce(string x, string reduced)
{
  var l = ParseNumber(x);
  Reduce(l);
  var lr = GetNumberString(l);
  System.Diagnostics.Debug.Assert(lr == reduced);
}

void TestReduceExplode(string x, string reduced)
{
  var l = ParseNumber(x);
  ReduceExplode(l, 0);
  var lr = GetNumberString(l);
  System.Diagnostics.Debug.Assert(lr == reduced);
}

void TestReduceSplit(string x, string reduced)
{
  var l = ParseNumber(x);
  ReduceSplit(l);
  var lr = GetNumberString(l);
  System.Diagnostics.Debug.Assert(lr == reduced);
}

Number Add(Number left, Number right)
{
  var n  = new Number(left, right);
  Reduce(n);
  return n;
}

void Reduce(Number number)
{
  while (true)
  {
    var (b, _, _) = ReduceExplode(number, 0);
    if (b)
      continue;

    if (ReduceSplit(number))
      continue;

    break;
  }
}

bool ReduceSplit(Number number)
{
  if (number.Num != null)
  {
    System.Diagnostics.Debug.Assert(number.Num < 10);
    return false;
  }

  if (number.Left != null && number.Left.Num != null && number.Left.Num >= 10)
  {
    System.Diagnostics.Debug.Assert(number.Left.Left == null);
    System.Diagnostics.Debug.Assert(number.Left.Right == null);
    var l = new Number(number.Left.Num.Value / 2);
    var r = new Number((number.Left.Num.Value + 1) / 2);
    number.Left = new Number(l, r);
    return true;
  }
  if (number.Left != null && ReduceSplit(number.Left))
    return true;

  if (number.Right != null && number.Right.Num != null && number.Right.Num >= 10)
  {
    System.Diagnostics.Debug.Assert(number.Right.Left == null);
    System.Diagnostics.Debug.Assert(number.Right.Right == null);
    var l = new Number(number.Right.Num.Value / 2);
    var r = new Number((number.Right.Num.Value + 1) / 2);
    number.Right = new Number(l, r);
    return true;
  }

  if (number.Right != null && ReduceSplit(number.Right))
    return true;

  return false;
}

(bool, int?, int?) ReduceExplode(Number number, int level)
{
  if (number.Left != null)
  {
    System.Diagnostics.Debug.Assert(number.Right != null);

    if (level >= 4 && number.Left.Num != null)
    {
      System.Diagnostics.Debug.Assert(number.Right.Num != null);

      int left = number.Left.Num.Value;
      int right = number.Right.Num.Value;

      number.Left = null;
      number.Right = null;
      number.Num = 0;
      return (true, left, right);
    }

    {
      var (b, l, r) = ReduceExplode(number.Left, level + 1);
      if (b)
      {
        if (r.HasValue)
        {
          if (AddRight(number.Right, r.Value))
            r = null;
        }
        return (b, l, r);
      }
    }
    {
      var (b, l, r) = ReduceExplode(number.Right, level + 1);
      if (b)
      {
        if (l.HasValue)
        {
          if (AddLeft(number.Left, l.Value))
            l = null;
        }
        return (b, l, r);
      }
    }
  }

  return (false, null, null);
}

bool AddRight(Number number, int val)
{
  if (number.Num != null)
  {
    System.Diagnostics.Debug.Assert(number.Left == null);
    System.Diagnostics.Debug.Assert(number.Right == null);
    number.Num += val;
    return true;
  }

  if (number.Left != null && AddRight(number.Left, val))
    return true;
  if (number.Right!= null && AddRight(number.Right, val))
    return true;

  return false;
}

bool AddLeft(Number number, int val)
{
  if (number.Num != null)
  {
    System.Diagnostics.Debug.Assert(number.Left == null);
    System.Diagnostics.Debug.Assert(number.Right == null);
    number.Num += val;
    return true;
  }

  if (number.Right != null && AddLeft(number.Right, val))
    return true;
  if (number.Left != null && AddLeft(number.Left, val))
    return true;

  return false;
}

string GetNumberString(Number number)
{
  if (number.Num.HasValue)
  {
    System.Diagnostics.Debug.Assert(number.Left == null);
    System.Diagnostics.Debug.Assert(number.Right == null);
    return number.Num.Value.ToString();
  }
  System.Diagnostics.Debug.Assert(number.Left != null);
  System.Diagnostics.Debug.Assert(number.Right != null);
  return $"[{GetNumberString(number.Left)},{GetNumberString(number.Right)}]";
}

IEnumerable<string> GetInput(string fileName)
{
  return File.ReadLines(fileName);
}

Number ParseNumber(string number)
{
  Queue<char> chars = new(number);
  var n = ParseNumberFromQueue(chars);
  // Reduce(n);
  return n;
}

Number ParseNumberFromQueue(Queue<char> chars)
{
  while (chars.Any())
  {
    if (char.IsDigit(chars.Peek()))
    {
      var num = ParseDigit(chars);
      return new Number(num);
    }
    else if (chars.Peek() == '[')
    {
      System.Diagnostics.Debug.Assert(chars.Dequeue() == '[');
      var left = ParseNumberFromQueue(chars);
      System.Diagnostics.Debug.Assert(chars.Dequeue() == ',');
      var right = ParseNumberFromQueue(chars);
      System.Diagnostics.Debug.Assert(chars.Dequeue() == ']');
      return new Number(left, right);
    }
    else
    {
      System.Diagnostics.Debug.Assert(false);
      continue;
    }
  }

  throw new ApplicationException();
}

int ParseDigit(Queue<char> chars)
{
  int num = 0;
  while (chars.Any() && char.IsDigit(chars.Peek()))
  {
    num *= 10;
    num += int.Parse(chars.Dequeue().ToString());
  }
  return num;
}

class Number
{
  public Number(int num)
  {
    Num = num;
    Left = null;
    Right = null;
  }
  public Number(Number left, Number right)
  {
    Left = left;
    Right = right;
    Num = null;
  }
  public int? Num { get; set; }
  public Number? Left { get; set; }
  public Number? Right { get; set; }
}

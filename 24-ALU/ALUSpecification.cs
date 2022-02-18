namespace _24_ALU
{
  public enum Register { w, x, y, z }

  public enum Operation { inp, add, mul, div, mod, eql };

  public interface IOperand { }

  public record RegisterOperand(Register Register) : IOperand;

  public record NumberOperand(int Number) : IOperand;

  public record InputOperand(int Index) : IOperand;

  public record Term(Operation Operation, IOperand Left, IOperand Right) : IOperand;

  public record Instruction(Operation Operation, Register Register, IOperand? Operand);
}
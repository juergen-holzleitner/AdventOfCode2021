namespace _24_ALU
{
  public enum Register { w, x, y, z }

  public enum Operation { inp, add, mul, div, mod, eql };

  public interface IOperand { }

  public record RegisterOperand(Register Register) : IOperand;

  public record NumberOperand(int Number) : IOperand;

  public record Instruction(Operation Operation, Register Register, IOperand? Operand);
}
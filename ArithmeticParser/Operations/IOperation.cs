using System.Numerics;

namespace ArithmeticParser.Operations;

public interface IOperation
{
    public Operations GetOperationType();
    public BigInteger Execute(BigInteger first, BigInteger second);
}
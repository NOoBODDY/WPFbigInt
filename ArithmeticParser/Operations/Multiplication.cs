using System.Numerics;

namespace ArithmeticParser.Operations;

public class Multiplication:ContinuosOperation
{
    public override Operations GetOperationType()
    {
        return Operations.Multiplication;
    }

    public override BigInteger Execute(BigInteger first, BigInteger second)
    {
        base.Execute(first, second);
        return DoOperation((x, y) => x * y);
    }
    
}
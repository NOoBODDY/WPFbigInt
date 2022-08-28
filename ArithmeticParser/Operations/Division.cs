using System.Numerics;

namespace ArithmeticParser.Operations;

public class Division:ContinuosOperation
{
    public override Operations GetOperationType()
    {
        return Operations.Division;
    }

    public override BigInteger Execute(BigInteger first, BigInteger second)
    {
        base.Execute(first, second);
        return DoOperation((x, y) => x / y);
    }
    
}
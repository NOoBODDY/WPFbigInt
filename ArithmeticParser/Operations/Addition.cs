using System.Diagnostics;
using System.Numerics;

namespace ArithmeticParser.Operations;

public class Addition: ContinuosOperation
{
    
    public override Operations GetOperationType()
    {
        return Operations.Addition;
    }

    public override BigInteger Execute(BigInteger first, BigInteger second)
    {
        base.Execute(first, second);
        return DoOperation((x, y) => x + y);
    }
    
}
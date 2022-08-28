using System.Diagnostics;
using System.Numerics;

namespace ArithmeticParser.Operations;

public class UnarSubtraction:ContinuosOperation
{
    public override Operations GetOperationType()
    {
        return Operations.UnarSubtraction;
    }

    public override BigInteger Execute(BigInteger first, BigInteger second)
    {
        base.Execute(first, second);

        return DoOperation((x, y) => -x);
    }
    
}
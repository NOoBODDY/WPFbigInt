using System.Diagnostics;
using System.Numerics;

namespace ArithmeticParser.Operations;

public abstract class ContinuosOperation: IOperation, ITimeCounter
{
    private TimeSpan _operationDuration;
    private Stopwatch _timePerOperation;
    private BigInteger _first;
    private BigInteger _second;

    protected ContinuosOperation()
    {
        _operationDuration = TimeSpan.Zero;
    }

    public abstract Operations GetOperationType();

    public virtual BigInteger Execute(BigInteger first, BigInteger second)
    {
        _first = first;
        _second = second;
        return default;
    }

    public TimeSpan GetExecutionDuration()
    {
        return _operationDuration;
    }

    private protected BigInteger DoOperation(Func<BigInteger, BigInteger, BigInteger> operationFunction)
    {
        Stopwatch timePerOperation = new Stopwatch();
        timePerOperation.Start();
        var result = operationFunction(_first, _second);
        timePerOperation.Stop();
        _operationDuration = timePerOperation.Elapsed;
        return result;
    }
    
}
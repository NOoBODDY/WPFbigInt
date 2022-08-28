using System.Diagnostics;
using System.Numerics;
using ArithmeticParser;
using ArithmeticParser.Operations;

/// <summary>
/// Грамматика:
/// E = T + E | T - E | E
///T = F * T | F / t | F
/// F = N | -F | (E)
/// </summary>
public class SyntaxParserBigInt
{
    private readonly string[] _tokens;

    private int _index;
    

    public Dictionary<Operations, int> OperaionsCounter { get; private set; }
    public Dictionary<Operations, TimeSpan> OperaionsDurations { get; private set; }
    
    public SyntaxParserBigInt(string[] tokens)
    {
        _tokens = tokens;
    }

    private void SetZeroFields()
    {
        OperaionsCounter = new Dictionary<Operations, int>();
        OperaionsDurations = new Dictionary<Operations, TimeSpan>();
        foreach (var operation in (Operations[]) Enum.GetValues(typeof(Operations)))
        {
            OperaionsCounter.Add(operation, 0);
            OperaionsDurations.Add(operation, TimeSpan.Zero);
        }
    }
    
    public BigInteger Parse()
    {
        SetZeroFields();
        
        BigInteger result = expression();
        if (_index != _tokens.Length)
            throw new Exception($@"Error at {_tokens[_index]}");
        return result;
    }
    BigInteger expression()
    {
        BigInteger first = term();
        while (_index < _tokens.Length)
        {
            string token = _tokens[_index];
            if (!(token == "+" || token == "-"))
                break;
            _index++;

            BigInteger second = term();

            first = DoOperation(token, first, second);
        }

        return first;
    }

    BigInteger term()
    {
        BigInteger first = factor();
        while (_index < _tokens.Length)
        {
            string token = _tokens[_index];
            if (!(token == "*" || token == "/"))
                break;
            _index++;

            BigInteger second = factor();

            first = DoOperation(token, first, second);
        }

        return first;
    }

    BigInteger factor()
    {
        if (_index >= _tokens.Length)
            throw new Exception("Unexpected end of expression");
        var nextToken = _tokens[_index];
        BigInteger result;
        if (nextToken == "(")
        {
            _index++;
            result = expression();
            string rightBracket;
            if (_index < _tokens.Length)
            {
                rightBracket = _tokens[_index];
                if (rightBracket == ")")
                {
                    _index++;
                    return result;
                }
                else
                {
                    throw new Exception($@"expected ')' but {rightBracket}");
                }
            }
            else
            {
                throw new Exception("Unexpected end of expression");
            }
            
        }
        else
        {
            if (nextToken == "-")
            {
                _index++;
                result = factor();
                return DoOperation("--", result, 0); // небольшой костыль не повредит :)
            }
        }

        if (! BigInteger.TryParse(nextToken, out result))
        {
            throw new Exception($@"expected numeric but {nextToken}");
        }
        _index++;
        return result;
    }

    private BigInteger DoOperation(string operationToken, BigInteger first, BigInteger second)
    {
        ContinuosOperation operation = ContinuosOperationFabric.GetInstance(operationToken);
        BigInteger result = operation.Execute(first, second);
        OperaionsCounter[operation.GetOperationType()]++;
        OperaionsDurations[operation.GetOperationType()] += operation.GetExecutionDuration();
        return result;
    }
}

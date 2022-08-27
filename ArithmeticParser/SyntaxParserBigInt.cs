using System.Numerics;
/// <summary>
/// Грамматика:
/// E = T + E | T - E | E
///T = F * T | F / t | F
/// F = N | -N | (E)
/// </summary>
public class SyntaxParserBigInt
{
    private readonly string[] _tokens;
    private int _index;

    public SyntaxParserBigInt(string[] tokens)
    {
        _tokens = tokens;
    }

    public BigInteger Parse()
    {
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
            if (token == "+")
            {
                first += second;
            }
            else
            {
                first -= second;
            }
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
            if (token == "*")
            {
                first *= second;
            }
            else
            {
                first /= second;
            }
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
                return -factor();
            }
        }

        if (! BigInteger.TryParse(nextToken, out result))
        {
            throw new Exception($@"expected numeric but {nextToken}");
        }
        _index++;
        return result;
    }
}
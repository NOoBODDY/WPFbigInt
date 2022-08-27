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
        var nextToken = _tokens[_index];
        if (nextToken == "(")
        {
            _index++;
            BigInteger result = expression();
            string rightBracket = _tokens[_index];
            if (_index > _tokens.Length)
                throw new Exception("Unexpected end of expression");
            if (rightBracket == ")")
            {
                _index++;
                return result;
            }

            throw new Exception($@"expected ')' but {rightBracket}");
        }
        else
        {
            if (nextToken == "-")
            {
                _index+=2;
                return -BigInteger.Parse(_tokens[_index-1]);
            }
        }
        _index++;
        return BigInteger.Parse(nextToken);
    }
}
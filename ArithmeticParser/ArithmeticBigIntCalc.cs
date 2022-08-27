using System.Numerics;

namespace ArithmeticParser;

public class ArithmeticBigIntCalc
{
    private readonly string _input;

    public ArithmeticBigIntCalc(string input) => _input = input;
    public BigInteger Calc()
    {
        var Ling = new LinguisticAnalyzer(_input);
        var tokens = Ling.ParseTokens();
        var Syntax = new SyntaxParserBigInt(tokens);
        return Syntax.Parse();
    }
}
using System.Numerics;
using ArithmeticParser.Operations;

namespace ArithmeticParser;

public class ArithmeticBigIntCalc
{
    private readonly string _input;

    public Dictionary<Operations.Operations, int> OperaionsCounter { get; private set; }
    public Dictionary<Operations.Operations, TimeSpan> OperaionsDurations { get; private set; }
    public ArithmeticBigIntCalc(string input) => _input = input;

    public BigInteger Calc()
    {
        var Ling = new LinguisticAnalyzer(_input);
        var tokens = Ling.ParseTokens();
        var Syntax = new SyntaxParserBigInt(tokens);
        var result = Syntax.Parse();
        OperaionsCounter = Syntax.OperaionsCounter;
        OperaionsDurations = Syntax.OperaionsDurations;
        return result;
    }
}
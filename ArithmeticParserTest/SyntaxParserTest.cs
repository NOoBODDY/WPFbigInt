using System.Numerics;

namespace ArithmeticParserTest;

public class SyntaxParserTest
{
    [Fact]
    public void SimpleExpression()
    {
        var tokens = new string[] {"12", "+", "31", "-", "53", "*", "42", "/", "2" };
        var syntaxParser = new SyntaxParserBigInt(tokens);

        var result = syntaxParser.Parse();
        
        Assert.Equal(new BigInteger(-1070), result);
    }

    [Fact]
    public void BigExpression()
    {
        var tokens = new string[] { "2", "+", "3", "*", "(", "4", "-", "5", ")", "+", "6", "-", "7" };
        var syntaxParser = new SyntaxParserBigInt(tokens);
        var result = syntaxParser.Parse();
        
        Assert.Equal(new BigInteger(-2), result);
    }
    [Fact]
    public void BigExpressionWithUnarMinus()
    {
        var tokens = new string[] { "-", "2", "+", "3", "*", "(", "4", "-", "5", ")", "+", "6", "-", "7" };
        var syntaxParser = new SyntaxParserBigInt(tokens);
        var result = syntaxParser.Parse();
        
        Assert.Equal(new BigInteger(-6), result);
    }
}
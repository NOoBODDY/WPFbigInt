using System.Numerics;
using ArithmeticParser;
using ArithmeticParser.Operations;

namespace ArithmeticParserTest;

public class ArithmeticBigIntCalcTest
{
    [Fact]
    public void SimpleExpression()
    {
        string input = "345 + 432 - 123";
        ArithmeticBigIntCalc parser = new ArithmeticBigIntCalc(input);

        BigInteger result = parser.Calc();
        
        Assert.Equal(new BigInteger(654), result);
        
    }
    [Fact]
    public void SimpleExpressionWithBrackets()
    {
        string input = "345 + (432 -123)";
        ArithmeticBigIntCalc parser = new ArithmeticBigIntCalc(input);

        BigInteger result = parser.Calc();
        
        Assert.Equal(new BigInteger(654), result);
    }
    
    [Fact]
    public void ArithmeticOrderTest()
    {
        string input = "345 * (432 -123)";
        ArithmeticBigIntCalc parser = new ArithmeticBigIntCalc(input);

        BigInteger result = parser.Calc();
        
        Assert.Equal(new BigInteger(106605), result);
    }
    
    [Fact]
    public void ArithmeticOrderMoreBracketsTest()
    {
        string input = "(43+12)*54 -(234 + 35 * (43 -23))";
        ArithmeticBigIntCalc parser = new ArithmeticBigIntCalc(input);

        BigInteger result = parser.Calc();
        
        Assert.Equal(new BigInteger(2036), result);
        Assert.Equal(2, parser.OperaionsCounter[Operations.Addition]);
        Assert.Equal(2, parser.OperaionsCounter[Operations.Subtraction]);
        Assert.Equal(2, parser.OperaionsCounter[Operations.Multiplication]);
    }
    [Fact]
    public void BigIntTestTest()
    {
        string input = "345735645345345 * (432234234999234234 -123234222344)";
        ArithmeticBigIntCalc parser = new ArithmeticBigIntCalc(input);

        BigInteger result = parser.Calc();
        
        Assert.Equal(BigInteger.Parse("149438739571348363549307081152050"), result);
    }
}
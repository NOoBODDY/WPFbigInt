using ArithmeticParser;

namespace ArithmeticParserTest;

public class LinguisticAnalyzerTest
{
    [Fact]
    public void NumericTokensTest()
    {
        string line = "42345456 123435 63423 41234";
        var analyzer = new LinguisticAnalyzer(line);

        string[] tokens = analyzer.ParseTokens();
        
        Assert.Equal(new string[] {"42345456", "123435", "63423", "41234"}, tokens);
    }
    
    [Fact]
    public void NumericAndArithmeticSignsSplitedBySpaceTokensTest()
    {
        string line = "42345456 + 123435 - 63423 41234";
        var analyzer = new LinguisticAnalyzer(line);

        string[] tokens = analyzer.ParseTokens();
        
        Assert.Equal(new string[] {"42345456","+", "123435","-", "63423", "41234"}, tokens);
    }
    [Fact]
    public void NumericAndArithmeticSignsWithoutSpaceTokensTest()
    {
        string line = "42345456+123435-63423 41234";
        var analyzer = new LinguisticAnalyzer(line);

        string[] tokens = analyzer.ParseTokens();
        
        Assert.Equal(new string[] {"42345456","+", "123435","-", "63423", "41234"}, tokens);
    }
    [Fact]
    public void NumericAndArithmeticSignsAndBracketsWithoutSpaceTokensTest()
    {
        string line = "42345456+(123435-63423)- 41234";
        var analyzer = new LinguisticAnalyzer(line);

        string[] tokens = analyzer.ParseTokens();
        
        Assert.Equal(new string[] {"42345456","+","(", "123435","-", "63423",")","-", "41234"}, tokens);
    }
    
    [Fact]
    public void AllLexTest()
    {
        string line = "-2342*((423+34)/3 +42345456+(123435-63423))- 41234";
        var analyzer = new LinguisticAnalyzer(line);

        string[] tokens = analyzer.ParseTokens();
        
        Assert.Equal(new string[] {"-","2342","*","(","(","423","+","34",")","/","3","+","42345456","+","(", "123435","-", "63423",")",")","-", "41234"}, tokens);
    }
    
    [Fact]
    public void SignCountTest()
    {
        string line = "42345456+(123435-63423)- 41234";
        var analyzer = new LinguisticAnalyzer(line);

        string[] tokens = analyzer.ParseTokens();
        
        Assert.Equal(1, analyzer.PlusCount);
        Assert.Equal(2, analyzer.MinusCount);
        Assert.Equal(0, analyzer.MultCount);
        Assert.Equal(0, analyzer.DivCount);
    }
}
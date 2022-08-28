namespace ArithmeticParser.Operations;

public class ContinuosOperationFabric
{
    public static ContinuosOperation GetInstance(string token)
    {
        switch (token)
        {
            case "+":
                return new Addition();
            case "-":
                return new Subtraction();
            case "*":
                return new Multiplication();
            case "/":
                return new Division();
            case "--":
                return new UnarSubtraction();
            default:
                throw new Exception($@"{token}: unknown operation");
        }
    }
}
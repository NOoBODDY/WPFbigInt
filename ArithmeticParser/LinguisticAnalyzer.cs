using System.Text.RegularExpressions;

namespace ArithmeticParser;
/// <summary>
/// [0..9] - число
/// - + / * - арифметические знаки
/// ( ) - скобки
/// </summary>
public class LinguisticAnalyzer
{
    private readonly string _line;
    private readonly Regex _regex;
    
    public int PlusCount { get; private set; }
    public int MinusCount { get; private set; }
    public int MultCount { get; private set; }
    public int DivCount { get; private set; }
    
    public LinguisticAnalyzer(string line)
    {
        _line = line;
        string pattern = @"(?<NUM>\d+)+?|(?<PLUS>[+]){1}|(?<MINUS>[-]){1}|(?<LEFTBR>[(]){1}|(?<RIGHTBR>[)]){1}|(?<MULT>[*]){1}|(?<DIV>[/]){1}";
        _regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    }
    public string[] ParseTokens()
    {
        PlusCount = 0;
        MinusCount = 0;
        MultCount = 0;
        DivCount = 0;
        var matches = _regex.Matches(_line);
        foreach (Match match in matches)
        {
            var groups = match.Groups;
            if (groups["PLUS"].Success)
                PlusCount++;
            if (groups["MINUS"].Success)
                MinusCount++;
            if (groups["MULT"].Success)
                MultCount++;
            if (groups["DIV"].Success)
                DivCount++;
        }
        return matches.Select(x => x.Value).ToArray();
        
    }
    
}
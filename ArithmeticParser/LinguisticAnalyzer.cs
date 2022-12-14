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

    public LinguisticAnalyzer(string line)
    {
        _line = line;
        string pattern = @"(?<NUM>\d+)+?|(?<PLUS>[+]){1}|(?<MINUS>[-]){1}|(?<LEFTBR>[(]){1}|(?<RIGHTBR>[)]){1}|(?<MULT>[*]){1}|(?<DIV>[/]){1}";
        _regex = new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
    }
    public string[] ParseTokens()
    {
        var matches = _regex.Matches(_line);
        return matches.Select(x => x.Value).ToArray();
        
    }
    
}
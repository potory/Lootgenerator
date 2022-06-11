using System.Text.RegularExpressions;

namespace LootGenerator.Utilities;

public class DiceUtility
{
    private readonly Regex _regex = new Regex(@"^(\d+)?[dк](\d+)([\+\-]\d+)?$");
    private readonly Random _random = new Random();

    public bool IsCorrectDiceString(string str)
    {
        if (str == null)
        {
            throw new ArgumentException(null, nameof(str));
        }
        
        return int.TryParse(str, out _) || _regex.IsMatch(str.ToLower());
    }

    public int Roll(string str, out string calculations)
    {
        if (str == null)
        {
            throw new ArgumentException(null, nameof(str));
        }

        if (int.TryParse(str, out var result))
        {
            calculations = result.ToString();
            return result;
        }

        str = str.ToLower();
        
        var match = _regex.Match(str);

        var count = int.Parse(match.Groups[1].Value);
        var dice = int.Parse(match.Groups[2].Value);

        calculations = string.Empty;

        for (var i = 0; i < count; i++)
        {
            var num = _random.Next(1, dice + 1);
            result += num;

            if (i > 0)
            {
                calculations += '+';
            }

            calculations += num.ToString();
        }

        if (!int.TryParse(match.Groups[3].Value, out var mod))
        {
            return result;
        }
        
        result += mod;

        switch (mod)
        {
            case > 0:
                calculations += $"+{mod}";
                break;
            case < 0:
                calculations += $"+({mod})";
                break;
        }

        return result;
    }
}
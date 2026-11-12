using System;
using System.Text.RegularExpressions;

public class Test
{
    public static void Main()
    {
        string text = ""11 01 001 2 2 1 1 H 045 01 I11 A001 000 A 1B 43909 00 25 1 1 5 S 28 C0010 811 2025 2 1 3,569.77"";
        string pattern = @""([A-Z0-9][A-Z0-9\s\-]{35,85}?)[^\d,]*?([\d,]+\.\d{2})"";
        var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
        foreach (Match match in matches) {
            string currentClave = Regex.Replace(match.Groups[1].Value, @""\s+"", """");
            char toChar = currentClave[currentClave.Length - 1];
            Console.WriteLine(currentClave + "" | "" + toChar);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DisplayNames.Utils
{
    internal class StringTools
    {
        public static Dictionary<string, string> SpecialDict = new Dictionary<string, string>()
        {
            {"1", "!"}, {"2", "@"}, {"3", "#"}, {"4", "$"},
            {"5", "%"}, {"6", "^"}, {"7", "&"}, {"8", "*"},
            {"9", "("}, {"0", ")"}, {"H", ":"}, {"J", ";"},
            {"K", "'"}, {"L", "\""}, {"V", "<"}, {"B", ">"},
            {"N", "."}, {"M", "/"}, {"Q", "`"}, {"W", "~"}
        };

        public static string AlphaToSpecial(string Alphanumeric)
        {
            if (Alphanumeric.Length > 1 || !SpecialDict.ContainsKey(Alphanumeric)) return "";
            SpecialDict.TryGetValue(Alphanumeric, out var special);
            return special;
        }

        public static string TruncateString(string str)
        {
            if (str.Length > Main.MaxCharacters)
            {
                return str.Substring(0, Main.MaxCharacters);
            }
            else return str;
        }
    }
}

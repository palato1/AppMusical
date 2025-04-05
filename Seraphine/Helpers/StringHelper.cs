using System.Text.RegularExpressions;

namespace Seraphine.Helpers;

public static class StringHelper
{
    public static string SomenteNumeros(this string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return "";
        }

        s = string.Join("", Regex.Split(s, @"[^\d]"));

        return s;
    }
}

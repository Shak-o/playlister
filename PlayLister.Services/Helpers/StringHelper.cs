using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayLister.Services.Helpers
{
    public static class StringHelper
    {
        //TODO: Could be improved
        public static string GetValidName(string name)
        {
            var validName = string.Empty;
            List<char> toRemove = new List<char>()
                {'"', '!', '@', '#', '$', '%', '^', '&', '*', '{', '}', '/', '\\', '|', ',', '.', '\'', '~', '`'};

            foreach (char c in name)
            {
                if ((c == ']' || c == ')') && name.IndexOf(c) != name.Length - 1)
                {
                    if ((c == ']' && name.IndexOf('[') == 0) || (c == ')' && name.IndexOf('(') == 0))
                        validName = name.Substring(name.IndexOf(c) + 1, name.Length - name.IndexOf(c) - 1).Trim();
                    else
                    {
                        var firstPart = c == ']' ? validName.Substring(0, validName.IndexOf('[')) : validName.Substring(0, validName.IndexOf('('));
                        var secondPart = c == ']'
                            ? validName.Substring(validName.IndexOf(']') + 1, validName.Length - validName.IndexOf(']') - 1)
                            : validName.Substring(validName.IndexOf(')') + 1, validName.Length - validName.IndexOf(')') - 1);
                        validName = firstPart + secondPart;
                    }
                }
                if ((c == ']' || c == ')') && name.IndexOf(c) == name.Length - 1)
                {
                    validName = c == ']' ? name.Substring(0, name.IndexOf('[')).Trim() : name.Substring(0, name.IndexOf('(')).Trim();
                }
            }

            foreach (var character in toRemove)
            {
                if (validName.Contains(character))
                    validName = validName.Remove(validName.IndexOf(character), 1);
            }

            return validName;
        }
    }
}

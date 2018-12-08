using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adventOfCode18.challenges.day5
{
    public class Day5Challenge : IChallenge
    {
        private const string Puzzle = "challenges/day5/puzzle5.txt";

        public async Task<string> Challenge1()
        {
            var polymers = (await ParsePuzzle(Puzzle)).First();
            var stringBuilder = new StringBuilder(15000);
            stringBuilder.Append(polymers[0]);
            for (var i = 1; i < polymers.Length; i++)
            {
                var curChar = polymers[i];
                var prevChar = stringBuilder.Length == 0 ? ' ' : stringBuilder[stringBuilder.Length - 1];
                if (char.ToUpperInvariant(curChar).Equals(char.ToUpperInvariant(prevChar)) && !curChar.Equals(prevChar))
                {
                    stringBuilder.Length--;
                }
                else
                {
                    stringBuilder.Append(curChar);
                }
            }

            return stringBuilder.Length.ToString();
        }


        public async Task<string> Challenge2()
        {
            var polymers = (await ParsePuzzle(Puzzle)).First();
            var popularChars = polymers.ToLower().GroupBy(cr => cr).OrderBy(g => g.Count()).Take(10);
            var res = new List<int>();
            foreach (var g in popularChars)
            {
                var c = g.Key;
                var s = string.Copy(polymers).Replace(c + "", "").Replace("" + char.ToUpper(c), "");
                var stringBuilder = new StringBuilder(15000);
                stringBuilder.Append(s[0]);
                for (var i = 1; i < s.Length; i++)
                {
                    var curChar = s[i];
                    var prevChar = stringBuilder.Length == 0 ? ' ' : stringBuilder[stringBuilder.Length - 1];
                    if (char.ToUpperInvariant(curChar).Equals(char.ToUpperInvariant(prevChar)) &&
                        !curChar.Equals(prevChar))
                    {
                        stringBuilder.Length--;
                    }
                    else
                    {
                        stringBuilder.Append(curChar);
                    }
                }

                res.Add(stringBuilder.Length);
            }

            return res.Min().ToString();
        }

        private async Task<IReadOnlyCollection<string>> ParsePuzzle(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var content = await sr.ReadToEndAsync();
                return content.Split("\n").ToArray();
            }
        }
    }
}
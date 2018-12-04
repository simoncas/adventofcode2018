using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventOfCode18.challenges.day3
{
    public class Day3Challenge : IChallenge
    {
        private const string Puzzle = "challenges/day3/puzzle3.txt";

        public async Task<string> Challenge1()
        {
            var claims = await ParsePuzzle(Puzzle);
            var res = claims.SelectMany(c => c.Area.ToArray()).GroupBy(x => x.Item2).Count(g => g.Count() > 1);
            return res.ToString();
        }

        public async Task<string> Challenge2()
        {
            var claims = await ParsePuzzle(Puzzle);
            var ids = claims.Select(c => c.Id).ToArray();
            var overlappedIds = claims.SelectMany(c => c.Area.ToArray()).GroupBy(x => x.Item2)
                .Where(g => g.Count() > 1).SelectMany(g => g.Select(p => p.Item1)).Distinct().ToArray();
            return ids.First(id => !overlappedIds.Contains(id));
        }
        
        private async Task<IReadOnlyCollection<Claim>> ParsePuzzle(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var content = await sr.ReadToEndAsync();
                return content.Split("\n").Select(r => new Claim(r)).ToArray();
            }
        }

        private class Claim
        {
            public string Id { get; }
            private int InchesLeft { get; }
            private int InchesTop { get;}
            private int Wide { get;}
            private int Height { get;  }

            public List<(string,string)> Area { get; }

            public Claim(String rawClaim)
            {
                var linePattern =
                    new Regex("#(?<id>[0-9]+) @ (?<y>[0-9]+),(?<x>[0-9]+): (?<width>[0-9]+)x(?<height>[0-9]+)");
                var match = linePattern.Match(rawClaim);
                Id = match.Groups["id"].Value;
                InchesLeft = int.Parse(match.Groups["y"].Value);
                InchesTop = int.Parse(match.Groups["x"].Value);
                Wide = int.Parse(match.Groups["width"].Value);
                Height = int.Parse(match.Groups["height"].Value);
                Area = new List<(string,string)>();
                for (var i = InchesLeft+1; i <= InchesLeft+Wide; i++)
                {
                    for (var j = InchesTop+1; j <= InchesTop+Height; j++)
                    {
                        Area.Add((Id,$"{i}-{j}"));
                    }
                }
            }
        }
    }

}


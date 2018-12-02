using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adventOfCode18.challenges.day2
{
    public class Day2Challenge : IChallenge

    {
        private const string Puzzle = "challenges/day2/puzzle2.txt";
        public async Task<string> Challenge1()
        {
            var inputs = await ParsePuzzle(Puzzle);
            var twos = inputs.Count(s => s.GroupBy(c => c).Any(g => g.Count() == 2));
            var threes = inputs.Count(s => s.GroupBy(c => c).Any(g => g.Count() == 3));
            return (twos * threes).ToString();
        }

        public async Task<string> Challenge2()
        {
            var inputs = await ParsePuzzle(Puzzle);
            var idLength = inputs.ToArray()[0].Length;
            for (int i = 0; i < idLength; i++)
            {
                var idx = i;
                var groups = inputs.Select(s => s.Remove(idx,1)).GroupBy(s => s).Where(g => g.Count() == 2);
                var groupsArr = groups.ToArray();
                if (groupsArr.Length == 1)
                {
                    return groupsArr.ToArray().First().Key;
                }
            }
            return "No solution found";
        }
        
        private async Task<IReadOnlyCollection<string>> ParsePuzzle(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var content = await sr.ReadToEndAsync();
                return content.Split("\n");
            }
        }
    }
}
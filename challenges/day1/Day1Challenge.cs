using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adventOfCode18.challenges.day1
{
    public class Day1Challenge : IChallenge
    {
        private const string Puzzle = "challenges/day1/puzzle1.txt";

        public async Task<string> Challenge1()
        {
            var frequencies = await ParsePuzzle(Puzzle);
            return frequencies.Sum().ToString();
        }

        public async Task<string> Challenge2()
        {
            var frequencies = await ParsePuzzle(Puzzle);
            var frequencyHistories = new HashSet<int>();
            var crtFrequency = 0;
            while (true)
            {
                foreach (var f in frequencies)
                {
                    crtFrequency += f;
                    if (frequencyHistories.Contains(crtFrequency))
                    {
                        return crtFrequency.ToString();
                    }
                    frequencyHistories.Add(crtFrequency);
                }
            }
        }
        
        private async Task<IReadOnlyCollection<int>> ParsePuzzle(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var content = await sr.ReadToEndAsync();
                return content.Split("\n").Select(int.Parse).ToArray();
            }
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using adventOfCode18.day;

namespace adventOfCode18.day1
{
    public class Day1Challenge : IChallenge
    {
        private const string Puzzle = "day1/puzzle1.txt";

        private async Task<IReadOnlyCollection<int>> ParsePuzzle(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var content = await sr.ReadToEndAsync();
                return content.Split("\n").Select(int.Parse).ToArray();
            }
        }

        public async Task<string> Challenge1()
        {
            var frequencies = await ParsePuzzle(Puzzle);
            return frequencies.Sum().ToString();
        }

        public async Task<string> Challenge2()
        {
            var frequencies = await ParsePuzzle(Puzzle);
            var frequencyHistories = new SortedSet<int>();
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

    }
}
#!/usr/bin/env bash

# exit on error
set -e
cd "$(dirname "$0")"

if [[ $# -eq 0 ]]
    then
    echo "No arguments supplied, day number must be supplied"
    exit 1
fi

dayNum=$1

if [[ -d challenges/day${dayNum} ]]
    then
    echo "Challenge ${dayNum} already exists"
    exit 1
fi

# Create the challenge template for the desired day

mkdir -p challenges/day${dayNum}
touch challenges/day${dayNum}/Day${dayNum}Challenge.cs
touch challenges/day${dayNum}/puzzle${dayNum}.txt

echo "using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace adventOfCode18.challenges.day${dayNum}
{
    public class Day${dayNum}Challenge : IChallenge
    {
        private const string Puzzle = \"challenges/day${dayNum}/puzzle1.txt\";

        public async Task<string> Challenge1()
        {
             return \"No solution implemented\";
        }

        public async Task<string> Challenge2()
        {
             return \"No solution implemented\";
        }
        
        private async Task<IReadOnlyCollection<string>> ParsePuzzle(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var content = await sr.ReadToEndAsync();
                return content.Split(\"\n\").ToArray();
            }
        }
    }
}
" > challenges/day${dayNum}/Day${dayNum}Challenge.cs
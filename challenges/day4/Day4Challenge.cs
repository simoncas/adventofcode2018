using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventOfCode18.challenges.day4
{
    public class Day4Challenge : IChallenge
    {
        private const string Puzzle = "challenges/day4/puzzle4.txt";

        public async Task<string> Challenge1()
        {
            var inputs = await ParsePuzzle(Puzzle);

            var shiftEvents = inputs.Select(l => new ShiftEvent(l)).ToList();
            shiftEvents.Sort((a, b) => a.DateTime.CompareTo(b.DateTime));

            var crtGuardId = "";
            foreach (var shiftEvent in shiftEvents)
            {
                if (shiftEvent.IsBeginShift)
                {
                    crtGuardId = shiftEvent.GuardId;
                }
                else
                {
                    shiftEvent.GuardId = crtGuardId;
                }
            }

            var guards = shiftEvents.GroupBy(s => s.GuardId).Select(g => new Guard(g.Key, g.ToArray())).ToList();
            guards.Sort((g1, g2) => g1.SleepDuration.CompareTo(g2.SleepDuration));

            return (int.Parse(guards.Last().GuardId) * guards.Last().MostAsleepMinute).ToString();
        }

        public async Task<string> Challenge2()
        {
            var inputs = await ParsePuzzle(Puzzle);
            var shiftEvents = inputs.Select(l => new ShiftEvent(l)).ToList();
            shiftEvents.Sort((a, b) => a.DateTime.CompareTo(b.DateTime));

            var crtGuardId = "";
            foreach (var shiftEvent in shiftEvents)
            {
                if (shiftEvent.IsBeginShift)
                {
                    crtGuardId = shiftEvent.GuardId;
                }
                else
                {
                    shiftEvent.GuardId = crtGuardId;
                }
            }

            var guards = shiftEvents.GroupBy(s => s.GuardId).Select(g => new Guard(g.Key, g.ToArray())).ToList();
            var (guardId, minute, _) = guards.Select(g => g.ComputeMostAsleepFrequency()).OrderByDescending(a => a.Item3).First();
            return (int.Parse(guardId) * minute).ToString();
        }

        private async Task<IReadOnlyCollection<string>> ParsePuzzle(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var content = await sr.ReadToEndAsync();
                return content.Split("\n").ToArray();
            }
        }


        private class Guard
        {
            public string GuardId { get; }
            private ShiftEvent[] ShiftEvents { get; }

            public double SleepDuration { get; private set; }

            public double MostAsleepMinute { get; private set; }

            public Guard(string guardId, ShiftEvent[] shiftEvents)
            {
                GuardId = guardId;
                ShiftEvents = shiftEvents;
                ComputeSleepTime();
                ComputeMostAsleepMinute();
            }

            private void ComputeSleepTime()
            {
                var shiftSleepDuration = 0.0;
                for (var i = 0; i < ShiftEvents.Length; i++)
                {
                    var shiftEvent = ShiftEvents[i];
                    if (shiftEvent.IsWakeUp)
                    {
                        shiftSleepDuration += (shiftEvent.DateTime - ShiftEvents[i - 1].DateTime).TotalMinutes;
                    }
                }
                SleepDuration = shiftSleepDuration;
            }


            private void ComputeMostAsleepMinute()
            {
                if (!ShiftEvents.Any(s => s.IsFallAsleep))
                {
                    MostAsleepMinute = -1;
                    return;
                }

                var minutes = new List<int>();
                for (var i = 0; i < ShiftEvents.Length; i++)
                {
                    if (!ShiftEvents[i].IsWakeUp) continue;
                    var start = ShiftEvents[i - 1].DateTime.Minute;
                    var end = ShiftEvents[i].DateTime.Minute;
                    minutes.AddRange(Enumerable.Range(start, (end - start)));
                }

                MostAsleepMinute = minutes.GroupBy(i => i).Select(g => (g.Key, g.Count())).OrderBy(g => g.Item2)
                    .Last()
                    .Item1;
            }

            public (string, int, int) ComputeMostAsleepFrequency()
            {
                if (!ShiftEvents.Any(s => s.IsFallAsleep))
                {
                    return (GuardId, -1, -1);
                }
                var minutes = new List<int>();
                for (var i = 0; i < ShiftEvents.Length; i++)
                {
                    if (!ShiftEvents[i].IsWakeUp) continue;
                    var start = ShiftEvents[i - 1].DateTime.Minute;
                    var end = ShiftEvents[i].DateTime.Minute;
                    minutes.AddRange(Enumerable.Range(start, (end - start)));
                }
                var (minute, count) = minutes.GroupBy(i => i).Select(g => (g.Key, g.Count())).OrderBy(g => g.Item2)
                    .Last();
                return (GuardId, minute, count);
            }
        }

        private class ShiftEvent
        {
            private static readonly Regex LinePattern =
                new Regex(@"\[(?<time>.*)\] (Guard #(?<id>\d+)|(?<up>wakes up)|(?<sleep>falls asleep))");

            public string GuardId { get; set; }
            public DateTime DateTime { get; }
            public bool IsBeginShift { get; }
            public bool IsWakeUp { get;}
            public bool IsFallAsleep { get; }

            public ShiftEvent(string line)
            {
                var matches = LinePattern.Match(line);
                DateTime = DateTime.ParseExact(matches.Groups["time"].ToString(), "yyyy-MM-dd HH:mm", null);
                GuardId = matches.Groups["id"].Value;
                IsBeginShift = matches.Groups["id"].Success;
                IsWakeUp = matches.Groups["up"].Success;
                IsFallAsleep = matches.Groups["sleep"].Success;
            }
        }
    }
}
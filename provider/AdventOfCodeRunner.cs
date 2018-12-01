using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using adventOfCode18.challenges;

namespace adventOfCode18.provider
{
    public static class AdventOfCodeRunner
    {
        public static async Task Run()
        {
            //Load all classes that implements the IDayRunner interface 
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && typeof(IChallenge).IsAssignableFrom(p));
            var tasks = new List<Task>();
            foreach (var t in types)
            {
                var dayRunner = (IChallenge) Activator.CreateInstance(t);
                tasks.AddRange(DayRunner(dayRunner));
            }
            await Task.WhenAll(tasks);
        }

        private static IEnumerable<Task> DayRunner(IChallenge challenge)
        {
            return new List<Task>()
            {
                challenge.Challenge1().ContinueWith(r => PresentResult(r, challenge, true)),
                challenge.Challenge2().ContinueWith(r => PresentResult(r, challenge, false))
            };
        }

        private static async Task PresentResult(Task<string> result, IChallenge challenge, bool isChallenge1)
        {
            var day = challenge.GetType().Name.Replace("Challenge", "");
            var res = await result;
            var noChallenge = isChallenge1 ? 1 : 2;
            var outputRes = $"{day}\tchallenge {noChallenge}\t-> {res}";
            Console.WriteLine(outputRes);
        }
    }
}
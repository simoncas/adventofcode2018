using System.Threading.Tasks;

namespace adventOfCode18.challenges
{
    public interface IChallenge
    {
        Task<string> Challenge1();
        Task<string> Challenge2();
    }
}
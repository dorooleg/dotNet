using System.Linq;
using System.Threading.Tasks;

namespace Multithreading
{
    public class Philosophers
    {
        private const int AmountOfEating = 10;
        private const int CountPhilisophers = 5;
        private readonly Fork[] _forks = Enumerable.Range(0, CountPhilisophers).Select(_ => new Fork()).ToArray();

        public bool Simulate()
            => Enumerable
                .Range(0, CountPhilisophers)
                .Select(idx => Task.Run(() =>
                    {
                        var p = new Philosopher(_forks[idx], _forks[(idx + 1) % CountPhilisophers]);
                        for (var _ = 0; _ < AmountOfEating; _++)
                        {
                            p.Eat();
                        }
                        return p;
                    })
                )
                .ToList()
                .All(a =>
                {
                    a.Wait();
                    return a.Result.IsDone;
                });
    }
}
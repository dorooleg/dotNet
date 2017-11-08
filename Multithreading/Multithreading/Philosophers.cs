using System.Linq;
using System.Threading.Tasks;

namespace Multithreading
{
    public class Philosophers
    {
        private const int AmountOfEating = 10;
        private const int CountPhilisophers = 5;
        private readonly Fork[] _forks = Enumerable.Range(0, CountPhilisophers).Select(_ => new Fork()).ToArray();

        public void Simulate()
        {
            Enumerable
                .Range(0, CountPhilisophers)
                .Select(idx => Task.Run(() =>
                    {
                        var p = new Philosopher(_forks[idx], _forks[(idx + 1) % CountPhilisophers]);
                        Enumerable.Range(0, AmountOfEating).ToList().ForEach(_ => p.Eat());
                    })
                )
                .ToList()
                .ForEach(a => a.Wait());
        }
    }
}
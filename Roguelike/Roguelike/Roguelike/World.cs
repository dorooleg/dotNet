using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;

namespace Roguelike
{
    public class World
    {
        private const char Player = '@';
        private const char Empty = ' ';
        private const char Wall = '#';
        private readonly List<char[]> _map;

        public World([NotNull] string filename)
        {
            _map = File.ReadAllLines(filename).Select(x => x.ToCharArray()).ToList();
            if (!Validate(_map))
            {
                throw new InvalidDataException();
            }

            _map.ForEach(Console.WriteLine);
        }

        private static bool Validate(List<char[]> map) => map.Count > 0 && map[0].Length > 0
                                                          && map.TrueForAll(x => x.Length == map[0].Length)
                                                          && map[0].All(x => x == Wall)
                                                          && map[map.Count - 1].All(x => x == Wall)
                                                          && map.TrueForAll(x => x[0] == Wall)
                                                          && map.TrueForAll(x => x[x.Length - 1] == Wall)
                                                          && map.Select(x => x.Count(y => y == Player)).Sum() == 1;

        private int GetPlayerX() => _map.FindIndex(x => x.Contains(Player));

        private int GetPlayerY() => Array.FindIndex(_map[GetPlayerX()], w => w == Player);

        public void MoveTo(int dx, int dy)
        {
            Assert.LessOrEqual(dx, 1);
            Assert.GreaterOrEqual(dx, -1);
            Assert.LessOrEqual(dy, 1);
            Assert.GreaterOrEqual(dy, -1);

            var x = GetPlayerX();
            var y = GetPlayerY();
            var newX = x + dx;
            var newY = y + dy;
            if (_map[newX][newY] == Wall)
                return;

            _map[x][y] = Empty;
            _map[newX][newY] = Player;

            Update(x, y, newX, newY);
        }

        private void Update(int x, int y, int newX, int newY)
        {
            Console.SetCursorPosition(y, x);
            Console.Write(Empty);
            Console.SetCursorPosition(newY, newX);
            Console.Write(Player);
            Console.SetCursorPosition(0, _map.Count);
        }
    }
}
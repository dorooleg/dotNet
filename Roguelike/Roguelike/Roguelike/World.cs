using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

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
            try
            {
                _map = File.ReadAllLines(filename).Select(x => x.ToCharArray()).ToList();
            }
            catch (IOException)
            {
            }
        }

        public (int x, int y) PlayerPosition { get; private set; }

        public bool Validate()
        {
            if (_map == null || !Validate(_map))
            {
                return false;
            }

            var playerX = _map.FindIndex(x => x.Contains(Player));
            var playerY = Array.FindIndex(_map[playerX], w => w == Player);
            PlayerPosition = (playerX, playerY);
            _map.ForEach(Console.WriteLine);
            return true;
        }

        private static bool Validate([NotNull] List<char[]> map) => map.Count > 0 && map[0].Length > 0
                                                                    && map.TrueForAll(x => x.Length == map[0].Length)
                                                                    && map[0].All(x => x == Wall)
                                                                    && map[map.Count - 1].All(x => x == Wall)
                                                                    && map.TrueForAll(x => x[0] == Wall)
                                                                    && map.TrueForAll(x => x[x.Length - 1] == Wall)
                                                                    && map.Select(x => x.Count(y => y == Player))
                                                                        .Sum() == 1;

        public void MoveTo((int dx, int dy) offset)
        {
            var (dx, dy) = offset;
            var x = PlayerPosition.x;
            var y = PlayerPosition.y;
            var newX = x + dx;
            var newY = y + dy;

            if (_map[newX][newY] == Wall)
            {
                return;
            }

            Update(x, y, newX, newY);
        }

        private void Update(int x, int y, int newX, int newY)
        {
            _map[x][y] = Empty;
            _map[newX][newY] = Player;

            PlayerPosition = (newX, newY);

            Console.SetCursorPosition(y, x);
            Console.Write(Empty);
            Console.SetCursorPosition(newY, newX);
            Console.Write(Player);
            Console.SetCursorPosition(0, _map.Count);
        }
    }
}
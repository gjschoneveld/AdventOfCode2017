using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Day10;

namespace Day14
{
    class Program
    {
        static (int x, int y) FindUsed(List<List<char>> grid)
        {
            for (int y = 0; y < grid.Count; y++)
            {
                var row = grid[y];
                for (int x = 0; x < row.Count; x++)
                {
                    if (row[x] == '#')
                    {
                        return (x, y);
                    }
                }
            }

            throw new Exception("No used location found");
        }

        static void RemoveRegion(List<List<char>> grid, (int x, int y) member)
        {
            var toVisit = new Queue<(int x, int y)>();
            toVisit.Enqueue(member);

            while(toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                if (current.y < 0 || current.y >= grid.Count)
                {
                    continue;
                }

                if (current.x < 0 || current.x >= grid[current.y].Count)
                {
                    continue;
                }

                if (grid[current.y][current.x] != '#')
                {
                    continue;
                }

                toVisit.Enqueue((current.x, current.y - 1));
                toVisit.Enqueue((current.x - 1, current.y));
                toVisit.Enqueue((current.x + 1, current.y));
                toVisit.Enqueue((current.x, current.y + 1));

                grid[current.y][current.x] = '.';
            }
        }

        static void Main(string[] args)
        {
            string input = "hxtvlmkl";
            
            var hashes = new List<string>();

            int count = 128;
            for (int i = 0; i < count; i++)
            {
                var hash = KnotHash.HashString(input + "-" + i);
                hashes.Add(hash);
            }

            var hexToSquares = new Dictionary<char, string>
            {
                { '0', "...." },
                { '1', "...#" },
                { '2', "..#." },
                { '3', "..##" },
                { '4', ".#.." },
                { '5', ".#.#" },
                { '6', ".##." },
                { '7', ".###" },
                { '8', "#..." },
                { '9', "#..#" },
                { 'a', "#.#." },
                { 'b', "#.##" },
                { 'c', "##.." },
                { 'd', "##.#" },
                { 'e', "###." },
                { 'f', "####" },
            };

            var grid = hashes.Select(h => h.SelectMany(c => hexToSquares[c]).ToList()).ToList();

            var answer1 = grid.Sum(r => r.Count(c => c == '#'));
            Console.WriteLine($"Answer 1: {answer1}");

            int regions = 0;
            while(grid.Any(r => r.Any(c => c == '#')))
            {
                var location = FindUsed(grid);
                RemoveRegion(grid, location);
                regions++;
            }

            var answer2 = regions;
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

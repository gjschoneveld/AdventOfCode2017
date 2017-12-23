using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day21
{
    class Grid
    {
        public char[,] content;

        public int Size => content.GetLength(0);

        public Grid[,] Split()
        {
            var innerSize = Size % 2 == 0 ? 2 : 3;
            var subgridCount = Size / innerSize;

            var result = new Grid[subgridCount, subgridCount];
            for (int i = 0; i < subgridCount; i++)
            {
                for (int j = 0; j < subgridCount; j++)
                {
                    var content = new char[innerSize, innerSize];

                    for (int ii = 0; ii < innerSize; ii++)
                    {
                        for (int jj = 0; jj < innerSize; jj++)
                        {
                            content[ii, jj] = this.content[i * innerSize + ii, j * innerSize + jj];
                        }
                    }

                    result[i, j] = new Grid { content = content };
                }
            }

            return result;
        }

        public static Grid Combine(Grid[,] subgrids)
        {
            var subgridCount = subgrids.GetLength(0);
            var innerSize = subgrids[0, 0].Size;
            var size = subgridCount * innerSize;

            var content = new char[size, size];

            for (int i = 0; i < subgridCount; i++)
            {
                for (int j = 0; j < subgridCount; j++)
                {
                    for (int ii = 0; ii < innerSize; ii++)
                    {
                        for (int jj = 0; jj < innerSize; jj++)
                        {
                            content[i * innerSize + ii, j * innerSize + jj] = subgrids[i, j].content[ii, jj];
                        }
                    }
                }
            }

            return new Grid { content = content };
        }

        public Grid Rotate()
        {
            var content = new char[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    content[i, j] = this.content[j, Size - i - 1];
                }
            }

            return new Grid { content = content };
        }

        public Grid Flip()
        {
            var content = new char[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    content[i, j] = this.content[i, Size - j - 1];
                }
            }

            return new Grid { content = content };
        }

        public List<Grid> Similar()
        {
            List<Grid> result = new List<Grid>();

            var current = this;
            var rotations = 4;
            for (int i = 0; i < rotations; i++)
            {
                var flipped = current.Flip();

                result.Add(current);
                result.Add(flipped);

                current = current.Rotate();
            }

            return result;
        }

        public bool Equals(Grid other)
        {
            if (Size != other.Size)
            {
                return false;
            }

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (content[i, j] != other.content[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int Count(char c)
        {
            var result = 0;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (content[i, j] == c)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public void Print()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write(content[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static Grid Parse(string x)
        {
            var rows = x.Split('/');
            var size = rows.Length;

            var content = new char[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    content[i, j] = rows[i][j];
                }
            }

            return new Grid { content = content };
        }
    }

    class Rule
    {
        Grid pattern;
        public Grid result;

        public bool Match(Grid g)
        {
            if (pattern.Count('#') != g.Count('#'))
            {
                return false;
            }

            return pattern.Similar().Any(s => s.Equals(g));
        }

        public static Rule Parse(string x)
        {
            var parts = x.Split(new char[] { ' ', '=', '>' }, StringSplitOptions.RemoveEmptyEntries);

            return new Rule
            {
                pattern = Grid.Parse(parts[0]),
                result = Grid.Parse(parts[1])
            };
        }
    }

    class Program
    {
        static int Process(List<Rule> rules, int iterations)
        {
            var grid = Grid.Parse(".#./..#/###");

            for (int it = 0; it < iterations; it++)
            {
                var subgrids = grid.Split();

                var subgridCount = subgrids.GetLength(0);
                for (int i = 0; i < subgridCount; i++)
                {
                    for (int j = 0; j < subgridCount; j++)
                    {
                        var rule = rules.First(r => r.Match(subgrids[i, j]));
                        subgrids[i, j] = rule.result;
                    }
                }

                grid = Grid.Combine(subgrids);
            }

            return grid.Count('#');
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var rules = input.Select(line => Rule.Parse(line)).ToList();

            int iterations1 = 5;
            var answer1 = Process(rules, iterations1);
            Console.WriteLine($"Answer 1: {answer1}");

            int iterations2 = 18;
            var answer2 = Process(rules, iterations2);
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

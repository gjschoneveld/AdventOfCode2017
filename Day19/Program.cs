using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    class Program
    {
        enum Direction
        {
            Down,
            Right,
            Up,
            Left
        }

        static char GetItem(List<List<char>> diagram, int x, int y)
        {
            if (y < 0 || y >= diagram.Count)
            {
                return ' ';
            }

            if (x < 0 || x >= diagram[y].Count)
            {
                return ' ';
            }

            return diagram[y][x];
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var diagram = input.Select(line => line.ToList()).ToList();

            var letters = new List<char>();

            int y = 0;
            int x = diagram[y].IndexOf('|');
            var direction = Direction.Down;

            int length = 0;
            var current = GetItem(diagram, x, y);

            while (current != ' ')
            {
                length++;

                if (char.IsLetter(current))
                {
                    letters.Add(current);
                }
                else if (current == '+')
                {
                    if (direction == Direction.Down || direction == Direction.Up)
                    {
                        if (GetItem(diagram, x - 1, y) != ' ')
                        {
                            direction = Direction.Left;
                        }
                        else
                        {
                            direction = Direction.Right;
                        }
                    }
                    else
                    {
                        if (GetItem(diagram, x , y - 1) != ' ')
                        {
                            direction = Direction.Up;
                        }
                        else
                        {
                            direction = Direction.Down;
                        }
                    }
                }

                switch (direction)
                {
                    case Direction.Down:
                        y++;
                        break;
                    case Direction.Right:
                        x++;
                        break;
                    case Direction.Up:
                        y--;
                        break;
                    case Direction.Left:
                        x--;
                        break;
                }

                current = GetItem(diagram, x, y);
            }

            var answer1 = new String(letters.ToArray());
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = length;
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

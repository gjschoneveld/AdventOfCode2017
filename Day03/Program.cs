using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    class Program
    {
        static IEnumerable<(int x, int y)> Positions()
        {
            var x = 0;
            var y = 0;
            var circle = 0;

            while (true)
            {
                // right
                for (int i = 0; i < 2 * circle - 1; i++)
                {
                    yield return (x, y);
                    y++;
                }

                // top
                for (int i = 0; i < 2 * circle; i++)
                {
                    yield return (x, y);
                    x--;
                }

                // left
                for (int i = 0; i < 2 * circle; i++)
                {
                    yield return (x, y);
                    y--;
                }

                // bottom
                for (int i = 0; i < 2 * circle + 1; i++)
                {
                    yield return (x, y);
                    x++;
                }

                circle++;
            }
        }

        static (int x, int y) FindPosition(int value)
        {
            int current = 1;
            foreach (var pos in Positions())
            {
                if (current == value)
                {
                    return pos;
                }
                current++;
            }

            throw new Exception("Position not found");
        }

        static int GetValue(Dictionary<(int x, int y), int> values, (int x, int y) pos)
        {
            if (values.ContainsKey(pos))
            {
                return values[pos];
            }

            return 0;
        }

        static int FindValue(int value)
        {
            var values = new Dictionary<(int x, int y), int>();

            foreach (var pos in Positions())
            {
                if (pos.x == 0 && pos.y == 0)
                {
                    values.Add(pos, 1);
                    continue;
                }

                var current = GetValue(values, (pos.x + 1, pos.y));
                current += GetValue(values, (pos.x + 1, pos.y - 1));
                current += GetValue(values, (pos.x, pos.y - 1));
                current += GetValue(values, (pos.x - 1, pos.y - 1));
                current += GetValue(values, (pos.x - 1, pos.y));
                current += GetValue(values, (pos.x - 1, pos.y + 1));
                current += GetValue(values, (pos.x, pos.y + 1));
                current += GetValue(values, (pos.x + 1, pos.y + 1));
                values.Add(pos, current);

                if (current > value)
                {
                    return current;
                }
            }

            throw new Exception("Value not found");
        }

        static void Main(string[] args)
        {
            int input = 312051;

            var pos = FindPosition(input);
            var answer1 = Math.Abs(pos.x) + Math.Abs(pos.y);
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = FindValue(input);
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

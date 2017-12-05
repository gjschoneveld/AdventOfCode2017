using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day05
{
    class Program
    {
        static int Steps(IEnumerable<int> items, bool part2)
        {
            var list = items.ToList();

            int index = 0;
            int steps = 0;

            while (0 <= index && index < list.Count)
            {
                var offset = list[index];

                if (part2 && offset >= 3)
                {
                    list[index]--;
                }
                else
                {
                    list[index]++;
                }

                index += offset;
                steps++;
            }

            return steps;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var items = input.Select(line => int.Parse(line));

            var answer1 = Steps(items, false);
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = Steps(items, true);
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day02
{
    class Program
    {
        static int FindPerfectDivision(IEnumerable<int> row)
        {
            foreach (var high in row)
            {
                foreach (var low in row)
                {
                    if (high / low > 1 && high % low == 0)
                    {
                        return high / low;
                    }
                }
            }

            throw new Exception("No perfect division");
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var rows = input.Select(line => line.Split('\t').Select(value => int.Parse(value)));

            var answer1 = rows.Select(r => r.Max() - r.Min()).Sum();
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = rows.Select(r => FindPerfectDivision(r)).Sum();
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

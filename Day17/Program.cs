using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            int step = 363;

            int current = 0;
            var buffer = new List<int> { 0 };

            var insertions = 2017;
            for (int i = 1; i <= insertions; i++)
            {
                current = (current + step) % buffer.Count + 1;
                buffer.Insert(current, i);
            }

            var answer1 = buffer[(current + 1) % buffer.Count];
            Console.WriteLine($"Answer 1: {answer1}");


            current = 0;
            var value = 0;

            insertions = 5_000_000;
            for (int i = 1; i <= insertions; i++)
            {
                current = (current + step) % i + 1;
                if (current == 1)
                {
                    value = i;
                }
            }

            var answer2 = value;
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}
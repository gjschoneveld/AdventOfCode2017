using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    class Generator
    {
        long current;
        long factor;
        long multiple;

        public Generator(long start, long factor, long multiple = 1)
        {
            current = start;
            this.factor = factor;
            this.multiple = multiple;
        }

        private long Step()
        {
            current = (current * factor) % 2147483647L;
            return current;
        }

        public long Next()
        {
            var next = Step();
            while (next % multiple != 0)
            {
                next = Step();
            }

            return next;
        }
    }

    class Program
    {
        static int Count(List<Generator> generators, int rounds)
        {
            var count = 0;
            for (int i = 0; i < rounds; i++)
            {
                var values = generators.Select(g => g.Next() & 0xffff).ToList();

                if (values.Min() == values.Max())
                {
                    count++;
                }
            }

            return count;
        }

        static void Main(string[] args)
        {
            var startA = 634;
            var startB = 301;

            var factorA = 16807;
            var factorB = 48271;

            var multipleA = 4;
            var multipleB = 8;

            var genA = new Generator(startA, factorA);
            var genB = new Generator(startB, factorB);

            var rounds = 40_000_000;

            var answer1 = Count(new List<Generator> { genA, genB }, rounds);
            Console.WriteLine($"Answer 1: {answer1}");


            genA = new Generator(startA, factorA, multipleA);
            genB = new Generator(startB, factorB, multipleB);

            rounds = 5_000_000;

            var answer2 = Count(new List<Generator> { genA, genB }, rounds);
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    class Layer
    {
        int depth;
        int range;

        public bool Hit(int delay = 0)
        {
            var period = 2 * (range - 1);
            var hit = (delay + depth) % period == 0;

            return hit;
        }

        public int Severity => depth * range;

        public static Layer Parse(string line)
        {
            var parts = line.Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

            var depth = int.Parse(parts[0]);
            var range = int.Parse(parts[1]);

            var result = new Layer
            {
                depth = depth,
                range = range
            };

            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var layers = input.Select(line => Layer.Parse(line)).ToList();

            var answer1 = layers.Where(l => l.Hit()).Sum(l => l.Severity);
            Console.WriteLine($"Answer 1: {answer1}");

            int delay = 0;
            while (layers.Any(l => l.Hit(delay)))
            {
                delay++;
            }

            var answer2 = delay;
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

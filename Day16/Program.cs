using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    abstract class Move
    {
        public abstract void Apply(List<char> line);

        protected void Swap(List<char> line, int index0, int index1)
        {
            var tmp = line[index0];
            line[index0] = line[index1];
            line[index1] = tmp;
        }

        public static Move Parse(string x)
        {
            switch (x[0])
            {
                case 's':
                    return Spin.Parse(x);
                case 'x':
                    return Exchange.Parse(x);
                case 'p':
                    return Partner.Parse(x);
            }

            throw new Exception("Parse error");
        }
    }

    class Spin : Move
    {
        int amount;

        public override void Apply(List<char> line)
        {
            var copy = new List<char>(line);

            for (int i = 0; i < line.Count; i++)
            {
                var newIndex = (i + amount) % line.Count;
                line[newIndex] = copy[i];
            }
        }

        public static new Spin Parse(string x)
        {
            var amount = int.Parse(x.Substring(1));

            return new Spin
            {
                amount = amount
            };
        }
    }

    class Exchange : Move
    {
        int index0;
        int index1;

        public override void Apply(List<char> line)
        {
            Swap(line, index0, index1);
        }

        public static new Exchange Parse(string x)
        {
            var parts = x.Substring(1).Split('/');

            var index0 = int.Parse(parts[0]);
            var index1 = int.Parse(parts[1]);

            return new Exchange
            {
                index0 = index0,
                index1 = index1
            };
        }
    }

    class Partner : Move
    {
        char name0;
        char name1;

        public override void Apply(List<char> line)
        {
            var index0 = line.IndexOf(name0);
            var index1 = line.IndexOf(name1);
            Swap(line, index0, index1);
        }

        public static new Partner Parse(string x)
        {
            var parts = x.Substring(1).Split('/');

            var name0 = parts[0][0];
            var name1 = parts[1][0];

            return new Partner
            {
                name0 = name0,
                name1 = name1
            };
        }
    }

    class LineComparer : IEqualityComparer<List<char>>
    {
        public bool Equals(List<char> x, List<char> y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(List<char> obj)
        {
            return obj[0] * obj[1];
        }
    }

    class Program
    {
        static List<char> GenerateLine(int count)
        {
            return Enumerable.Range(0, count).Select(i => (char)(i + 'a')).ToList();
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");

            var moves = input.Split(',').Select(x => Move.Parse(x)).ToList();

            int count = 16;
            var line = GenerateLine(count);

            foreach (var m in moves)
            {
                m.Apply(line);
            }

            var answer1 = new string(line.ToArray());
            Console.WriteLine($"Answer 1: {answer1}");

            
            var positionAfter = new Dictionary<int, List<char>>();

            var start = GenerateLine(count);
            line = start;
            do
            {
                positionAfter[positionAfter.Count] = line;

                line = new List<char>(line);
                foreach (var m in moves)
                {
                    m.Apply(line);
                }
            } while (!line.SequenceEqual(start));

            int dances = 1_000_000_000;
            var index = dances % positionAfter.Count;
            line = positionAfter[index];

            var answer2 = new string(line.ToArray());
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

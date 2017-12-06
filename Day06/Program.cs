using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day06
{
    class StateComparer : IEqualityComparer<State>
    {
        public bool Equals(State x, State y)
        {
            return x.items.SequenceEqual(y.items);
        }

        public int GetHashCode(State obj)
        {
            return 0;
        }
    }

    class State
    {
        public List<int> items;

        public State Next()
        {
            var value = items.Max();
            var index = items.IndexOf(value);

            var newItems = new List<int>(items);

            newItems[index] = 0;
            for (int i = 1; i <= value; i++)
            {

                newItems[(index + i) % items.Count]++;
            }

            return new State { items = newItems };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = "5	1	10	0	1	7	13	14	3	12	8	10	7	12	0	6";

            var items = input.Split('\t').Select(i => int.Parse(i)).ToList();

            var history = new Dictionary<State, int>(new StateComparer());

            int steps = 0;
            var current = new State { items = items };
            while (!history.ContainsKey(current))
            {
                history.Add(current, steps);
                current = current.Next();
                steps++;
            }

            var answer1 = steps;
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = steps - history[current];
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

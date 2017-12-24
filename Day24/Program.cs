using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24
{
    class Component
    {
        public List<int> ports;

        public bool visited;

        public int Strength() => ports.Sum();

        public int Other(int x)
        {
            if (ports.First() == x)
            {
                return ports.Last();
            }
            else
            {
                return ports.First();
            }
        }

        public static Component Parse(string x)
        {
            var parts = x.Split('/');

            return new Component
            {
                ports = parts.Select(p => int.Parse(p)).ToList()
            };
        }
    }

    class Bridge
    {
        public int length;
        public int strength;
    }

    class Program
    {
        enum Mode
        {
            Part1,
            Part2
        }

        static Bridge Max(Bridge a, Bridge b, Mode mode)
        {
            switch (mode)
            {
                case Mode.Part1:
                    if (a.strength >= b.strength)
                    {
                        return a;
                    }
                    else
                    {
                        return b;
                    }
                case Mode.Part2:
                    if (a.length > b.length)
                    {
                        return a;
                    }
                    else if (a.length < b.length)
                    {
                        return b;
                    }
                    else
                    {
                        return Max(a, b, Mode.Part1);
                    }
            }

            throw new Exception("Error");
        }

        static Bridge MaxStrength(Dictionary<int, List<Component>> componentLookup, int endport, Mode mode)
        {
            var possibleNeighbours = componentLookup[endport].Where(c => !c.visited).ToList();

            var max = new Bridge();

            foreach (var nb in possibleNeighbours)
            {
                nb.visited = true;

                var newEndport = nb.Other(endport);
                var end = MaxStrength(componentLookup, newEndport, mode);

                var current = new Bridge
                {
                    length = 1 + end.length,
                    strength = nb.Strength() + end.strength 
                };

                max = Max(max, current, mode);
                
                nb.visited = false;
            }

            return max;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var components = input.Select(line => Component.Parse(line)).ToList();

            var componentLookup = new Dictionary<int, List<Component>>();
            foreach (var c in components)
            {
                foreach (var p in c.ports)
                {
                    if (!componentLookup.ContainsKey(p))
                    {
                        componentLookup[p] = new List<Component>();
                    }

                    componentLookup[p].Add(c);
                }
            }

            var answer1 = MaxStrength(componentLookup, 0, Mode.Part1).strength;
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = MaxStrength(componentLookup, 0, Mode.Part2).strength;
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

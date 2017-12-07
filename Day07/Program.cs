using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    class Node
    {
        public string name;
        public int weight;

        public Node parent;
        public List<Node> children;

        public int totalWeight;

        public int CalculateTotalWeight()
        {
            totalWeight = weight + children.Sum(c => c.CalculateTotalWeight());
            return totalWeight;
        }

        public Node WrongNode()
        {
            if (children.Count == 0)
            {
                return this;
            }

            var min = children.Min(c => c.totalWeight);
            var max = children.Max(c => c.totalWeight);

            if (min == max)
            {
                return this;
            }

            Node wrongChild = null;
            if (children.Count(c => c.totalWeight == min) == 1)
            {
                wrongChild = children.Single(c => c.totalWeight == min);
            }
            else if (children.Count(c => c.totalWeight == max) == 1)
            {
                wrongChild = children.Single(c => c.totalWeight == max);
            }

            return wrongChild.WrongNode();
        }

        public int CorrectWeight()
        {
            var other = parent.children.First(c => c != this);

            int correctWeight = other.totalWeight - children.Sum(c => c.totalWeight);

            return correctWeight;
        }
    }

    class Program
    {
        static Node GetOrCreateNode(Dictionary<string, Node> nodes, string name)
        {
            if (!nodes.ContainsKey(name))
            {
                nodes.Add(name, new Node { name = name });
            }

            return nodes[name];
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var nodes = new Dictionary<string, Node>();

            foreach (var line in input)
            {
                var parts = line.Split(new char[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

                var name = parts[0];

                var node = GetOrCreateNode(nodes, name);

                node.weight = int.Parse(parts[1]);
                node.children = new List<Node>();

                var childNames = parts.Skip(3);

                foreach (var childName in childNames)
                {
                    var child = GetOrCreateNode(nodes, childName);
                    child.parent = node;
                    node.children.Add(child);
                }
            }

            var root = nodes.Values.Single(n => n.parent == null);

            var answer1 = root.name;
            Console.WriteLine($"Answer 1: {answer1}");

            root.CalculateTotalWeight();
            var wrong = root.WrongNode();
            var answer2 = wrong.CorrectWeight();
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

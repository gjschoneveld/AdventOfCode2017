using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    class Vertex
    {
        public int id;
        public List<Vertex> neighbours;

        public bool visited;
    }

    class Program
    {
        static Vertex GetOrCreateVertex(Dictionary<int, Vertex> vertices, int id)
        {
            if (!vertices.ContainsKey(id))
            {
                vertices.Add(id, new Vertex { id = id });
            }

            return vertices[id];
        }

        static void VisitConnectedVertices(Vertex member)
        {
            var toVisit = new Queue<Vertex>();
            toVisit.Enqueue(member);

            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                if (current.visited)
                {
                    continue;
                }

                var newNeighbours = current.neighbours.Where(nb => !nb.visited);
                foreach (var nnb in newNeighbours)
                {
                    toVisit.Enqueue(nnb);
                }

                current.visited = true;
            }
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var vertices = new Dictionary<int, Vertex>();

            foreach (var line in input)
            {
                var parts = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                var id = int.Parse(parts[0]);

                var vertex = GetOrCreateVertex(vertices, id);

                vertex.neighbours = new List<Vertex>();

                var neighbourIds = parts.Skip(2).Select(x => int.Parse(x));

                foreach (var neighbourId in neighbourIds)
                {
                    var nb = GetOrCreateVertex(vertices, neighbourId);
                    vertex.neighbours.Add(nb);
                }
            }

            VisitConnectedVertices(vertices[0]);

            var answer1 = vertices.Values.Count(v => v.visited);
            Console.WriteLine($"Answer 1: {answer1}");

            var groups = 1;
            foreach (var vertex in vertices.Values)
            {
                if (!vertex.visited)
                {
                    VisitConnectedVertices(vertex);
                    groups++;
                }
            }

            var answer2 = groups;
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

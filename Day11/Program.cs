using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    enum Direction
    {
        n,
        nw,
        ne,
        sw,
        se,
        s
    }

    class Location
    {
        public int x;
        public int y;

        public Location Neighbour(Direction dir)
        {
            switch (dir)
            {
                case Direction.n:
                    return new Location{ x = x, y = y - 2 };
                case Direction.nw:
                    return new Location { x = x - 1, y = y - 1 };
                case Direction.ne:
                    return new Location { x = x + 1, y = y - 1 };
                case Direction.sw:
                    return new Location { x = x - 1, y = y + 1 };
                case Direction.se:
                    return new Location { x = x + 1, y = y + 1 };
                case Direction.s:
                    return new Location { x = x, y = y + 2 };
            }

            throw new Exception("Something went wrong");
        }

        public List<Location> Neighbours() => Enum.GetValues(typeof(Direction)).Cast<Direction>().Select(d => Neighbour(d)).ToList();
    }

    class LocationComparer : IEqualityComparer<Location>
    {
        public bool Equals(Location a, Location b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public int GetHashCode(Location obj)
        {
            return obj.x * obj.y;
        }
    }

    class Grid
    {
        Dictionary<Location, int> distance;

        int maxDistance;
        List<Location> border;

        Location current;

        public Grid()
        {
            current = new Location();
            distance = new Dictionary<Location, int>(new LocationComparer()) { { current, 0 } };
            border = new List<Location> { current };
        }

        void Expand()
        {
            maxDistance++;

            var newBorder = border.SelectMany(l => l.Neighbours().Where(n => !distance.ContainsKey(n))).Distinct(new LocationComparer()).ToList();

            foreach(var l in newBorder)
            {
                distance.Add(l, maxDistance);
            }

            border = newBorder;
        }

        public void MoveTo(Direction dir)
        {
            var next = current.Neighbour(dir);

            if (!distance.ContainsKey(next))
            {
                Expand();
            }

            current = next;
        }

        public int Distance()
        {
            return distance[current];
        }

        public int MaxDistance()
        {
            return maxDistance;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");

            var directions = input.Split(',').Select(d => (Direction)Enum.Parse(typeof(Direction), d)).ToList();

            var grid = new Grid();
            foreach (var d in directions)
            {
                grid.MoveTo(d);
            }

            var answer1 = grid.Distance();
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = grid.MaxDistance();
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

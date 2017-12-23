using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day22
{
    enum Direction
    {
        Left,
        Up,
        Right,
        Down
    }

    class Location
    {
        public int x;
        public int y;

        public static Direction TurnLeft(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    return Direction.Down;
                case Direction.Up:
                    return Direction.Left;
                case Direction.Right:
                    return Direction.Up;
                case Direction.Down:
                    return Direction.Right;
            }

            throw new Exception("Error");
        }

        public static Direction TurnRight(Direction dir)
        {
            return TurnLeft(TurnLeft(TurnLeft(dir)));
        }

        public static Direction Reverse(Direction dir)
        {
            return TurnLeft(TurnLeft(dir));
        }

        public Location Step(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    return new Location { x = x - 1, y = y };
                case Direction.Up:
                    return new Location { x = x, y = y - 1 };
                case Direction.Right:
                    return new Location { x = x + 1, y = y };
                case Direction.Down:
                    return new Location { x = x, y = y + 1 };
            }

            throw new Exception("Error");
        }
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

    enum Status
    {
        Clean,
        Weakened,
        Infected,
        Flagged
    }

    class Grid
    {
        Dictionary<Location, Status> statuses;
        Location current;
        Direction direction;

        public int infections;

        private Status GetStatus(Location loc)
        {
            if (!statuses.ContainsKey(loc))
            {
                return Status.Clean;
            }

            return statuses[loc];
        }

        private void SetStatus(Location loc, Status status)
        {
            if (status == Status.Clean && statuses.ContainsKey(loc))
            {
                statuses.Remove(loc);
                return;
            }

            statuses[loc] = status;
        }

        public void BurstPart1()
        {
            if (GetStatus(current) == Status.Infected)
            {
                direction = Location.TurnRight(direction);
                SetStatus(current, Status.Clean);
            }
            else
            {
                direction = Location.TurnLeft(direction);
                SetStatus(current, Status.Infected);
                infections++;
            }

            current = current.Step(direction);
        }

        public void BurstPart2()
        {
            var status = GetStatus(current);

            switch (status)
            {
                case Status.Clean:
                    direction = Location.TurnLeft(direction);
                    SetStatus(current, Status.Weakened);
                    break;
                case Status.Weakened:
                    infections++;
                    SetStatus(current, Status.Infected);
                    break;
                case Status.Infected:
                    direction = Location.TurnRight(direction);
                    SetStatus(current, Status.Flagged);
                    break;
                case Status.Flagged:
                    direction = Location.Reverse(direction);
                    SetStatus(current, Status.Clean);
                    break;
            }

            current = current.Step(direction);
        }

        public static Grid Parse(string[] input)
        {
            var statuses = new Dictionary<Location, Status>(new LocationComparer()); 

            for (int y = 0; y < input.Length; y++)
            {
                var row = input[y];
                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x] == '#')
                    {
                        statuses.Add(new Location { x = x, y = y }, Status.Infected);
                    }
                }
            }

            var center = new Location { x = input[0].Length / 2, y = input.Length / 2 };

            return new Grid
            {
                statuses = statuses,
                current = center,
                direction = Direction.Up
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");


            var grid1 = Grid.Parse(input);

            int iterations1 = 10_000;
            for (int i = 0; i < iterations1; i++)
            {
                grid1.BurstPart1();
            }

            var answer1 = grid1.infections;
            Console.WriteLine($"Answer 1: {answer1}");


            var grid2 = Grid.Parse(input);

            int iterations2 = 10_000_000;
            for (int i = 0; i < iterations2; i++)
            {
                grid2.BurstPart2();
            }

            var answer2 = grid2.infections;
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

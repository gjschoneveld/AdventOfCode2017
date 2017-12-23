using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day20
{
    class Formula
    {
        public decimal a;
        public decimal b;
        public decimal c;

        private decimal Sqrt(decimal d)
        {
            var result = (decimal)Math.Sqrt((double)d);

            return result;
        }

        public List<decimal> CrossPoints(Formula other)
        {
            var difference = new Formula { a = a - other.a, b = b - other.b, c = c - other.c };
            return difference.Roots();
        }

        public List<decimal> Roots()
        {
            if (a != 0)
            {
                var roots = new List<decimal>();

                var d = b * b - 4 * a * c;
                if (d >= 0)
                {
                    var root = (-b + Sqrt(d)) / (2 * a);
                    roots.Add(root);
                }
                if (d > 0)
                {
                    var root = (-b - Sqrt(d)) / (2 * a);
                    roots.Add(root);
                }

                return roots;
            }
            else if (b != 0)
            {
                var root = -c / b;
                return new List<decimal> { root };
            }
            else if (c != 0)
            {
                return new List<decimal>();
            }

            return null;
        }
    }

    class Collision
    {
        public int time;
        public Particle particle;
    }

    class Particle
    {
        public int id;

        public List<int> p;
        public List<int> v;
        public List<int> a;

        public bool dead;
        private List<Collision> collisions;

        public int TotalVelocity()
        {
            return v.Sum(v => Math.Abs(v));
        }

        public int TotalAcceleration()
        {
            return a.Sum(a => Math.Abs(a));
        }

        public List<Formula> PositionFormulas()
        {
            var result = new List<Formula>();

            for (int i = 0; i < p.Count; i++)
            {
                result.Add(new Formula { a = a[i] / 2m, b = v[i] + (a[i] / 2m), c = p[i] });
            }

            return result;
        }

        public bool IsInt(decimal d)
        {
            return d == (int)d;
        }

        public int CollisionTime(Particle other)
        {
            var formulas = PositionFormulas();
            var otherFormulas = other.PositionFormulas();

            List<decimal> collisions = null;
            for (int i = 0; i < formulas.Count; i++)
            {
                var currentCollisions = formulas[i].CrossPoints(otherFormulas[i])?.Where(c => IsInt(c))?.ToList();
                if (collisions == null)
                {
                    collisions = currentCollisions;
                }
                else if (currentCollisions != null)
                {
                    collisions = collisions.Intersect(currentCollisions).ToList();
                }
            }

            if (collisions.Where(c => c >= 0).Count() == 0)
            {
                return -1;
            }

            return collisions.Where(c => c >= 0).Min(c => (int)c);
        }

        public void GatherCollisions(List<Particle> particles)
        {
            collisions = new List<Collision>();

            foreach (var p in particles)
            {
                if (p.id == id)
                {
                    continue;
                }

                var time = CollisionTime(p);
                if (time == -1)
                {
                    continue;
                }

                collisions.Add(new Collision {
                    particle = p,
                    time = time
                });
            }
        }

        public List<Collision> NextCollisions()
        {
            var alive = collisions.Where(c => !c.particle.dead).ToList();

            if (alive.Count == 0)
            {
                return new List<Collision>();
            }

            var minTime = collisions.Min(c => c.time);
            var next = collisions.Where(c => c.time == minTime).ToList();

            return next;
        }

        public static Particle Parse(int id, string x)
        {
            var parts = x.Split(new char[] { 'p', 'v', 'a', ' ', ',', '=', '<', '>' }, StringSplitOptions.RemoveEmptyEntries);

            return new Particle
            {
                id = id,
                p = new List<int> { int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]) },
                v = new List<int> { int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]) },
                a = new List<int> { int.Parse(parts[6]), int.Parse(parts[7]), int.Parse(parts[8]) },
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var particles = input.Select((line, index) => Particle.Parse(index, line)).ToList();


            var minAcceleration = particles.Min(p => p.TotalAcceleration());
            var minParticles = particles.Where(p => p.TotalAcceleration() == minAcceleration).ToList();

            var minVelocity = minParticles.Min(p => p.TotalVelocity());
            var minParticle = minParticles.Single(p => p.TotalVelocity() == minVelocity);

            var answer1 = minParticle.id;
            Console.WriteLine($"Answer 1: {answer1}");


            foreach (var p in particles)
            {
                p.GatherCollisions(particles);
            }

            var next = particles.SelectMany(p => p.NextCollisions()).ToList();
            while (next.Count > 0)
            {
                var time = next.Min(c => c.time);
                var current = next.Where(c => c.time == time).ToList();

                foreach (var c in current)
                {
                    c.particle.dead = true;
                }

                next = particles.SelectMany(p => p.NextCollisions()).ToList();
            }

            var answer2 = particles.Count(p => !p.dead);
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

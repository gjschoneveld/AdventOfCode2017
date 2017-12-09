using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day09
{
    class Lexer
    {
        string input;
        int index;

        public Lexer(string input)
        {
            this.input = input;
        }

        public char Next()
        {
            return input[index++];
        }

        public char Peek()
        {
            return input[index];
        }
    }

    abstract class Item
    {
        public abstract int CalculateScore(int x);

        public abstract int CountGarbage();

        public static Item Parse(Lexer l)
        {
            var c = l.Peek();

            if (c == '<')
            {
                return Garbage.Parse(l);
            }
            else if (c == '{')
            {
                return Group.Parse(l);
            }

            throw new Exception("Could not parse item");
        }
    }

    class Garbage : Item
    {
        int count;

        public override int CalculateScore(int x)
        {
            return 0;
        }

        public override int CountGarbage()
        {
            return count;
        }

        public static new Garbage Parse(Lexer l)
        {
            int count = -1; // -1 because > is also counted 

            var c = l.Next(); // consume <

            while (c != '>')
            {
                c = l.Next();

                if (c == '!')
                {
                    l.Next(); // consume char after !
                    continue;
                }

                count++;
            }

            return new Garbage { count = count };
        }
    }

    class Group : Item
    {
        List<Item> children = new List<Item>();

        public override int CalculateScore(int x)
        {
            return x + children.Sum(c => c.CalculateScore(x + 1));
        }

        public override int CountGarbage()
        {
            return children.Sum(c => c.CountGarbage());
        }

        public static new Group Parse(Lexer l)
        {
            var c = l.Next(); // consume {

            if (l.Peek() == '}')
            {
                l.Next(); // consume }
                return new Group();
            }

            var children = new List<Item>();

            while (c != '}')
            {
                var child = Item.Parse(l);
                children.Add(child);
                c = l.Next(); // either , or }
            }

            return new Group { children = children };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt");

            var item = Item.Parse(new Lexer(input));

            var answer1 = item.CalculateScore(1);
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = item.CountGarbage();
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

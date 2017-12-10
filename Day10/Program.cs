using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    class Hash
    {
        int current = 0;
        int skipSize = 0;

        List<int> list;
        List<int> lengths;

        public Hash(int itemCount, List<int> lengths)
        {
            list = Enumerable.Range(0, itemCount).ToList();
            this.lengths = lengths;
        }

        void Reverse(List<int> list, int start, int count)
        {
            var reversed = new List<int>();

            for (int i = 0; i < count; i++)
            {
                var index = (start + i) % list.Count;
                reversed.Add(list[index]);
            }

            reversed.Reverse();

            for (int i = 0; i < count; i++)
            {
                var index = (start + i) % list.Count;
                list[index] = reversed[i];
            }
        }

        void DoRound()
        {
            foreach (var len in lengths)
            {
                Reverse(list, current, len);
                current = (current + len + skipSize) % list.Count;
                skipSize += 1;
            }
        }

        public int Part1()
        {
            DoRound();
            return list[0] * list[1];
        }

        int Xor(List<int> items) => items.Aggregate((a, b) => a ^ b);

        List<int> Dense(List<int> items, int groups)
        {
            var result = new List<int>();

            int itemsPerGroup = items.Count / groups;

            for (int g = 0; g < groups; g++)
            {
                var inner = items.GetRange(g * itemsPerGroup, itemsPerGroup);
                int xor = Xor(inner);

                result.Add(xor);
            }

            return result;
        }

        string Hex(List<int> items) => string.Join("", items.Select(i => i.ToString("x2")));

        public string Part2()
        {
            var numberOfRounds = 64;
            for (int i = 0; i < numberOfRounds; i++)
            {
                DoRound();
            }

            var groups = 16;
            var dense = Dense(list, groups);
            var hex = Hex(dense);

            return hex;
        }
    }

    class Program
    {
        static List<int> ToAscii(string x)
        {
            var result = new List<int>();

            foreach (var c in x)
            {
                var ascii = (int)c;
                result.Add(ascii);
            }

            return result;
        }

        static void Main(string[] args)
        {
            string input = "212,254,178,237,2,0,1,54,167,92,117,125,255,61,159,164";

            int itemCount = 256;

            var lengths = input.Split(',').Select(l => int.Parse(l)).ToList();
            var hash1 = new Hash(itemCount, lengths);
            var answer1 = hash1.Part1();
            Console.WriteLine($"Answer 1: {answer1}");

            var ascii = ToAscii(input);
            var suffix = new List<int> { 17, 31, 73, 47, 23 };
            ascii.AddRange(suffix);
            var hash2 = new Hash(itemCount, ascii);
            var answer2 = hash2.Part2();
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

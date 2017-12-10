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

        int Xor(IEnumerable<int> items) => items.Aggregate((a, b) => a ^ b);

        IEnumerable<int> Dense(IEnumerable<int> items, int numberOfGroups)
        {
            var groups = items.Select((item, index) => new { item = item, group = index / numberOfGroups }).GroupBy(x => x.group, x => x.item);
            var dense = groups.Select(g => Xor(g));
            return dense;
        }

        string Hex(IEnumerable<int> items) => string.Join("", items.Select(i => i.ToString("x2")));

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
        static void Main(string[] args)
        {
            string input = "212,254,178,237,2,0,1,54,167,92,117,125,255,61,159,164";

            int itemCount = 256;

            var lengths = input.Split(',').Select(l => int.Parse(l)).ToList();
            var hash1 = new Hash(itemCount, lengths);
            var answer1 = hash1.Part1();
            Console.WriteLine($"Answer 1: {answer1}");

            var ascii = input.Select(c => (int)c).ToList();
            var suffix = new List<int> { 17, 31, 73, 47, 23 };
            ascii.AddRange(suffix);
            var hash2 = new Hash(itemCount, ascii);
            var answer2 = hash2.Part2();
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

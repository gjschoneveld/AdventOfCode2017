using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day04
{
    class Program
    {
        static bool IsValid1(IEnumerable<string> line)
        {
            return line.Count() == line.Distinct().Count();
        }

        static string SortLetters(string word)
        {
            return new string(word.OrderBy(c => c).ToArray());
        }

        static bool IsValid2(IEnumerable<string> line)
        {
            return IsValid1(line.Select(w => SortLetters(w)));
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var rows = input.Select(line => line.Split(' '));

            var answer1 = rows.Count(r => IsValid1(r));
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = rows.Count(r => IsValid2(r));
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

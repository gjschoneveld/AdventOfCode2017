using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day08
{
    class RegisterFile
    {
        Dictionary<string, int> storage = new Dictionary<string, int>();

        int maximumSeen = 0;

        public int GetValue(string name)
        {
            if (storage.ContainsKey(name))
            {
                return storage[name];
            }

            return 0;
        }

        public void SetValue(string name, int value)
        {
            if (!storage.ContainsKey(name))
            {
                storage.Add(name, value);
            }
            else
            {
                storage[name] = value;
            }

            maximumSeen = Math.Max(maximumSeen, value);
        }

        public int MaximumValue()
        {
            return storage.Values.Max();
        }

        public int MaximumSeen()
        {
            return maximumSeen;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var registerFile = new RegisterFile();

            foreach (var line in input)
            {
                var parts = line.Split();

                var target = parts[0];
                var action = parts[1];
                var amount = int.Parse(parts[2]);

                var source = parts[4];
                var operand = parts[5];
                var constant = int.Parse(parts[6]);

                var sourceValue = registerFile.GetValue(source);

                bool comparisonResult = false;
                switch (operand)
                {
                    case "==":
                        comparisonResult = sourceValue == constant;
                        break;
                    case "!=":
                        comparisonResult = sourceValue != constant;
                        break;
                    case "<":
                        comparisonResult = sourceValue < constant;
                        break;
                    case ">":
                        comparisonResult = sourceValue > constant;
                        break;
                    case "<=":
                        comparisonResult = sourceValue <= constant;
                        break;
                    case ">=":
                        comparisonResult = sourceValue >= constant;
                        break;
                }

                if (!comparisonResult)
                {
                    continue;
                }

                var targetValue = registerFile.GetValue(target);

                switch (action)
                {
                    case "inc":
                        targetValue += amount;
                        break;
                    case "dec":
                        targetValue -= amount;
                        break;
                }

                registerFile.SetValue(target, targetValue);
            }

            var answer1 = registerFile.MaximumValue();
            Console.WriteLine($"Answer 1: {answer1}");

            var answer2 = registerFile.MaximumSeen();
            Console.WriteLine($"Answer 1: {answer2}");

            Console.ReadKey();
        }
    }
}

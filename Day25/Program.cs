using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day25
{
    class State
    {
        public string name;

        public List<int> nextValue;
        public List<int> nextOffset;
        public List<string> nextState;

        public static State Parse(List<string> lines)
        {
            var nameParts = lines[0].Split(' ', ':');
            var name = nameParts[2];

            var valueParts0 = lines[2].Trim().Split(' ', '.');
            var value0 = int.Parse(valueParts0[4]);

            var directionParts0 = lines[3].Trim().Split(' ', '.');
            var direction0 = directionParts0[6];
            var offset0 = direction0 == "left" ? -1 : 1;

            var stateParts0 = lines[4].Trim().Split(' ', '.');
            var state0 = stateParts0[4];

            var valueParts1 = lines[6].Trim().Split(' ', '.');
            var value1 = int.Parse(valueParts1[4]);

            var directionParts1 = lines[7].Trim().Split(' ', '.');
            var direction1 = directionParts1[6];
            var offset1 = direction1 == "left" ? -1 : 1;

            var stateParts1 = lines[8].Trim().Split(' ', '.');
            var state1 = stateParts1[4];

            return new State
            {
                name = name,
                nextValue = new List<int> { value0, value1 },
                nextOffset = new List<int> { offset0, offset1 },
                nextState = new List<string> { state0, state1 },
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var startParts = input[0].Split(' ', '.');
            var stateName = startParts[3];

            var iterationParts = input[1].Split();
            var iterations = int.Parse(iterationParts[5]);

            var states = new List<State>();
            for (int i = 3; i < input.Length; i += 10)
            {
                states.Add(State.Parse(input.Skip(i).Take(9).ToList()));
            }


            var ones = new HashSet<int>();

            int position = 0;
            for (int i = 0; i < iterations; i++)
            {
                var state = states.First(s => s.name == stateName);

                int value = ones.Contains(position) ? 1 : 0;

                var nextValue = state.nextValue[value];
                if (nextValue == 0 && ones.Contains(position))
                {
                    ones.Remove(position);
                }
                else if (nextValue == 1 && !ones.Contains(position))
                {
                    ones.Add(position);
                }

                position += state.nextOffset[value];
                stateName = state.nextState[value];
            }

            var answer = ones.Count;
            Console.WriteLine($"Answer: {answer}");

            Console.ReadKey();
        }
    }
}

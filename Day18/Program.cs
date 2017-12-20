using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    abstract class Instruction
    {
        protected Value x;
        protected Value y;

        public abstract long Execute(Processor proc);

        public static Instruction Parse(string line)
        {
            var parts = line.Split();

            var opcode = parts[0];

            var x = parts.Length > 1 ? Value.Parse(parts[1]) : null;
            var y = parts.Length > 2 ? Value.Parse(parts[2]) : null;

            switch (opcode)
            {
                case "snd":
                    return new Snd { x = x };
                case "set":
                    return new Set { x = x, y = y };
                case "add":
                    return new Add { x = x, y = y };
                case "mul":
                    return new Mul { x = x, y = y };
                case "mod":
                    return new Mod { x = x, y = y };
                case "rcv":
                    return new Rcv { x = x };
                case "jgz":
                    return new Jgz { x = x, y = y };
            }

            throw new Exception("Parse error");
        }
    }

    class Snd : Instruction
    {
        // snd X plays a sound with a frequency equal to the value of X.
        public override long Execute(Processor proc)
        {
            switch (proc.mode)
            {
                case Mode.Part1:
                    proc.sound = x.Get(proc);
                    break;
                case Mode.Part2:
                    proc.output.Enqueue(x.Get(proc));
                    proc.count++;
                    break;
            }

            return 1;
        }
    }

    class Set : Instruction
    {
        // set X Y sets register X to the value of Y.
        public override long Execute(Processor proc)
        {
            x.Set(proc, y.Get(proc));
            return 1;
        }
    }

    class Add : Instruction
    {
        // add X Y increases register X by the value of Y.
        public override long Execute(Processor proc)
        {
            x.Set(proc, x.Get(proc) + y.Get(proc));
            return 1;
        }
    }

    class Mul : Instruction
    {
        // mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
        public override long Execute(Processor proc)
        {
            x.Set(proc, x.Get(proc) * y.Get(proc));
            return 1;
        }
    }

    class Mod : Instruction
    {
        // mod X Y sets register X to the remainder of dividing the value contained in register X by the value of Y (that is, it sets X to the result of X modulo Y).
        public override long Execute(Processor proc)
        {
            x.Set(proc, x.Get(proc) % y.Get(proc));
            return 1;
        }
    }

    class Rcv : Instruction
    {
        // rcv X recovers the frequency of the last sound played, but only when the value of X is not zero. (If it is zero, the command does nothing.)
        public override long Execute(Processor proc)
        {
            switch (proc.mode)
            {
                case Mode.Part1:
                    if (x.Get(proc) != 0)
                    {
                        proc.halt = true;
                    }
                    break;
                case Mode.Part2:
                    if (proc.input.Count == 0)
                    {
                        proc.halt = true;
                        return 0;
                    }

                    x.Set(proc, proc.input.Dequeue());
                    break;
            }

            return 1;
        }
    }

    class Jgz : Instruction
    {
        // jgz X Y jumps with an offset of the value of Y, but only if the value of X is greater than zero. (An offset of 2 skips the next instruction, an offset of -1 jumps to the previous instruction, and so on.)
        public override long Execute(Processor proc)
        {
            if (x.Get(proc) > 0)
            {
                return y.Get(proc);
            }

            return 1;
        }
    }

    abstract class Value
    {
        public abstract long Get(Processor proc);

        public abstract void Set(Processor proc, long value);

        public static Value Parse(string x)
        {
            var first = x[0];

            if (char.IsLetter(first))
            {
                return new Register { name = first };
            }
            else
            {
                return new Constant { value = long.Parse(x) };
            }
        }
    }

    class Constant : Value
    {
        public long value;

        public override long Get(Processor proc)
        {
            return value;
        }

        public override void Set(Processor proc, long value)
        {
            throw new NotImplementedException();
        }
    }

    class Register : Value
    {
        public char name;

        public override long Get(Processor proc)
        {
            if (!proc.registerFile.ContainsKey(name))
            {
                proc.registerFile[name] = 0;
            }

            return proc.registerFile[name];
        }

        public override void Set(Processor proc, long value)
        {
            proc.registerFile[name] = value;
        }
    }

    enum Mode
    {
        Part1,
        Part2
    }

    class Processor
    {
        public Mode mode;

        public List<Instruction> program;

        public Dictionary<char, long> registerFile = new Dictionary<char, long>();

        public long sound;
        public bool halt;

        private int programCounter;

        public Queue<long> input;
        public Queue<long> output;
        public int count;

        public Queue<long> Run(Queue<long> input = null)
        {
            this.input = input ?? new Queue<long>();
            output = new Queue<long>();

            halt = false;
            while (!halt && 0 <= programCounter && programCounter < program.Count)
            {
                var offset = (int)program[programCounter].Execute(this);
                programCounter += offset;
            }

            return output;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var program = input.Select(line => Instruction.Parse(line)).ToList();

            var processor = new Processor { mode = Mode.Part1, program = program };
            processor.Run();

            var answer1 = processor.sound;
            Console.WriteLine($"Answer 1: {answer1}");


            var procs = new List<Processor> {
                new Processor { mode = Mode.Part2, program = program },
                new Processor { mode = Mode.Part2, program = program },
            };

            for (int i = 0; i < procs.Count; i++)
            {
                procs[i].registerFile['p'] = i;
            }

            var buffer = procs[0].Run();
            var activeProcessor = 1;
            do
            {
                buffer = procs[activeProcessor].Run(buffer);
                activeProcessor = (activeProcessor + 1) % procs.Count;
            } while (buffer.Count > 0);

            var answer2 = procs[1].count;
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

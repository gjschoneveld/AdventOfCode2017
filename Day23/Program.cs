using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day23
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
                case "set":
                    return new Set { x = x, y = y };
                case "sub":
                    return new Sub { x = x, y = y };
                case "mul":
                    return new Mul { x = x, y = y };
                case "jnz":
                    return new Jnz { x = x, y = y };
                case "mod":
                    return new Mod { x = x, y = y };
            }

            throw new Exception("Parse error");
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

    class Sub : Instruction
    {
        // sub X Y decreases register X by the value of Y.
        public override long Execute(Processor proc)
        {
            x.Set(proc, x.Get(proc) - y.Get(proc));
            return 1;
        }
    }

    class Mul : Instruction
    {
        // mul X Y sets register X to the result of multiplying the value contained in register X by the value of Y.
        public override long Execute(Processor proc)
        {
            x.Set(proc, x.Get(proc) * y.Get(proc));
            proc.multiplications++;
            return 1;
        }
    }

    class Jnz : Instruction
    {
        // jnz X Y jumps with an offset of the value of Y, but only if the value of X is not zero. (An offset of 2 skips the next instruction, an offset of -1 jumps to the previous instruction, and so on.)
        public override long Execute(Processor proc)
        {
            if (x.Get(proc) != 0)
            {
                return y.Get(proc);
            }

            return 1;
        }
    }

    class Mod : Instruction
    {
        // mod X Y sets register X to the remainder of dividing the value contained in register X by the value of Y (that is, it sets X to the result of X modulo Y).
        public override long Execute(Processor proc)
        {
            x.Set(proc, x.Get(proc) % y.Get(proc));
            proc.multiplications++;
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

    class Processor
    {
        public List<Instruction> program;

        public Dictionary<char, long> registerFile = new Dictionary<char, long>();

        public int multiplications;

        private int programCounter;

        public void Run()
        {
            while (0 <= programCounter && programCounter < program.Count)
            {
                var offset = (int)program[programCounter].Execute(this);
                programCounter += offset;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var program = input.Select(line => Instruction.Parse(line)).ToList();


            var processor = new Processor { program = program };
            processor.Run();

            var answer1 = processor.multiplications;
            Console.WriteLine($"Answer 1: {answer1}");


            var replacementLocation = 10;
            var replacement = new string[]
            {
                "set e b",
                "mod e d",
                "jnz e 2",
                "set f 0",
                "jnz 1 6"
            };

            for (int i = 0; i < replacement.Length; i++)
            {
                program[replacementLocation + i] = Instruction.Parse(replacement[i]);
            }

            processor = new Processor { program = program };
            processor.registerFile['a'] = 1;
            processor.Run();

            var answer2 = processor.registerFile['h'];
            Console.WriteLine($"Answer 2: {answer2}");

            Console.ReadKey();
        }
    }
}

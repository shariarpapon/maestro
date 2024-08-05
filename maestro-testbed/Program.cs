using Everime.Maestro;
using System;
using System.IO;

namespace MaestroTestbed
{
    public class Program
    {
        sealed class IO : IMaestroIOHandler
        {
            public void ClearInput()
            {
                Console.Clear();
            }

            public string Read()
            {
                return Console.ReadLine();
            }

            public void Write(string output)
            {
                Console.WriteLine(output);
            }
        }


        public static void Main(string[] args)
        {
            Console.WriteLine("### Initiating Maestro Terminal Test ###");

            IMaestroCommand[] commands = new IMaestroCommand[]
            {
                new CMD_PrintInput(),
                new CMD_CopyFile()
            };

            MaestroConfigurations config = MaestroConfigurations.Create(new IO(), commands);
            var terminal = new MaestroTerminal(config);
            while (true)
            {
                terminal.ScanInput();
            }
        }

        class CMD_PrintInput : IMaestroCommand
        {
            public string Keyword => "input";

            public uint MinimumArgumentCount => 1;

            public string Description => "Prints all the inputs";

            public bool Execute(MaestroTerminal terminal, string[] args)
            {
                foreach (string arg in args)
                    Console.WriteLine(arg);
                return true;
            }
        }

        class CMD_CopyFile : IMaestroCommand
        {
            public string Keyword => "copy";

            public uint MinimumArgumentCount => 2;

            public bool Execute(MaestroTerminal terminal, string[] args)
            {
                string source = args[0];
                string dest = args[1];
                if (source == dest)
                {
                    Console.WriteLine("File with same name already exists in the destination path.");
                    return false;
                }

                File.Copy(source, dest);
                return true;
            }
        }
    }
}
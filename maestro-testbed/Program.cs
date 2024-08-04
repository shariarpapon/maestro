using Everime.Maestro;

namespace MaestroTestbed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("### Initiating Maestro Terminal Test ###");

            IOProvider ioProvider = new IOProvider(Console.ReadLine!, Console.WriteLine, Console.Clear);

            IMaestroCommand[] commands = new IMaestroCommand[]
            {
                new CMD_PrintInput(),
                new CMD_CopyFile()
            };

            MaestroConfigurations config = MaestroConfigurations.Create(ioProvider, commands)
                                                                  .SetHelpKeyword("help")
                                                                  .SetPrintCommandExecutionResults(true);
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
                    terminal.IoProvider.Output(arg);
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
                    terminal.IoProvider.Output("File with same name already exists in the destination path.");
                    return false;
                }

                File.Copy(source, dest);
                return true;
            }
        }
    }
}
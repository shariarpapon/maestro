using Maestro;

public static class Program 
{
    public static void Main() 
    {
        RWProvider rwProvider = new RWProvider(Console.Write, Console.WriteLine, Console.ReadLine!);
        List<CommandAction> cmds = new List<CommandAction>();
        cmds.AddRange(MaestroTerminal.DEFAULT_COMMAND_ACTIONS);
        cmds.Add(new Command_Gen());
        MaestroTerminal.InitiateThread("maestro testbed", rwProvider, cmds.ToArray());
    }

    private class Command_Gen: CommandAction
    {
        public override string Keyword => "gen";
        public override uint RequiredArgCount => 2;
        public override string Description =>
            "Generates a documentation file from the source file and saves it to the given output path.\n" +
            "1: source file path\n" +
            "2: output file path";

        public override bool Invoke(object invoker, string[] args) 
        {
            try
            {
                string sourcePath = args[0];
                string outputPath = args[1];
                string dir = Path.GetDirectoryName(outputPath)!;
                if (!File.Exists(sourcePath))
                {
                    BuiltInMessages.FileNotFoundError(sourcePath);
                    return false;
                }
                else if (!Directory.Exists(dir)) 
                {
                    BuiltInMessages.DirectoryNotFoundError(dir);
                    return false;
                }

                string source = File.ReadAllText(sourcePath);
                string output = "_TEST_MAESTRO_OUTPUT_\n" + source;
                File.WriteAllText(outputPath, output);
                MaestroLogger.PrintInfo("doc generation successful", Keyword);
                return true;
            }
            catch 
            {
                MaestroLogger.PrintError("unable to execute command", Keyword);
                return false;
            }
        }
    }
}
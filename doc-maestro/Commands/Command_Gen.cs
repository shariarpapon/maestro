namespace Maestro.Commands
{
    internal sealed class Command_Gen : CommandAction
    {
        internal override string Keyword => "gen";
        internal override uint RequiredArgCount => 2;

        internal override bool Invoke(object invoker, string[] args)
        {
            string sourcePath = args[1];
            string outputPath = args[2];
            if (!File.Exists(sourcePath))
            {
                MaestroLogger.PrintError(BuiltInMessages.FileNotFoundError, sourcePath);
                return false;
            }

            string dir = Path.GetDirectoryName(outputPath)!;
            if (!Directory.Exists(dir))
            {
                MaestroLogger.PrintError(BuiltInMessages.DirectoryNotFound, dir);
                return false;
            }

            return true;
        }
    }
}

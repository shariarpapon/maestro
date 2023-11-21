using Maestro.InternalErrors;

namespace Maestro.Core
{
    internal static class CommandManager
    {
        private const char COMMAND_DELIMITER = '&';
        private const char ARGUMENT_DELIMITER = ' ';

        private static readonly Dictionary<string, CommandAction> CommandActions = new Dictionary<string, CommandAction>()
        {
            { "gen", new(3) },
            { "cls", new(1) },
            { "help", new(1) },
        };

        internal static bool ParseAndExecute(string source, MaestroTerminal terminal)
        {
            MaestroParser parser = new MaestroParser(source, COMMAND_DELIMITER, ARGUMENT_DELIMITER);

            string[] statements = parser.ParseStatements();
            for (int i = 0; i < statements.Length; i++)
            {
                string statement = statements[i];
                string[] args = parser.ParseArguments(statement);
                if (args == null || args.Length <= 0)
                {
                    MaestroLogger.Print(new InvalidKeywordError(statement));
                    return false;
                }

                string keyword = args[0];
                if (!CommandActions.ContainsKey(keyword))
                {
                    MaestroLogger.Print(new InvalidKeywordError(keyword));
                    return false;
                }

                int argCount = args.Length;
                CommandAction info = CommandActions[keyword];
                if (argCount > info.RequiredArgCount)
                {
                    MaestroLogger.Print(new ArgumentOverflowError(keyword));
                    return false;
                }
                else if (argCount < info.RequiredArgCount)
                {
                    MaestroLogger.Print(new InsufficientArgumentsError(keyword));
                    return false;
                }

                if (!Execute(keyword, args, terminal))
                    return false;                
            }
            return true;
        }

        private static bool Execute(string cmd, string[] args, MaestroTerminal commandTerminal) 
        {
            MaestroLogger.Print(CommandExecutionSuccessMessage(cmd));
            return true;
        }

        private static MaestroMessage CommandExecutionSuccessMessage(string cmd)
            => new MaestroMessage("command successfully executed", cmd, ConsoleColor.DarkGray, default);
    }
}

namespace Maestro.Commands
{
    internal static class MaestroCommandHandler
    {
        private const char COMMAND_DELIMITER = '&';
        private const char ARGUMENT_DELIMITER = ' ';

        private static readonly Dictionary<string, CommandAction> CommandActions = new Dictionary<string, CommandAction>()
        {
            { "cls",  new Command_CLS() },
            { "exit", new Command_Exit() }
        };

        internal static bool ParseAndExecute(string source, object invokeAuthorizer)
        {
            MaestroParser parser = new MaestroParser(source, COMMAND_DELIMITER, ARGUMENT_DELIMITER);

            string[] statements = parser.ParseStatements();
            for (int i = 0; i < statements.Length; i++)
            {
                string statement = statements[i];
                string[] args = parser.ParseArguments(statement);
                if (args == null || args.Length < 0)
                {
                    MaestroLogger.PrintError(BuiltInMessages.NoValidCommandKeywordError, statement);
                    return false;
                }

                string keyword = args[0];
                if (!CommandActions.ContainsKey(keyword))
                {
                    MaestroLogger.PrintError(BuiltInMessages.InvalidKeywordError, keyword);
                    return false;
                }

                //Minus 1 because the command keyword itself is inlcuded int he array.
                int argCount = args.Length - 1;
                CommandAction action = CommandActions[keyword];
                if (argCount > action.RequiredArgCount)
                {
                    MaestroLogger.PrintError(BuiltInMessages.ArgumentOverflowError, keyword);
                    return false;
                }
                else if (argCount < action.RequiredArgCount)
                {
                    MaestroLogger.PrintError(BuiltInMessages.InsufficientArgumentsError, keyword);
                    return false;
                }

                if (!Execute(invokeAuthorizer, action, args))
                    return false;
            }
            return true;
        }

        private static bool Execute(object invokeAuthorizer, CommandAction action, string[] args)
        {
            if (action.Invoke(invokeAuthorizer, args))
                MaestroLogger.PrintInfo("command executed", args[0]);

            return true;
        }

    }
}

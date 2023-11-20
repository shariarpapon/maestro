using DocMaestro.ErrorHandling;

namespace DocMaestro.Core.Commands
{
    internal static class CommandManager
    {
        private const string COMMAND_DELIMITER = "\0\t\r\b\n ";
        private const string COMMAND_SEPERATOR = "&";
        private const string ARGUMENT_SEPERATOR = ";";

        private enum MaestroCommand
        {
            gen,
            help,
            cls,
        }

        private static readonly Dictionary<string, MaestroCommandDefinition> Commands = new Dictionary<string, MaestroCommandDefinition>()
        {
            { MaestroCommand.gen.ToString(), new(2) },
            { MaestroCommand.help.ToString(), new(0) },
            { MaestroCommand.cls.ToString(), new(0) },
        };

        //TODO: Fix command and argument parsing
        internal static bool TryParseAndExecute(string input, out MaestroMessage[] output)
        {
            string[] cmdArray = input.Split(COMMAND_SEPERATOR);
            output = new MaestroMessage[cmdArray.Length];
            for (int i = 0; i < cmdArray.Length; i++)
            {
                string cmd = cmdArray[i];
                string cmdKeyword = PraseCommandKeyword(cmd);
                if (!Commands.ContainsKey(cmdKeyword))
                    return ErrorHandler.PushError(new InternalErrors.InvalidCommandError(cmdKeyword));

                MaestroCommandDefinition cmdDefinition = Commands[cmdKeyword];
                string[] args = null!;
                if (cmdDefinition.argumentCount > 0)
                {
                    args = cmd.Split(ARGUMENT_SEPERATOR);
                    if (args.Length > cmdDefinition.argumentCount)
                        return ErrorHandler.PushError(new InternalErrors.ArgumentOverflowError(cmd));
                    else if (args.Length < cmdDefinition.argumentCount)
                        return ErrorHandler.PushError(new InternalErrors.InsufficientArgumentError(cmd));
                }
                if (!Execute(cmd, args, out output[i]))
                    return false;
                
            }
            return true;
        }

        private static string PraseCommandKeyword(string input) 
        {
            string buffer = "";
            for (int i = 0; i < input.Length; i++) 
            {
                if (COMMAND_DELIMITER.Contains(input[i]))
                    break;
                buffer += input[i];
            }
            return buffer;
        }

        private static bool Execute(string cmd, string[] args, out MaestroMessage output) 
        {
            output = new MaestroMessage($"successfully executed <{cmd}>", ConsoleColor.Yellow, default);
            return true;
        }

    }
}

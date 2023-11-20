using Maestro.Core;

namespace Maestro.Core.Commands
{
    internal static class CommandManager
    {
        private const string COMMAND_DELIMITER = "\0\t\r\b\n ";
        private const string COMMAND_SEPERATOR = "&";
        private const string ARGUMENT_SEPERATOR = ";";

        private enum CommandKeywords
        {
            gen,
            help,
            cls,
        }

        private static readonly Dictionary<string, CommandInfo> Commands = new Dictionary<string, CommandInfo>()
        {
            { CommandKeywords.gen.ToString(), new(2) },
            { CommandKeywords.help.ToString(), new(0) },
            { CommandKeywords.cls.ToString(), new(0) },
        };

        //TODO: Fix command and argument parsing
        internal static bool ParseAndExecute(string input, MaestroTerminal commandTerminal)
        {
            string[] cmdArray = input.Split(COMMAND_SEPERATOR);
            for (int i = 0; i < cmdArray.Length; i++)
            {
                string cmd = cmdArray[i];
                string cmdKeyword = PraseCommandKeyword(cmd);
                if (!Commands.ContainsKey(cmdKeyword))
                {
                    commandTerminal.PushMessage(new InternalErrors.InvalidCommandError(cmdKeyword));
                    return false;
                }
                CommandInfo info = Commands[cmdKeyword];
                string[] args = null!;
                if (info.argumentCount > 0)
                {
                    args = cmd.Split(ARGUMENT_SEPERATOR);
                    if (args.Length > info.argumentCount)
                    {
                        commandTerminal.PushMessage(new InternalErrors.ArgumentOverflowError(cmdKeyword));
                        return false;
                    }
                    else if (args.Length < info.argumentCount)
                    {
                        commandTerminal.PushMessage(new InternalErrors.InsufficientArgumentError(cmdKeyword));
                        return false;
                    }

                }
                if (!Execute(cmd, args, commandTerminal))
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

        private static bool Execute(string cmd, string[] args, MaestroTerminal commandTerminal) 
        {
            commandTerminal.PushMessage(CommandExecutionSuccessMessage(cmd));
            return true;
        }

        private static MaestroMessage CommandExecutionSuccessMessage(string cmd)
            => new MaestroMessage($"successfully executed <{cmd}>", ConsoleColor.Green, default);
    }
}

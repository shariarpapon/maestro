namespace Everime.Maestro
{
    internal sealed class CommandHandler
    {
        private Dictionary<string, IMaestroCommand> _commands;
        private MaestroTerminal _terminal;

        /// <summary>
        /// Handles parsing and executing the commands.
        /// </summary>
        /// <param name="commands">The command definitons for this terminal.</param>
        /// <exception cref="System.Exception"></exception>
        internal CommandHandler(MaestroTerminal terminal, IEnumerable<IMaestroCommand> commands) 
        {
            _terminal = terminal;
            _commands = new Dictionary<string, IMaestroCommand>();
            foreach (IMaestroCommand cmd in commands)
            {
                if (_commands.ContainsKey(cmd.Keyword))
                    throw new System.Exception($"More than one command with the keyword <{cmd.Keyword}>");
                _commands.Add(cmd.Keyword, cmd);
            }
        }

        internal CommandExecutionResult Execute(ParsedCommand parsedCommand) 
        {
            try
            {
                IMaestroCommand command = null!;
                if (_commands.ContainsKey(parsedCommand.keyword))
                {
                    command = _commands[parsedCommand.keyword];
                    if (parsedCommand.argumentCount < command.MinimumArgumentCount)
                        return new CommandExecutionResult(CommandExecutionStatus.InvalidArgumentCount, parsedCommand, command);
                    else if (command.Execute(_terminal, parsedCommand.arguments))
                        return new CommandExecutionResult(CommandExecutionStatus.Successful, parsedCommand, command);
                    else
                        return new CommandExecutionResult(CommandExecutionStatus.FailedExecution, parsedCommand, command);
                }
                else
                {
                    return new CommandExecutionResult(CommandExecutionStatus.KeywordNotFound, parsedCommand, null!);
                }
            }
            catch (Exception exception)
            {
                return new CommandExecutionResult(CommandExecutionStatus.FatalError, parsedCommand, null!, exception);
            }
        }

        internal CommandExecutionResult[] Execute(ParsedCommand[] parsedCommand)
        {
            CommandExecutionResult[] results = new CommandExecutionResult[parsedCommand.Length];
            for (int i = 0; i < parsedCommand.Length; i++)
                results[i] = Execute(parsedCommand[i]);
            return results;
        }
    }
}

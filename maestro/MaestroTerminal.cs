using System.Text;

namespace Everime.Maestro
{
    /// <summary>
    /// Maestro terminal environment. 
    /// <br>Handles all the terminal logic.</br>
    /// </summary>
    public sealed class MaestroTerminal
    {
        /// <summary>
        /// A globally unique identifier for this terminal.
        /// </summary>
        public readonly string GUID;
        public IOProvider IoProvider => _configurations.ioProvider;
        public IEnumerable<IMaestroCommand> Commands => _configurations.commands;

        private MaestroConfigurations _configurations;
        private MaestroParser _parser;
        private CommandHandler _commandHandler;

        /// <summary>
        /// Initialize the terminal with the given configurations.
        /// </summary>
        public MaestroTerminal(MaestroConfigurations configurations)
        {
            GUID = GenerateUID();
            _configurations = configurations;
            _parser = new MaestroParser(configurations.parsingSymbols);
            _commandHandler = new CommandHandler(this, configurations.commands);
        }

        /// <summary>
        /// Scans the terminal input for input submission character (default: <b>\n</b>).
        /// <br>Upon submission, the input will be cleared and scanned for valid commands and executed if any were found.</br>
        /// </summary>
        public void ScanInputSubmission(char submissionChar = '\n') 
        {
            string input = IoProvider.Input();
            if (string.IsNullOrEmpty(input))
                return;
            if (input[input.Length - 1] == submissionChar)
            {
                IoProvider.ClearInput();
                Scan(input);
            }
        }

        /// <summary>
        /// Scans the terminal input for any valid commands and executes them if found.
        /// <br>Does not wait for input submission character.</br>
        /// </summary>
        public void ScanInput() 
        {
            Scan(IoProvider.Input());
        }

        /// <summary>
        /// Scans the given input string for any valid commands and executes them if found.
        /// <br>Does not wait for input submission character.</br>
        /// </summary>
        /// <param name="source">The source string to be scanned.</param>
        public void Scan(string source) 
        {
            if (!string.IsNullOrEmpty(_configurations.helpKeyword) && source == _configurations.helpKeyword)
            {
                IoProvider.Output(GetCommandInformation());
                return;
            }

            ParserOutput parserOutput = _parser.Parse(source);
            if (parserOutput.status != ParseStatus.Successful)
                return;

            CommandExecutionResult[] results = _commandHandler.Execute(parserOutput.commands);

            foreach (CommandExecutionResult result in results)
            {
                if (_configurations.printCommandExecutionResult)
                    PrintCommandExecutionResult(result);

                _configurations.onCommandExecutedCallback?.Invoke(result);
            }
        }

        /// <summary>
        /// Prints out information of all the commands as defined.
        /// </summary>
        /// <returns>Neatly formatted command information.</returns>
        public string GetCommandInformation() 
        {
            if (Commands.Count() <= 0)
                return string.Empty;

            StringBuilder buffer = new StringBuilder("COMMANDS\n\n");
            string space = "    ";
            foreach (var cmd in Commands) 
            {
                buffer.AppendLine($"{space}Command: {cmd.Keyword}");
                buffer.AppendLine($"{space}Minimum Arguments: {cmd.MinimumArgumentCount}");
                if (cmd is ICommandDescriptionProvider) 
                    buffer.AppendLine($"{space}{((ICommandDescriptionProvider)cmd).Description}");                
                buffer.AppendLine("---");
            }
            return buffer.ToString();
        }

        /// <summary>
        /// Updates the parsing symbols to the given one.
        /// </summary>
        public void UpdateParsingSymbols(ParsingSymbols parsingSymbols) 
        {
            _parser = new MaestroParser(parsingSymbols);
        }
        
        private void PrintCommandExecutionResult(CommandExecutionResult result) 
        {
            switch (result.executionStatus)
            {
                case CommandExecutionStatus.Successful:
                    IoProvider.Output($"<{result.parsedCommand.keyword}> executed succesfully.");
                    break;
                case CommandExecutionStatus.FailedExecution:
                    IoProvider.Output($"<{result.parsedCommand.keyword}> execution failed.");
                    break;
                case CommandExecutionStatus.InvalidArgumentCount:
                    IoProvider.Output($"<{result.parsedCommand.keyword}> <entered: {result.parsedCommand.argumentCount}> <req: {result.commandDefinition.MinimumArgumentCount}> does not meet minimum argument count.");
                    break;
                case CommandExecutionStatus.KeywordNotFound:
                    IoProvider.Output($"<{result.parsedCommand.keyword}> keyword not found.");
                    break;
                case CommandExecutionStatus.FatalError:
                    if(result.exception != null)
                        IoProvider.Output("fatal: " + result.exception.Message);
                    break;
            }
        }

        private string GenerateUID()
        {
            return $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}" +
                   $"/{Guid.NewGuid()}" +
                   $"/{GetHashCode()}" +
                   $"/{new System.Random(DateTime.UtcNow.Millisecond).NextInt64()}";
        }
    }
}

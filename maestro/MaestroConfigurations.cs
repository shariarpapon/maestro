namespace Everime.Maestro
{
    /// <summary>
    /// Provides all essential configurations needed to build the terminal and other non-essential perferences.
    /// </summary>
    public sealed class MaestroConfigurations
    {
        internal readonly IOProvider ioProvider;
        internal readonly IEnumerable<IMaestroCommand> commands;

        internal string helpKeyword = string.Empty;
        internal bool printCommandExecutionResult = true;
        internal ParsingSymbols parsingSymbols = ParsingSymbols.Default;
        internal System.Action<CommandExecutionResult> onCommandExecutedCallback = null!;

        private MaestroConfigurations(IOProvider ioProvider, IEnumerable<IMaestroCommand> commands) 
        {
            if (ioProvider == null) 
                throw new System.Exception("IO provider cannot be null.");

            if (commands == null || commands.Count() <= 0) 
                throw new System.Exception("No valid commands were passed in to the terminal builder.");

            this.ioProvider = ioProvider;
            this.commands = commands;
        }

        /// <param name="ioProvider">This will provide the terminal with the input reader and output writer methods.</param>
        /// <param name="commands">All valid commands this terminal can execute.</param>
        public static MaestroConfigurations Create(IOProvider ioProvider, IEnumerable<IMaestroCommand> commands) 
        {
            return new MaestroConfigurations(ioProvider, commands);
        }

        /// <summary>
        /// Sets a callback action that will be invoked when commands are executed.
        /// </summary>
        /// <param name="onCommandExecuted">Callback event for when commands are executed.</param>
        public MaestroConfigurations SetOnCommandExecutedCallback(System.Action<CommandExecutionResult> onCommandExecuted)
        {
            onCommandExecutedCallback = onCommandExecuted;
            return this;
        }

        /// <summary>
        /// Set a keyword for the typical "help" commands in a terminal which prints out all the valid commands and their descriptions.
        /// </summary>
        /// <param name="helpKeyword">Help command keyword</param>
        public MaestroConfigurations SetHelpKeyword(string helpKeyword) 
        {
            this.helpKeyword = helpKeyword;
            return this;
        }

        /// <summary>
        /// Parsing symbols essentially defines what kind of format the commands are going to be in.
        /// </summary>
        /// <param name="parsingSymbols">The parsing symbols to be assigned to the parser.</param>
        public MaestroConfigurations SetParsingSymbols(ParsingSymbols parsingSymbols)
        {
            this.parsingSymbols = parsingSymbols;
            return this;
        }

        /// <summary>
        /// Parsing symbols essentially defines what kind of format the commands are going to be in.
        /// </summary>
        /// <param name="commandDelimiter">Multiple commands on the same line are seperated by this character.</param>
        /// <param name="identifierDelimiter">Identifiers are seperated by this character.</param>
        /// <param name="whitespacePlaceholder">This character should be inserted in-place of whitespaces when inputting commands.</param>
        public MaestroConfigurations SetParsingSymbols(char commandDelimiter, char identifierDelimiter, string whitespacePlaceholder)
        {
            SetParsingSymbols(new ParsingSymbols(commandDelimiter, identifierDelimiter, whitespacePlaceholder));
            return this;
        }

        /// <summary>
        /// Sets whether the command executions results should be printed or not.
        /// </summary>
        /// <param name="printResults">If true, the command execution results will be printed to the terminal output.</param>
        public MaestroConfigurations SetPrintCommandExecutionResults(bool printResults) 
        {
            printCommandExecutionResult = printResults;
            return this;
        }
    }
}

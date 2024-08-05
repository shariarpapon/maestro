using System.Collections.Generic;
using System.Linq;

namespace Everime.Maestro
{
    /// <summary>
    /// Provides all essential configurations needed to build the terminal and other non-essential perferences.
    /// </summary>
    public sealed class MaestroConfigurations
    {
        internal IMaestroParser parser;
        internal readonly IMaestroIOHandler ioHandler;
        internal readonly IEnumerable<IMaestroCommand> commands;
        internal System.Action<CommandExecutionResult> onCommandExecutedCallback = null;
        internal bool printCommandExecutionResult = true;
        internal bool printParserErrors = true;
        internal string helpKeyword = "help";
        internal string lineStarter = "> ";

        private MaestroConfigurations(IMaestroIOHandler ioHandler, IEnumerable<IMaestroCommand> commands) 
        {
            parser = new MaestroParser();
            this.ioHandler = ioHandler;
            this.commands = commands;
        }

        /// <param name="ioHandler">This will provide the terminal with the input reader and output writer methods.</param>
        /// <param name="commands">All valid commands this terminal can execute.</param>
        public static MaestroConfigurations Create(IMaestroIOHandler ioHandler, IEnumerable<IMaestroCommand> commands) 
        {
            if (ioHandler == null)
                throw new System.Exception("IO provider cannot be null.");

            if (commands == null || commands.Count() <= 0)
                throw new System.Exception("No valid commands were passed in to the terminal builder.");

            return new MaestroConfigurations(ioHandler, commands);
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
        /// Set a keyword for the typical "help" commands in a terminal which prints out all the valid commands and their descriptions (default: <b>help</b>).
        /// </summary>
        /// <param name="helpKeyword">Help command keyword</param>
        public MaestroConfigurations SetHelpKeyword(string helpKeyword) 
        {
            this.helpKeyword = helpKeyword;
            return this;
        }


        /// <summary>
        /// Set a new parser for the terminal to use.
        /// <br>By default it uses the MaestroParser.</br>
        /// </summary>
        /// <param name="parser">A IMaestroParser object which defines the parsing methods.</param>
        public MaestroConfigurations SetParser(IMaestroParser parser) 
        {
            this.parser = parser;
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

        /// <summary>
        /// A string that will be printed at the begining of every line on the terminal output.(default: <b>'> '</b>).
        /// </summary>
        /// <param name="lineStarter"></param>
        /// <returns></returns>
        public MaestroConfigurations SetLineStarter(string lineStarter) 
        {
            this.lineStarter = lineStarter;
            return this;
        }

        /// <summary>
        /// Sets boolean flag for whether the terminal should be print any parser errors or not.
        /// </summary>
        /// <param name="printParserErrors">If true, the terminal will print out the parse-status if parsing is not succesful.</param>
        public MaestroConfigurations SetPrintParserErrors(bool printParserErrors)
        {
            this.printParserErrors = printParserErrors;
            return this;
        }
    }
}

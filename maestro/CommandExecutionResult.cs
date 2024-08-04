namespace Everime.Maestro
{
    public sealed class CommandExecutionResult 
    {
        public readonly CommandExecutionStatus executionStatus;
        public readonly IMaestroCommand commandDefinition;
        public readonly ParsedCommand parsedCommand;
        public readonly Exception exception = null!;

        internal CommandExecutionResult(CommandExecutionStatus executionStatus, ParsedCommand parsedCommand, IMaestroCommand commandDefinition) 
        {
            this.executionStatus = executionStatus;
            this.parsedCommand = parsedCommand;
            this.commandDefinition = commandDefinition;
        }

        internal CommandExecutionResult(CommandExecutionStatus executionStatus, ParsedCommand parsedCommand, IMaestroCommand commandDefinition, Exception exception)
        {
            this.executionStatus = executionStatus;
            this.parsedCommand = parsedCommand;
            this.commandDefinition = commandDefinition;
            this.exception = exception;
        }
    }
}
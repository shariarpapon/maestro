namespace Everime.Maestro
{
    public enum CommandExecutionStatus
    {
        Successful,
        FatalError,
        KeywordNotFound,
        KeywordNullOrEmpty,
        InvalidArgumentCount,
        FailedExecution
    }
}
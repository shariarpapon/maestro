namespace Maestro
{
    public static class BuiltInMessages
    {
        //Errors
        public const string InvalidArgumentError = "invalid argument";
        public const string InvalidKeywordError = "invalid keyword";
        public const string ArgumentOverflowError = "command has too many arguments";
        public const string InsufficientArgumentsError = "command has insufficient arguments";
        public const string NoValidCommandKeywordError = "no valid command keywords found";
        public const string TerminalAlreadyExistsError = "cannot have multiple instances of terminal";
        public const string FileNotFoundError = "file not found";
        public const string DirectoryNotFound = "directory not found";

        //Warnings
        public const string InvokerNotAuthorizedWarning = "request denied - invoker not authorized";
    }
}
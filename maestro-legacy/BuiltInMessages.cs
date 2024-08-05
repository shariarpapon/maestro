namespace Everime.Maestro.Legacy
{
    public static class BuiltInMessages
    {
        #region Errors
        public static void InvalidArgumentError(object context) 
           => MaestroLogger.PrintError("invalid argument", context);
        public static void InvalidKeywordError(object context)
            => MaestroLogger.PrintError("invalid keyword", context);
        public static void ArgumentOverflowError(object context) 
            => MaestroLogger.PrintError("command has too many arguments", context);
        public static void InsufficientArgumentsError(object context) 
            => MaestroLogger.PrintError("command has insufficient arguments", context);
        public static void NoValidCommandKeywordError(object context)
            => MaestroLogger.PrintError("no valid command keyword found", context);
        public static void TerminalAlreadyExistsError(object context)
            => MaestroLogger.PrintError("cannot have multiple instances of terminal", context);
        public static void FileNotFoundError(object context)
            => MaestroLogger.PrintError("file not found", context);
        public static void DirectoryNotFoundError(object context)
            => MaestroLogger.PrintError("directory not found", context);
        #endregion

        #region Warnings
        public static void InvokerNotAuthorizedWarning(object context)
            => MaestroLogger.PrintWarning("request denied, invoker not authorized", context);
        #endregion
    }
}
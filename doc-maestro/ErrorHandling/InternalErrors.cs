namespace Maestro.ErrorHandling
{
    public static class InternalErrors
    {
        public sealed class TerminalAlreadyExistsError : Error
        {
            public TerminalAlreadyExistsError(object context)
            : base("an instance of a terminal already exist", context) { }
        }

        public sealed class InsufficientArgumentError : Error 
        {
            public InsufficientArgumentError(object context)
            : base("insufficient arguments for the command", context) { }
        }

        public sealed class ArgumentOverflowError : Error
        {
            public ArgumentOverflowError(object context)
            : base("too many arguments", context) { }
        }

        public sealed class InvalidArgumentError : Error
        {
            public InvalidArgumentError(object context)
            : base("invalid argument", context) { }
        }

        public sealed class InvalidCommandError : Error
        {
            public InvalidCommandError(object context)
            : base("invalid command", context) { }
        }

        public sealed class FileNotFoundError : Error
        {
            public FileNotFoundError(object context)
            : base("file not found", context) { }
        }

        public sealed class DirectoryNotFoundError: Error
        {
            public DirectoryNotFoundError(object context)
            : base("directory not found", context) { }
        }
    }
}

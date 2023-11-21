namespace Maestro.Core
{
    public class ErrorMessage : MaestroMessage
    {
        public const ConsoleColor ERROR_FOREGROUND = ConsoleColor.Red;

        public ErrorMessage(string message, object context)
            : this(string.Format("error: {0} <{1}>", message, context)) { }

        public ErrorMessage(string message)
            : base(message, ERROR_FOREGROUND, default) { }

        public static implicit operator bool(ErrorMessage e) => false;
        public static implicit operator int(ErrorMessage e) => -1;
    }
}

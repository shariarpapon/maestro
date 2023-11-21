using Maestro.Core;

namespace Maestro.InternalErrors
{
    public class ErrorMessage : MaestroMessage
    {
        public const ConsoleColor ERROR_FOREGROUND = ConsoleColor.Red;

        public ErrorMessage(string message, object context)
            : base(string.Format("{0} {1}", "error:", message), context, ERROR_FOREGROUND, default) {}

        public static implicit operator bool(ErrorMessage e) => false;
        public static implicit operator int(ErrorMessage e) => -1;
    }
}

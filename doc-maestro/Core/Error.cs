namespace Maestro.Core
{
    public class Error : MaestroMessage
    {
        public const ConsoleColor ERROR_FOREGROUND = ConsoleColor.Red;
        public const ConsoleColor ERROR_BACKGROUND = default;

        public Error(string message, object context)
            :base(message + $" <{context}>", ERROR_FOREGROUND, ERROR_BACKGROUND){}

        public static implicit operator bool(Error e) => false;
        public static implicit operator int(Error e) => -1;
    }
}

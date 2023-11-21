namespace Maestro
{
    public sealed class MaestroLogger
    {
        public const ConsoleColor WARNING_FG = ConsoleColor.Yellow;
        public const ConsoleColor ERROR_FG = ConsoleColor.DarkRed;
        public const ConsoleColor INFO_FG = ConsoleColor.DarkGray;
        private static MaestroLogger _Instance = null!;

        public MaestroLogger(MaestroTerminal terminal)
        {
            if (_Instance != null)
                return;
            else
            {
                _Instance = this;
            }
        }

        internal void Dispose()
        {
            _Instance = null!;
        }

        public static void PrintFormat(MaestroMessage message)
        {
            MaestroTerminal.RequestPushMessage(message, _Instance);
        }

        public static void Print(string message)
        {
            PrintFormat(new MaestroMessage(message));
        }

        public static void Print(string message, object context)
        {
            PrintFormat(new MaestroMessage(message, context));
        }

        public static void PrintInfo(string message, object context)
        {
            PrintFormat(new MaestroMessage(message, context, INFO_FG, default));
        }

        public static void PrintError(string message, object context)
        {
            PrintFormat(new MaestroMessage(message, context, ERROR_FG, default));
        }

        public static void PrintWarning(string message, object context)
        {
            PrintFormat(new MaestroMessage(message, context, WARNING_FG, default));
        }
    }
}

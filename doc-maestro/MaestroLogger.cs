namespace Maestro
{
    public sealed class MaestroLogger
    {
        private const ConsoleColor WARNING_FG = ConsoleColor.Yellow;
        private const ConsoleColor ERROR_FG = ConsoleColor.DarkRed;
        private const ConsoleColor INFO_FG = ConsoleColor.DarkGray;
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

        public static void Print(object message)
        {
            PrintFormat(new MaestroMessage(message));
        }

        public static void Print(object message, object context)
        {
            PrintFormat(new MaestroMessage(message, context));
        }

        public static void PrintInfo(object message, object context)
        {
            PrintFormat(new MaestroMessage(message, context, INFO_FG, default));
        }

        public static void PrintError(object message, object context)
        {
            PrintFormat(new MaestroMessage(message, context, ERROR_FG, default));
        }

        public static void PrintWarning(object message, object context)
        {
            PrintFormat(new MaestroMessage(message, context, WARNING_FG, default));
        }
    }
}

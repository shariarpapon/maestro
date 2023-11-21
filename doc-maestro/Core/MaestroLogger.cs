namespace Maestro.Core
{
    public sealed class MaestroLogger
    {
        private static MaestroLogger _Instance = null!;
        private MaestroTerminal _terminal = null!;

        public MaestroLogger(MaestroTerminal terminal)
        {
            if (_Instance != null)
                return;
            else
            { 
                _Instance = this;
                _terminal = terminal;
            }
        }

        internal void Dispose() 
        {
            _Instance = null!;
            _terminal = null!;
        }

        public static void Print(MaestroMessage message)
        {
            _Instance._terminal.PushMessage(message);
        }

        public static void Print(string message)
        {
            Print(new MaestroMessage(message));
        }

        public static void PrintInfo(string message) 
        {
            Print(new MaestroMessage(message, ConsoleColor.Green, default));
        }

        public static void PrintError(string message) 
        {
            Print(new ErrorMessage(message));
        }

        public static void PrintWarning(string message) 
        {
            Print(new WarningMessage(message));
        }
    }
}

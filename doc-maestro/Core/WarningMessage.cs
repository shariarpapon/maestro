namespace Maestro.Core
{
    public sealed class WarningMessage : MaestroMessage
    {
        private const ConsoleColor WARNING_FOREGROUND = ConsoleColor.Yellow;

        public WarningMessage(string message) : 
            base(message, WARNING_FOREGROUND, default) {}

        public WarningMessage(string message, object context) :
           base(message, context, WARNING_FOREGROUND, default) {}
    }
}

namespace DocMaestro.Core
{
    public sealed class MaestroMessage
    {
        public ConsoleColor ForegroundColor { get; private set; } = Console.ForegroundColor;
        public ConsoleColor BackgroundColor { get; private set; } = Console.BackgroundColor;
        public string Message { get; private set; } = null;

        public MaestroMessage() { }

        public MaestroMessage(string message) 
        {
            Message = message;
        }

        public MaestroMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Message = message;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }
}

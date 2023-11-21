namespace Maestro
{
    public sealed class MaestroMessage
    {
        public readonly ConsoleColor foregroundColor = Console.ForegroundColor;
        public readonly ConsoleColor backgroundColor = Console.BackgroundColor;
        public readonly string message = null!;

        public MaestroMessage(string message)
        {
            this.message = message;
        }

        public MaestroMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            this.message = message;
            this.foregroundColor = foregroundColor;
            this.backgroundColor = backgroundColor;
        }

        public MaestroMessage(string message, object context)
        {
            this.message = context == null ? message : string.Format("{0} <{1}>", message, context);
        }

        public MaestroMessage(string message, object context, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            : this(message, context)
        {
            this.foregroundColor = foregroundColor;
            this.backgroundColor = backgroundColor;
        }

    }
}

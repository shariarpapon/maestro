namespace Everime.Maestro.Legacy
{
    public sealed class MaestroMessage
    {
        public readonly ConsoleColor foregroundColor = Console.ForegroundColor;
        public readonly ConsoleColor backgroundColor = Console.BackgroundColor;
        public readonly object message = null!;

        public MaestroMessage(object message)
        {
            this.message = message;
        }

        public MaestroMessage(object message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            this.message = message;
            this.foregroundColor = foregroundColor;
            this.backgroundColor = backgroundColor;
        }

        public MaestroMessage(object message, object context)
        {
            this.message = context == null ? message : string.Format("{0} <{1}>", message, context);
        }

        public MaestroMessage(object message, object context, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            : this(message, context)
        {
            this.foregroundColor = foregroundColor;
            this.backgroundColor = backgroundColor;
        }

    }
}

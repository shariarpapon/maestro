using DocMaestro.Core;

namespace DocMaestro.ErrorHandling
{
    public static class ErrorHandler
    {
        public const ConsoleColor ERROR_FOREGROUND_COLOR = ConsoleColor.Red;
        public const ConsoleColor ERROR_BACKGROUND_COLOR = default;

        public static bool PushError(Error error)
        {
            MaestroMessage errorMessage = new MaestroMessage(error.Message, ERROR_FOREGROUND_COLOR, ERROR_BACKGROUND_COLOR);
            MaestroMain.ActiveTerminal.PushMessage(errorMessage);
            return false;
        }
    }

    public class Error 
    {
        public string Message { get; private set; } = "";
        public object Context { get; private set; } = null;

        public Error(string error, object context) 
        {
            Message = $"error: {error} <{context.ToString()}>";
            Context = context;
        }
    }
}

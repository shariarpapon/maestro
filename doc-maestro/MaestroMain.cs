using DocMaestro.Core;
using DocMaestro.ErrorHandling;

namespace DocMaestro
{
    public static class MaestroMain
    {
        internal static MaestroTerminal ActiveTerminal { get; private set; }

        private static void Main()
        {
            if (ActiveTerminal == null)
            {
                ActiveTerminal = new MaestroTerminal("Maestro");
                ActiveTerminal.Initiate();
            }
            else 
            {
                ErrorHandler.PushError(new InternalErrors.TerminalAlreadyExistsError(ActiveTerminal.Title));
            }
        }

        public static void Debug(string msg) 
        {
            ActiveTerminal.PushMessage(new MaestroMessage(msg, ConsoleColor.DarkMagenta, default));
        }
    }
}

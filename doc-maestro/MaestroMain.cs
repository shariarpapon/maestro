using Maestro.Core;
namespace Maestro
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
                MaestroLogger.Print(new InternalErrors.TerminalAlreadyExistsError(ActiveTerminal.Title));
            }
        }
    }
}

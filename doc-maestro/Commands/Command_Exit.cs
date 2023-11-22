namespace Maestro.Commands
{
    internal sealed class Command_Exit : CommandAction
    {
        internal override string Keyword => "exit";
        internal override uint RequiredArgCount => 0;
        internal override bool Invoke(object invoker, string[] args)
        {
            return MaestroTerminal.RequestExit(invoker);
        }
    }
}

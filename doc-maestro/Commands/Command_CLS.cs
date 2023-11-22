namespace Maestro.Commands
{
    internal sealed class Command_CLS : CommandAction
    {
        internal override string Keyword => "cls";
        internal override uint RequiredArgCount => 0;

        internal override bool Invoke(object invoker, string[] args)
        {
            return MaestroTerminal.RequestClearTerminal(invoker);
        }
    }
}

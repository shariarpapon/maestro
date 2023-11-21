namespace Maestro.Commands
{
    public sealed class Command_Exit : CommandAction
    {
        public override uint RequiredArgCount => 0;
        public override bool Invoke(object invoker, string[] args)
        {
            return MaestroTerminal.RequestExit(invoker);
        }
    }
}

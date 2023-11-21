namespace Maestro.Commands
{
    public abstract class CommandAction
    {
        public abstract uint RequiredArgCount { get; }
        public abstract bool Invoke(object invoker, string[] args);
    }
}

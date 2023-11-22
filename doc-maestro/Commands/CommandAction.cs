namespace Maestro.Commands
{
    internal abstract class CommandAction
    {
        internal abstract string Keyword { get; }
        internal abstract uint RequiredArgCount { get; }
        internal abstract bool Invoke(object invoker, string[] args);
    }
}

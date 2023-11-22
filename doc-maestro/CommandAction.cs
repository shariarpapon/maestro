namespace Maestro
{
    public abstract class CommandAction
    {
        public abstract string Keyword { get; }
        public abstract uint RequiredArgCount { get; }
        public virtual string Description { get; } = string.Empty;
        public abstract bool Invoke(object invoker, string[] args);
    }
}

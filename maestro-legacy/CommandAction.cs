namespace MaestroCommandliner
{
    public abstract class CommandAction
    {
        public abstract string Keyword { get; }
        public abstract uint RequiredArgCount { get; }
        public abstract bool Invoke(object invoker, string[] args);
        public virtual string Description { get; } = string.Empty;
    }
}

namespace Maestro.Core
{
    internal class CommandAction
    {
        internal int RequiredArgCount { get; }
        internal CommandAction(int argCountIncludingCmd) 
        {
            RequiredArgCount = argCountIncludingCmd + 1;
        }
    }
}

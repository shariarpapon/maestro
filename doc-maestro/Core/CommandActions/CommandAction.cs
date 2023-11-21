namespace Maestro.Core
{
    internal class CommandAction
    {
        internal uint RequiredArgCount { get; }
        internal CommandAction(uint argCountIncludingCmd) 
        {
            RequiredArgCount = argCountIncludingCmd;
        }
    }
}

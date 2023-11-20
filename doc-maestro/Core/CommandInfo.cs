namespace Maestro.Core.Commands
{
    internal sealed class CommandInfo
    {
        internal int argumentCount = 0;
        internal CommandInfo(int argumentCount) 
        {
            this.argumentCount = argumentCount;
        }
    }
}

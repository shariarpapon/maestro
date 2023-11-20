namespace Maestro.Core.Commands
{
    internal sealed class MaestroCommandDefinition
    {
        internal int argumentCount = 0;
        internal MaestroCommandDefinition(int argumentCount) 
        {
            this.argumentCount = argumentCount;
        }
    }
}

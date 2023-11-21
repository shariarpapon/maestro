using Maestro.Core;

namespace Maestro.InternalErrors
{
    public sealed class InsufficientArgumentsError : ErrorMessage
    {
        public InsufficientArgumentsError(object context)
        : base("command has insufficient arguments", context) { }
    }
}

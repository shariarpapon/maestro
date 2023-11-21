using Maestro.Core;

namespace Maestro.InternalErrors
{
    public sealed class ArgumentOverflowError : ErrorMessage
    {
        public ArgumentOverflowError(object context)
        : base("command has too many arguments", context) { }
    }
}

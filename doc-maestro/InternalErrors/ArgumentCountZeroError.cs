using Maestro.Core;

namespace Maestro.InternalErrors
{
    public sealed class ArgumentCountZeroError : ErrorMessage
    {
        public ArgumentCountZeroError(object context)
            : base("argument count must be greater than or equal to 1 because the command keyword itself is an argument.", context) 
        {}
    }
}

using Maestro.Core;

namespace Maestro.InternalErrors
{
    public sealed class InvalidArgumentError : ErrorMessage
    {
        public InvalidArgumentError(object context) 
            :base("command has invalid arguments.", context)
        {
        }
    }
}

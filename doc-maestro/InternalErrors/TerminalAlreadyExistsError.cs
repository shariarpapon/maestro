using Maestro.Core;

namespace Maestro.InternalErrors
{
    public sealed class TerminalAlreadyExistsError : ErrorMessage
    {
        public TerminalAlreadyExistsError(object context)
        : base("an instance of a terminal already exist", context) { }
    }
}

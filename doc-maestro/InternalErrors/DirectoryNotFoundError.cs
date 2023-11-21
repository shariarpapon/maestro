using Maestro.Core;

namespace Maestro.InternalErrors
{
    public sealed class DirectoryNotFoundError : ErrorMessage
    {
        public DirectoryNotFoundError(object context)
        : base("directory not found", context) { }
    }
}

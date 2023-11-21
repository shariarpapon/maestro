using Maestro.Core;

namespace Maestro.InternalErrors
{
    public sealed class FileNotFoundError : ErrorMessage
    {
        public FileNotFoundError(object context)
        : base("file not found", context) { }
    }
}

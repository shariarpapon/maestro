namespace Maestro.InternalErrors
{
    public sealed class InvalidKeywordError : ErrorMessage
    {
        public InvalidKeywordError(object context)
            : base("invalid keyword", context) { }
    }
}

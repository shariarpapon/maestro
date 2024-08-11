namespace Everime.Maestro
{
    public enum ParseStatus 
    { 
        SourceNullOrEmpty = 0,
        NoValidTokensFound,
        NoValidStatementsFound,
        NoValidCommandsFound,
        Failed,
        Successful
    }
}
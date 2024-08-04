namespace Everime.Maestro
{
    internal sealed class ParserOutput 
    {
        internal readonly ParseStatus status;
        internal readonly ParsedCommand[] commands;

        internal ParserOutput(ParseStatus status, ParsedCommand[] commands) 
        {
            this.status = status;
            this.commands = commands;
        }
    }
}

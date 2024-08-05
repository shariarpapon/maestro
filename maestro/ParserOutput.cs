namespace Everime.Maestro
{
    /// <summary>
    /// Holds the output data of a parsed source.
    /// </summary>
    public sealed class ParserOutput
    {
        internal readonly ParseStatus status;
        internal readonly ParsedCommand[] commands;

        public ParserOutput(ParseStatus status, ParsedCommand[] commands) 
        {
            this.status = status;
            this.commands = commands;
        }
    }
}

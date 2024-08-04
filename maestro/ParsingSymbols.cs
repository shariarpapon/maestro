namespace Everime.Maestro
{
    /// <summary>
    /// This symbols will be used to parse the commands and arguments.
    /// </summary>
    public sealed class ParsingSymbols
    {
        /// <summary>
        /// A default set of parsing symbols.
        /// <br>Command Delimiter: <b>&amp;</b>
        /// <br/>Identifier Delimiter: <b>single-whitespace</b>
        /// <br/>Whitespace Placeholder: <b>::</b></br>
        /// </summary>
        public static ParsingSymbols Default => new ParsingSymbols('&', ' ', "::");

        public readonly char commandDelimiter;
        public readonly char identifierDelimiter;
        public readonly string whitespacePlaceholder;

        public ParsingSymbols(char commandDelimiter, char identifierDelimiter, string whtiespacePlaceholder) 
        {
            this.commandDelimiter = commandDelimiter;
            this.identifierDelimiter = identifierDelimiter;
            this.whitespacePlaceholder = whtiespacePlaceholder;
        }
    }
}

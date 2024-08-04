using System.Text;

namespace Everime.Maestro
{
    internal sealed class MaestroParser
    {
        private readonly char _commandDelimiter;
        private readonly char _identifierDelimiter;
        private readonly string _whitespacePlaceholder;

        internal MaestroParser(ParsingSymbols symbols)
            : this(symbols.commandDelimiter, symbols.identifierDelimiter, symbols.whitespacePlaceholder) { }

        internal MaestroParser(char commandDelimiter, char argumentDelimiter, string whitespacePlaceholder)
        {
            _commandDelimiter = commandDelimiter;
            _identifierDelimiter = argumentDelimiter;
            _whitespacePlaceholder = whitespacePlaceholder;
        }

        /// <summary>
        /// Parses the given source string and returns an output object containing the data.
        /// </summary>
        /// <param name="source">The string to parse.</param>
        internal ParserOutput Parse(string source) 
        {
            if (string.IsNullOrEmpty(source))
                return new ParserOutput(ParseStatus.SourceNullOrEmpty, null!);
            source = FormatSource(source);
            ParseStatus status = ParseCommands(source, out ParsedCommand[] commands);
            return new ParserOutput(status, commands);
        }

        //Parse all the commands from the source
        private ParseStatus ParseCommands(string source, out ParsedCommand[] commands) 
        {
            string[] statements = ParseStatements(source);
            commands = null!;

            if (statements.Length <= 0)
            {
                return ParseStatus.NoValidStatementsFound;
            }

            commands = new ParsedCommand[statements.Length];
            HashSet<string> tracker = new HashSet<string>();
            for (int i = 0; i < statements.Length; i++)
            {
                if (tracker.Contains(statements[i]))
                    continue;
                string[] args = ParseArguments(statements[i]);
                string keyword = args[0];
                args = args.Skip(1).ToArray();
                commands[i] = new ParsedCommand(keyword, args);
                tracker.Add(statements[i]);
            }
            return ParseStatus.Successful;
        }

        //Parse statements from given commands
        private string[] ParseStatements (string source)
        {
            source = TrimRightOf(source, _commandDelimiter);
            source = TrimLeftOf(source, _commandDelimiter);
            return source.Split(_commandDelimiter);
        }

        //Parse arguments from given statement
        private string[] ParseArguments(string statement)
        {
            string trimmed = statement.Trim();           
            string[] args = trimmed.Split(_identifierDelimiter);
            //Prase white spaces
            for (int i = 0; i < args.Length; i++)
                args[i] = args[i].Replace(_whitespacePlaceholder, " ");
            return args;
        }

        /// <summary>
        /// Formats the source string for parsing.
        /// </summary>
        /// <returns>The formatted string.</returns>
        private static string FormatSource(string source)
        {
            string trimmedSource = source.Trim();
            StringBuilder buffer = new StringBuilder(trimmedSource.Length);
            bool trim = false;
            for (int i = 0; i < trimmedSource.Length; i++)
            {
                char c = trimmedSource[i];
                if (char.IsWhiteSpace(c))
                {
                    if (trim) continue;
                    else
                    {
                        trim = true;
                        buffer.Append(c);
                    }
                }
                else
                {
                    if (trim) trim = false;
                    buffer.Append(c);
                }
            }
            return buffer.ToString();
        }

        /// <summary>
        /// Trims white spaces on the right of the target character.
        /// </summary>
        /// <returns>The trimmed string.</returns>
        private static string TrimRightOf(string input, char target)
        {
            StringBuilder buffer = new StringBuilder(input.Length);
            bool trim = false;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (trim)
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        buffer.Append(c);
                        trim = false;
                    }
                    continue;
                }
                else if (c == target)
                {
                    trim = true;
                    buffer.Append(c);
                    continue;
                }
                buffer.Append(c);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// Trims white spaces on the left of the target character.
        /// </summary>
        /// <returns>The trimmed string.</returns>
        private static string TrimLeftOf(string input, char target)
        {
            StringBuilder buffer = new StringBuilder(input.Length);
            bool trim = false;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[input.Length - i - 1];
                if (trim)
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        buffer.Append(c);
                        trim = false;
                    }
                    continue;
                }
                else if (c == target)
                {
                    trim = true;
                    buffer.Append(c);
                    continue;
                }
                buffer.Append(c);
            }
            string reversed = new string(buffer.ToString().Reverse().ToArray());
            return reversed;
        }
    }
}

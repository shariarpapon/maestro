using System.Text;

namespace MaestroCommandliner
{
    public class ParsedCommand 
    {
        public readonly string keyword;
        public readonly string[] arguments;
        public ParsedCommand(string keyword, string[] arguments) 
        {
            this.keyword = keyword;
            this.arguments = arguments;
        }
    }

    public class MaestroParser
    {
        private readonly string _source;
        private readonly char _commandDelimiter;
        private readonly char _argumentDelimiter;
        private readonly string _whiteSpaceSequence;

        public readonly ParsedCommand[] parsedCommands;

        public MaestroParser(string source, char commandDelimiter, char argumentDelimiter, string whiteSpaceSequence)
        {
            if (string.IsNullOrEmpty(source))
                throw new System.Exception("Cannot initialize parser, source string cannot be null.");
            _source = FormatSource(source);
            _commandDelimiter = commandDelimiter;
            _argumentDelimiter = argumentDelimiter;
            _whiteSpaceSequence = whiteSpaceSequence;
            parsedCommands = ParseCommands();
        }

        //Parse all the commands from the source
        private ParsedCommand[] ParseCommands() 
        {
            string[] statements = ParseStatements();
            if (statements.Length <= 0)
                return null!;
            List<ParsedCommand> commands = new List<ParsedCommand>();
            HashSet<string> tracker = new HashSet<string>();
            for (int i = 0; i < statements.Length; i++)
            {
                if (tracker.Contains(statements[i]))
                    continue;
                string[] args = ParseArguments(statements[i]);
                string keyword = args[0];
                args = args.Skip(1).ToArray();
                commands.Add(new ParsedCommand(keyword, args));
                tracker.Add(statements[i]);
            }
            return commands.ToArray();
        }

        //Parse statements from given commands
        private string[] ParseStatements()
        {
            string input = _source;
            input = TrimRightOf(input, _commandDelimiter);
            input = TrimLeftOf(input, _commandDelimiter);
            return input.Split(_commandDelimiter);
        }

        //Parse arguments from given statement
        private string[] ParseArguments(string statement)
        {
            string trimmed = statement.Trim();           
            string[] args = trimmed.Split(_argumentDelimiter);
            //Prase white spaces
            for (int i = 0; i < args.Length; i++)
                args[i] = args[i].Replace(_whiteSpaceSequence, " ");
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

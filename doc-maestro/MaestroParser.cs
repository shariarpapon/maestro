using System.Text;

namespace Maestro
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
        public readonly ParsedCommand[] parsedCommands;

        private const char _DEFAULT_COMMAND_DELIMITER = '&';
        private const char _DEFAULT_ARGUMENT_DELIMITER = ' ';

        public MaestroParser(string source)
            :this(source, _DEFAULT_COMMAND_DELIMITER, _DEFAULT_ARGUMENT_DELIMITER) {}

        public MaestroParser(string source, char commandDelimiter, char argumentDelimiter)
        {
            if (string.IsNullOrEmpty(source))
                throw new System.Exception("Cannot initialize parser, source string cannot be null.");
            _source = FormatSource(source);
            _commandDelimiter = commandDelimiter;
            _argumentDelimiter = argumentDelimiter;
            parsedCommands = ParseCommands();
        }

        public ParsedCommand[] ParseCommands() 
        {
            string[] commandStatements = ParseStatements();
            if (commandStatements.Length <= 0)
                return null!;
            List<ParsedCommand> parsed = new List<ParsedCommand>();
            HashSet<string> tracker = new HashSet<string>();
            for (int i = 0; i < commandStatements.Length; i++)
            {
                if (tracker.Contains(commandStatements[i]))
                    continue;
                string[] args = ParseArguments(commandStatements[i]);
                string keyword = args[0];
                args = args.Skip(1).ToArray();
                parsed.Add(new ParsedCommand(keyword, args));
                tracker.Add(commandStatements[i]);
            }
            return parsed.ToArray();
        }

        private string[] ParseStatements()
        {
            string buffer = _source;
            buffer = TrimRightOf(buffer, _commandDelimiter);
            buffer = TrimLeftOf(buffer, _commandDelimiter);
            return buffer.Split(_commandDelimiter);
        }

        private string[] ParseArguments(string statement)
        {
            string trimmed = statement.Trim();           
            string[] args = trimmed.Split(_argumentDelimiter);
            for (int i = 0; i < args.Length; i++)
                args[i] = args[i].Replace("::", " ");
            return args;
        }

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

        private static string TrimRightOf(string input, char delimiter)
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
                else if (c == delimiter)
                {
                    trim = true;
                    buffer.Append(c);
                    continue;
                }
                buffer.Append(c);
            }
            return buffer.ToString();
        }

        private static string TrimLeftOf(string input, char delimiter)
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
                else if (c == delimiter)
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

using System.Text;

namespace Maestro
{
    internal class MaestroParser
    {
        private string _source;
        private char _commandDelimiter;
        private char _argumentDelimiter = ' ';

        internal MaestroParser(string source, char commandDelimiter, char argumentDelimiter)
        {
            _source = FormatSource(source);
            _commandDelimiter = commandDelimiter;
            _argumentDelimiter = argumentDelimiter;
        }

        internal string[] ParseStatements()
        {
            string buffer = _source;
            buffer = TrimRightOf(buffer, _commandDelimiter);
            buffer = TrimLeftOf(buffer, _commandDelimiter);
            return buffer.Split(_commandDelimiter);
        }

        internal string[] ParseArguments(string statement)
        {
            string buffer = statement.Trim();
            string[] args = buffer.Split(_argumentDelimiter);
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

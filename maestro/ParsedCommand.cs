namespace Everime.Maestro
{
    public readonly struct ParsedCommand
    {
        public readonly string keyword;
        public readonly string[] arguments;
        public readonly uint argumentCount;
        public ParsedCommand(string keyword, string[] arguments)
        {
            this.keyword = keyword;
            this.arguments = arguments;
            argumentCount = (uint)arguments.Length;
        }
    }
}

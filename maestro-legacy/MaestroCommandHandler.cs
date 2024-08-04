namespace MaestroCommandliner
{
    public class MaestroCommandHandler
    {
        public static char s_CommandDelimiter { get; private set; } = '&';
        public static char s_ArgumentDelimiter { get; private set; } = ',';
        public static string s_WhiteSpaceSequence { get; private set; } = "::";
        public readonly CommandAction[] commandActions;

        public MaestroCommandHandler(CommandAction[] commandActions)
        {
            this.commandActions = commandActions;
        }

        public bool ParseAndExecute(string source, object invokeAuthorizer)
        {
            MaestroParser parser = new MaestroParser(source, s_CommandDelimiter, s_ArgumentDelimiter, s_WhiteSpaceSequence);
            foreach (var cmd in parser.parsedCommands)
            {
                CommandAction cmdAction = Array.Find(commandActions, c => c.Keyword == cmd.keyword)!;
                if (cmdAction == null || cmd.arguments.Length < cmdAction.RequiredArgCount)
                    return false;
                cmdAction?.Invoke(invokeAuthorizer, cmd.arguments);
            }
            return true;
        }

        public void SetParserSymbols(char commandDelim, char argDelim, string whiteSpaceSeq) 
        {
            s_CommandDelimiter = commandDelim;
            s_ArgumentDelimiter = argDelim;
            s_WhiteSpaceSequence = whiteSpaceSeq;
        }
    }
}

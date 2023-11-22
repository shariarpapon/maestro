namespace Maestro
{
    public class MaestroCommandHandler
    {
        private const char COMMAND_DELIMITER = '&';
        private const char ARGUMENT_DELIMITER = ' ';
        public readonly CommandAction[] commandActions;

        public MaestroCommandHandler(CommandAction[] commandActions)
        {
            this.commandActions = commandActions;
        }

        public bool ParseAndExecute(string source, object invokeAuthorizer)
        {
            MaestroParser parser = new MaestroParser(source, COMMAND_DELIMITER, ARGUMENT_DELIMITER);
            var commands = parser.parsedCommands;
            foreach (var cmd in commands)
            {
                CommandAction cmdAction = Array.Find(commandActions, c => c.Keyword == cmd.keyword)!;
                if (cmdAction == null || cmd.arguments.Length < cmdAction.RequiredArgCount)
                    return false;
                cmdAction?.Invoke(invokeAuthorizer, cmd.arguments);
            }
            return true;
        }
    }
}

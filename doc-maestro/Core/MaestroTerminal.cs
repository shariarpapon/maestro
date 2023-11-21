namespace Maestro.Core
{
    public class MaestroTerminal
    {
        private const string INTRO_TITLE =
         "+=============================+\n" +
         "|       Maestro Terminal      |\n" +
         "+=============================+\n" +
         "^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n";

        private readonly MaestroMessage _introMessage = new MaestroMessage(INTRO_TITLE, ConsoleColor.Cyan, default);
        private readonly Queue<MaestroMessage> _messageBuffer;
        private MaestroLogger _logger;
        private bool _running = true;
        public string Title { get; } = "terminal";

        public MaestroTerminal(string title) 
        {
            Title = title;
            _logger = new MaestroLogger(this);
            _messageBuffer = new Queue<MaestroMessage>();
        }

        ~MaestroTerminal() 
        {
            _logger.Dispose();
        }

        public void Initiate() 
        {
            Console.Title = Title;
            TerminalWrite(_introMessage);
            StartTerminal();
        }

        public void ClearTerminal() 
        {
            Console.Clear();
        }

        private void StartTerminal() 
        {
            while (_running) 
            {
                TerminalWrite(">> ", false);
                if (PeekMessage()) 
                    continue;

                ScanCommands();
            }
        }

        private void ScanCommands() 
        {
            string input = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(input))
                CommandManager.ParseAndExecute(input, this);
        }

        private bool PeekMessage() 
        {
            if (_messageBuffer.Count > 0)
            {
                TerminalWrite(_messageBuffer.Dequeue(), true);
                return true;
            }
            else return false;
        }

        internal void PushMessage(string message)
        {
            PushMessage(new MaestroMessage(message));
        }

        internal void PushMessage(MaestroMessage message)
        {
            _messageBuffer.Enqueue(message);
        }

        private void TerminalWrite(string message, bool newLine = false)
        {
            TerminalWrite(new MaestroMessage(message), newLine);
        }

        private void TerminalWrite(MaestroMessage message, bool newLine = false)
        {
            ConsoleColor prevFg = Console.ForegroundColor;
            ConsoleColor prevBg = Console.BackgroundColor;
            Console.ForegroundColor = message.foregroundColor;
            Console.BackgroundColor = message.backgroundColor;
            if (newLine)
                Console.WriteLine(message.message);
            else
                Console.Write(message.message);
            Console.ForegroundColor = prevFg; ;
            Console.BackgroundColor = prevBg;
        }

        public void Exit() 
        {
            _messageBuffer.Clear();
            _running = false;
        }
    }
}

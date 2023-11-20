using DocMaestro.Core.Commands;

namespace DocMaestro.Core
{
    public class MaestroTerminal
    {
        private const string INTRO_TITLE =
         "+=============================+\n" +
         "|       Maestro Terminal      |\n" +
         "+=============================+\n" +
         "###############################\n\n";
        private readonly MaestroMessage _introMessage = new MaestroMessage(INTRO_TITLE, ConsoleColor.Cyan, default);
        private readonly Queue<MaestroMessage> _messageBuffer;
        private bool _running = true;
        public string Title { get; } = "terminal";

        public MaestroTerminal(string title) 
        {
            Title = title;
            _messageBuffer = new Queue<MaestroMessage>();
        }

        public void Initiate() 
        {
            Console.Title = Title;
            LogMessage(_introMessage);
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
                LogMessage(">> ", false);
                if (PeekMessage()) 
                    continue;
                CaptureInput();
                
            }
        }

        private void CaptureInput() 
        {
            string input = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(input))
            {
                MaestroMessage[] output = null!;
                if (CommandManager.TryParseAndExecute(input, out output))
                {
                    foreach (MaestroMessage m in output)
                        PushMessage(m);
                }
            }
        }

        private bool PeekMessage() 
        {
            if (_messageBuffer.Count > 0)
            {
                LogMessage(_messageBuffer.Dequeue(), true);
                return true;
            }
            else return false;
        }

        public void PushMessage(string message)
        {
            PushMessage(new MaestroMessage(message));
        }

        public void PushMessage(MaestroMessage message)
        {
            _messageBuffer.Enqueue(message);
        }

        private void LogMessage(string message, bool newLine = false)
        {
            LogMessage(new MaestroMessage(message), newLine);
        }

        private void LogMessage(MaestroMessage message, bool newLine = false)
        {
            ConsoleColor prevFg = Console.ForegroundColor;
            ConsoleColor prevBg = Console.BackgroundColor;
            Console.ForegroundColor = message.ForegroundColor;
            Console.BackgroundColor = message.BackgroundColor;
            if (newLine)
                Console.WriteLine(message.Message);
            else
                Console.Write(message.Message);
            Console.ForegroundColor = prevFg; ;
            Console.BackgroundColor = prevBg;
        }

        public void Dispose() 
        {
            _messageBuffer.Clear();
            _running = false;
        }
    }
}

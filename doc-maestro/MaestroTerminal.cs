using Maestro.Commands;

namespace Maestro
{
    public class MaestroTerminal
    {
        private const string INTRO_TITLE =
         "+=============================+\n" +
         "|       Maestro Terminal      |\n" +
         "+=============================+\n" +
         "^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n";

        private static MaestroTerminal _Instance = null!;
        private static bool _IsProcessing = false;
        private readonly MaestroMessage _introMessage = new MaestroMessage(INTRO_TITLE, ConsoleColor.Cyan, default);
        private bool _running;
        private string _title;

        private MaestroLogger _logger;
        private readonly HashSet<object> _authorizedInvokers;
        private readonly Queue<MaestroMessage> _messageBuffer;
        private string _selfInvoker = null!;

        private MaestroTerminal(string title)
        {
            _title = title;
            _logger = new MaestroLogger(this);
            _messageBuffer = new Queue<MaestroMessage>();
            _authorizedInvokers = new HashSet<object>();
        }

        public static void Initiate(string title)
        {
            if (_Instance == null)
            {
                _Instance = new MaestroTerminal(title);
            }
            else
            {
                MaestroLogger.PrintError(BuiltInMessages.TerminalAlreadyExistsError, _Instance._title);
                return;
            }

            _Instance._selfInvoker = GenerateSelfInvoker();
            AuthorizeInvoker(_Instance._selfInvoker);
            AuthorizeInvoker(_Instance._logger);

            TerminalWrite(_Instance._introMessage);
            Console.Title = _Instance._title;
            StartTerminal();
        }

        public static Thread InitiateOnSeperateThread(string title)
        {
            if (_Instance != null || _IsProcessing)
            {
                MaestroLogger.PrintError(BuiltInMessages.TerminalAlreadyExistsError, _Instance!._title);
                return null!;
            }
            else
            {
                _IsProcessing = true;
                _Instance = new MaestroTerminal(title);
            }
            
            _Instance._selfInvoker = GenerateSelfInvoker();
            AuthorizeInvoker(_Instance._selfInvoker);
            AuthorizeInvoker(_Instance._logger);
            TerminalWrite(_Instance._introMessage);
            Console.Title = _Instance._title;

            Thread thread = new Thread(StartTerminal);
            thread.Start();
            return thread;
        }

        private static void AuthorizeInvoker(object invoker)
        {
            _Instance._authorizedInvokers.Add(invoker);
        }

        private static void StartTerminal()
        {
            _Instance._running = true;
            while (_Instance._running)
            {
                TerminalWrite(">> ", false);
                if (PeekMessage())
                    continue;
                ScanCommands();
            }
        }

        private static void ScanCommands()
        {
            string input = Console.ReadLine()!;
            if (!string.IsNullOrEmpty(input))
                MaestroCommandHandler.ParseAndExecute(input, _Instance._selfInvoker);
        }

        private static bool PeekMessage()
        {
            if (_Instance._messageBuffer.Count > 0)
            {
                TerminalWrite(_Instance._messageBuffer.Dequeue(), true);
                return true;
            }
            else return false;
        }

        private static void TerminalWrite(string message, bool newLine = false)
        {
            TerminalWrite(new MaestroMessage(message), newLine);
        }

        private static void TerminalWrite(MaestroMessage message, bool newLine = false)
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

        private static bool IsInvokerAuthorized(object invoker)
        {
            if (invoker == null || !_Instance._authorizedInvokers.Contains(invoker))
            {
                MaestroLogger.PrintWarning(BuiltInMessages.InvokerNotAuthorizedWarning, invoker!);
                return false;
            }
            return true;
        }

        private static string GenerateSelfInvoker()
        {
            long unixTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Guid guid = Guid.NewGuid();
            return string.Format("s-u{0}_g{1}", unixTime, guid);
        }

        public static bool RequestPushMessage(string message, object requester)
        {
            return RequestPushMessage(new MaestroMessage(message), requester);
        }

        public static bool RequestPushMessage(MaestroMessage message, object requester)
        {
            if (!IsInvokerAuthorized(requester))
                return false;

            _Instance._messageBuffer.Enqueue(message);
            return true;
        }

        public static bool RequestExit(object requester)
        {
            if (!IsInvokerAuthorized(requester))
                return false;

            RequestDisposeTerminal(_Instance._selfInvoker);
            Environment.Exit(0);
            return true;
        }

        public static bool RequestClearTerminal(object requester)
        {
            if (!IsInvokerAuthorized(requester))
                return false;

            Console.Clear();
            return true;
        }

        public static bool RequestDisposeTerminal(object requester)
        {
            if (!IsInvokerAuthorized(requester))
                return false;
            _Instance!._logger.Dispose();
            _Instance._selfInvoker = null!;
            _Instance = null!;
            _IsProcessing = false;
            return true;
        }

        public static void Close() 
        {
            RequestClearTerminal(_Instance!._selfInvoker);
            RequestDisposeTerminal(_Instance._selfInvoker);
        }
    }
}

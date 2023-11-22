using System.Runtime.Serialization;
using System.Text;

namespace Maestro
{
    public class RWProvider 
    {
        public readonly Action<object> charWriter;
        public readonly Action<object> lineWriter;
        public readonly Func<object> lineReader;

        public RWProvider(Action<object> charWriter, Action<object> lineWriter, Func<object> lineReader) 
        {
            this.charWriter = charWriter;
            this.lineWriter = lineWriter;
            this.lineReader = lineReader;
        }
    }

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
        private Action<object> _charWriter;
        private Action<object> _lineWriter;
        private Func<object> _lineReader;
        private MaestroCommandHandler _commandHandler;

        public static readonly CommandAction[] DEFAULT_COMMAND_ACTIONS  = 
        {
            new Command_CLS(),
            new Command_Exit(),
            new Command_Help()
        };

        private MaestroTerminal(string title, RWProvider rwProvider, params CommandAction[] cmdActions)
        {
            _title = title;
            _charWriter = rwProvider.charWriter;
            _lineWriter = rwProvider.lineWriter;
            _lineReader = rwProvider.lineReader;

            _logger = new MaestroLogger();
            _messageBuffer = new Queue<MaestroMessage>();
            _authorizedInvokers = new HashSet<object>();

            Console.Title = _title;
            _selfInvoker = GenerateSelfInvoker();
            _authorizedInvokers.Add(_selfInvoker);
            _authorizedInvokers.Add(_logger);
            _commandHandler = new MaestroCommandHandler(cmdActions == null || cmdActions.Length <=0 ? DEFAULT_COMMAND_ACTIONS : cmdActions);
        }

        public static void Initiate(string title, RWProvider rwProvider, CommandAction[] cmdActions)
        {
            if (_Instance == null && !_IsProcessing)
            {
                _IsProcessing = true;
                _Instance = new MaestroTerminal(title, rwProvider, cmdActions);
            }
            else
            {
                MaestroLogger.PrintError(BuiltInMessages.TerminalAlreadyExistsError, _Instance!._title);
                return;
            }
            TerminalWrite(_Instance._introMessage);
            StartMainLoop();
        }

        public static void InitiateThread(string title, RWProvider rwProvider, CommandAction[] cmdActions)
        {
            if (_Instance == null && !_IsProcessing)
            {
                _IsProcessing = true;
                _Instance = new MaestroTerminal(title, rwProvider, cmdActions);
            }
            else
            {
                MaestroLogger.PrintError(BuiltInMessages.TerminalAlreadyExistsError, _Instance!._title);
                return;
            }
            TerminalWrite(_Instance._introMessage);
            new Thread(StartMainLoop).Start();
        }

        private static void StartMainLoop()
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
            string input = (string)_Instance._lineReader()!;
            if (!string.IsNullOrEmpty(input))
                _Instance._commandHandler.ParseAndExecute(input, _Instance._selfInvoker);
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
                _Instance._lineWriter.Invoke(message.message);
            else
                _Instance._charWriter.Invoke(message.message);
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

        internal static bool RequestPushMessage(string message, object requester)
        {
            return RequestPushMessage(new MaestroMessage(message), requester);
        }

        internal static bool RequestPushMessage(MaestroMessage message, object requester)
        {
            if (!IsInvokerAuthorized(requester))
                return false;

            _Instance._messageBuffer.Enqueue(message);
            return true;
        }

        private static bool RequestExit(object requester)
        {
            if (!IsInvokerAuthorized(requester))
                return false;

            RequestDisposeTerminal(_Instance._selfInvoker);
            Environment.Exit(0);
            return true;
        }

        private static bool RequestClearTerminal(object requester)
        {
            if (!IsInvokerAuthorized(requester))
                return false;

            Console.Clear();
            return true;
        }

        private static bool RequestDisposeTerminal(object requester)
        {
            if (!IsInvokerAuthorized(requester))
                return false;

            _Instance._running = false;
            _Instance._messageBuffer.Clear();
            _Instance!._logger.Dispose();
            _Instance._selfInvoker = null!;
            _Instance = null!;
            _IsProcessing = false;
            return true;
        }

        private static bool RequestPrintCommands(object requester) 
        {
            if (!IsInvokerAuthorized(requester))
                return false;
            StringBuilder buffer = new StringBuilder();
            buffer.Append("List of commands:\n   ---------------------\n");
            foreach (var c in _Instance._commandHandler.commandActions)
            {
                string info = string.Format("       {0}\n" +
                                            "       {1} arguments\n" +
                                            "       {2}\n" +
                                            "      _________________\n", 
                                            c.Keyword, c.RequiredArgCount,c.Description);
                buffer.Append(info);
            }
            MaestroLogger.PrintInfo(buffer.ToString(), null!);
            return true;
        }

        private sealed class Command_CLS : CommandAction
        {
            public override string Keyword => "cls";
            public override uint RequiredArgCount => 0;
            public override string Description => "Clears the terminal.";

            public override bool Invoke(object invoker, string[] args)
            {
                return RequestClearTerminal(invoker);
            }
        }

        private sealed class Command_Exit : CommandAction
        {
            public override string Keyword => "exit";
            public override uint RequiredArgCount => 0;
            public override string Description => "Exits the terminal environment.";

            public override bool Invoke(object invoker, string[] args)
            {
                return RequestExit(invoker);
            }
        }

        private sealed class Command_Help : CommandAction
        {
            public override string Keyword => "help";
            public override uint RequiredArgCount => 0;
            public override string Description => "Prints all executable commands and relevant information.";

            public override bool Invoke(object invoker, string[] args)
            {
                return RequestPrintCommands(invoker);
            }
        }
    }
}

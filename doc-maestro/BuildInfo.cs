namespace Maestro
{
    public static class BuildInfo
    {
        public const string VERSION = "1.0.0";
        public const string AUTHOR = "Shariar Papon";

        public static string ToString()
            => string.Format("| author: {0} | veriosn: {1} |", AUTHOR, VERSION);
    }
}

namespace Everime.Maestro.Legacy
{
    public static class BuildInfo
    {
        public const string VERSION = "u1.0.0";
        public const string AUTHOR = "Shariar Papon";

        public new static string ToString()
            => string.Format("veriosn: {1}\nauthor: {0}", AUTHOR, VERSION);
    }
}

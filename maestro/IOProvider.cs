namespace Everime.Maestro
{
    /// <summary>
    /// Provides the input/output functionality for the terminal.
    /// </summary>
    public sealed class IOProvider
    {
        /// <summary>
        /// This is the input provider for the terminal
        /// </summary>
        /// <returns>Should be the input the terminal will listern for.</returns>
        public delegate string TerminalInputReader();

        /// <summary>
        /// This is the output provider for the terminal.
        /// </summary>
        /// <param name="output">The output of the executed command.</param>
        public delegate void TerminalOutputWriter(string output);

        private readonly TerminalInputReader _reader;
        private readonly TerminalOutputWriter _writer;
        private readonly System.Action _inputCleaner;

        /// <param name="inputReader">The input provider for the terminal.</param>
        /// <param name="outputWriter">The output provider for the terminal.</param>
        /// <param name="inputCleaner">The action to clear the input terminal.</param>
        public IOProvider(TerminalInputReader inputReader, TerminalOutputWriter outputWriter, System.Action inputCleaner)
        {
            if (inputReader == null)
                throw new System.Exception("Input reader cannot be null.");
            if (outputWriter == null)
                throw new System.Exception("Output writer cannot be null.");
            if (inputCleaner == null)
                throw new System.Exception("Input Clearer cannot be null.");

            _reader = inputReader;
            _writer = outputWriter;
            this._inputCleaner = inputCleaner;
        }

        /// <returns>The read input</returns>
        public string Input() => _reader.Invoke()!;
        
        /// <summary>
        /// Prints the given string to the terminal output.
        /// </summary>
        /// <param name="output">The string to print out.</param>
        public void Output(string output) => _writer.Invoke(output);

        /// <summary>
        /// Clears the terminal input field.
        /// </summary>
        public void ClearInput() => _inputCleaner.Invoke();
    }
}

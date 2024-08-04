namespace Everime.Maestro
{
    /// <summary>
    /// Provides description for the command.
    /// </summary>
    public interface ICommandDescriptionProvider 
    {
        /// <summary>
        /// Description to provide.
        /// </summary>
        public string Description { get; }
    }
}
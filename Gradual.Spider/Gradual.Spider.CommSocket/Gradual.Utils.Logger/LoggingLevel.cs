namespace Gradual.Utils
{
    /// <summary>
    /// Represents different logging levels.
    /// </summary>
    public enum LoggingLevel : byte
    {
        /// <summary>
        /// Used for debugging.
        /// </summary>
        Debug,
        /// <summary>
        /// Used for informational purposes.
        /// </summary>
        Info,
        /// <summary>
        /// Flag as a warning.
        /// </summary>
        Warning,
        /// <summary>
        /// Flag as an error.
        /// </summary>
        Error,
        /// <summary>
        /// Flag as fatal.
        /// </summary>
        Fatal
    }
}
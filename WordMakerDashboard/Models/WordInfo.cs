namespace WordMakerDashboard.Models
{
    /// <summary>
    /// Represents a word and its attributes.
    /// </summary>
    public class WordInfo
    {
        /// <summary>
        /// Represents the word.
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// Represents the definition of the word.
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Represents an example of usage of the word.
        /// </summary>
        public string Example { get; set; }
    }
}